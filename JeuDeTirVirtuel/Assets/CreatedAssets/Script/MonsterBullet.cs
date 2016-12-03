using UnityEngine;

public class MonsterBullet : BulletBase {

    [SerializeField]
    private float _Damage;
    [SerializeField]
    private int _Health;

	private float _CheckTime = 0.0f;
	private bool _CloseToPlayer = false;

	public override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (other.GetComponent<Shield>())
        {
            PlayMetalImpact();
			Destruct();
        }
        else if (other.GetComponent<PlayerBullet>())
        {
            TakeDamage();
            PlayBulletImpact(0.5f);
        }
        else if(player)
        {
            player.TakeDamage(_Damage);
            PlayFleshImpact(0.4f);
            Destroy(gameObject);
        }
    }

    public override void Update()
    {
        if(!_Destructing && Time.time >= _CheckTime)
        {
            _CheckTime += 1;
            if(IsLost())
            {
                Destruct();
            }
        }
    }

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!_CloseToPlayer && (Mathf.Abs(gameObject.transform.position.x) + gameObject.transform.position.z) < 10)
		{
			gameObject.GetComponent<SphereCollider>().isTrigger = false;
			_CloseToPlayer = true;
		}
	}

    private void TakeDamage()
    {
        --_Health;
        if (_Health <= 0)
            Destroy(gameObject);
    }
}
