using UnityEngine;
using System.Collections;

public class PlayerBullet : BulletBase {

    [SerializeField]
    private float _MaxLifeTime = 2f;

    public override void Start ()
    {
        Destroy(gameObject, _MaxLifeTime);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRigidBody = other.GetComponent<Rigidbody>();

        if (!targetRigidBody)
            return;

        MonsterManager targetHealth = targetRigidBody.GetComponent<MonsterManager>();

        if (targetHealth)
        {
            targetHealth.TakeDamage(Damage);
            Destroy(gameObject);
        }

        MonsterBullet bulletCol = targetRigidBody.GetComponent<MonsterBullet>();
        if(bulletCol)
        {
            Destruct();
        }
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        OnTriggerEnter(collision.collider);
    }

    public override void Update () {
	
	}
}
