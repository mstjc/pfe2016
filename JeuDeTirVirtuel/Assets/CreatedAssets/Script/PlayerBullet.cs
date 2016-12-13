using UnityEngine;
using System.Collections;

public class PlayerBullet : BulletBase {

    [SerializeField]
    private float _MaxLifeTime = 2f;
    private float _Damage = 1;

    

    public override void Start ()
    {
        Destroy(gameObject, _MaxLifeTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        MonsterManager monsterHealth = other.GetComponent<MonsterManager>();
        // We check which collision occured
        if (monsterHealth)
        {
            monsterHealth.TakeDamage(_Damage);
            PlayFleshImpact(1.0f);
            Destroy(gameObject);
        }
        else if (other.GetComponent<MonsterBullet>() || other.GetComponent<Wall>())
        {
            PlayBulletImpact(1.0f);
            Destroy(gameObject);
        }
        else if (other.name.Equals("Terrain")) 
        {
            PlayGroundImpact();
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
       
    }
}
