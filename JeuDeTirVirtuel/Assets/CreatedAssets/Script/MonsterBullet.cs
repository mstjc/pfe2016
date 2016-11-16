using UnityEngine;

public class MonsterBullet : BulletBase {

    [SerializeField]
    private float _Damage;
    [SerializeField]
    private int _Health;

    private float _CheckTime = 0.0f;

    public override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (other.GetComponent<Shield>())
        {
            // Fuck yeah
            gameObject.GetComponent<SphereCollider>().isTrigger = false;
            Destruct();
        }
        else if (other.GetComponent<PlayerBullet>())
            TakeDamage();
        else if(player)
        {
            player.TakeDamage(_Damage);
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

    private void TakeDamage()
    {
        --_Health;
        if (_Health <= 0)
            Destroy(gameObject);
    }
}
