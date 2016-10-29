using UnityEngine;
using System.Collections;
using System;

public abstract class BulletBase : MonoBehaviour, IBullet {

    private float _Damage = 1f;
    public float Damage { get { return _Damage; } set { _Damage = value; } }

    public float _MaxRange = 50.0f;
    public float _TimeAliveAfterCollision = 0.75f;
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
