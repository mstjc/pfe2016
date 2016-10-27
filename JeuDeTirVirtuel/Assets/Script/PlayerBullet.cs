using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

    [SerializeField]
    private float _Damage = 1f;
    [SerializeField]
    private float _MaxLifeTime = 2f;

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, _MaxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRigidBody = other.GetComponent<Rigidbody>();

        if (!targetRigidBody)
            return;

        MonsterManager targetHealth = targetRigidBody.GetComponent<MonsterManager>();

        if (!targetHealth)
            return;

        targetHealth.TakeDamage(_Damage);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
