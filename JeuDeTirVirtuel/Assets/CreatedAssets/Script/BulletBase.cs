using UnityEngine;
using System.Collections;
using System;

public abstract class BulletBase : MonoBehaviour, IBullet {

    public float _MaxRange = 50.0f;
    public float _TimeAliveAfterCollision = 2.0f;
    protected bool _Destructing = false;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Destruct()
    {
        _Destructing = true;
        GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(StartDestruction());
    }

    public virtual bool IsLost()
    {
        return Mathf.Abs(transform.position.x) > _MaxRange || Mathf.Abs(transform.position.z) > _MaxRange;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRigidBody = other.GetComponent<Rigidbody>();

        if (!targetRigidBody)
            return;

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Destruct();
    }

    private IEnumerator StartDestruction()
    {
        yield return new WaitForSeconds(_TimeAliveAfterCollision);
        Destroy(gameObject);
    }
}
