using UnityEngine;
using System.Collections;

public class PlayerBullet : BulletBase {

    [SerializeField]
    private float _MaxLifeTime = 2f;

    public override void Start ()
    {
        Destroy(gameObject, _MaxLifeTime);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        Rigidbody targetRigidBody = other.GetComponent<Rigidbody>();

        if (!targetRigidBody)
            return;

        MonsterManager targetHealth = targetRigidBody.GetComponent<MonsterManager>();

        if (targetHealth)
        {
            targetHealth.TakeDamage(Damage);
            Destroy(gameObject);
            return;
        }

        MonsterBullet bulletCol = targetRigidBody.GetComponent<MonsterBullet>();
        if (bulletCol)
        {
            Destruct();
            return;
        }

        // we must have hit the floor.
        Destroy(gameObject);
    }

    public override void Update () {
	
	}
}
