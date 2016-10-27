using UnityEngine;
using System.Collections;
using System;

public abstract class BaseShooter : MonoBehaviour, IShooter {
    
    public bool CanShoot { get; set; }

    public bool IsShooting { get; protected set; }

    public float MaxShootingTime { get; set; }

    public float MinShootingTime { get; set; }

    public float ShootingTime { get; set; }

    protected Animator Anim { get { return _Anim; } }


    private Animator _Anim;

    // Use this for initialization
    protected virtual void Start () {
        _Anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

    protected virtual void FixedUpdate()
    {
        _Anim.SetBool("Shooting", IsShooting);
    }

    public virtual void Shoot(Vector3 direction)
    {
        // empty
    }
}
