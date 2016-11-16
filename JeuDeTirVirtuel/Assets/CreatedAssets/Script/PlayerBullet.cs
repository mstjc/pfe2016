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

        if (monsterHealth)
        {
            monsterHealth.TakeDamage(_Damage);
            Destroy(gameObject);
        }
        else if (other.GetComponent<MonsterBullet>())
            Destroy(gameObject);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
       
    }
}
