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

        if (!targetHealth)
            return;

        targetHealth.TakeDamage(Damage);

        Destroy(gameObject);
    }

    public override void Update () {
	
	}
}
