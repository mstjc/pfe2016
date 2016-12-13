using UnityEngine;
using System.Collections;
using System;

public abstract class ShooterBase : MonoBehaviour, IShooter {

    [SerializeField]
    private float _MinShootingFreq = 4.0f;

    [SerializeField]
    private float _MaxShootingFreq = 8.0f;

    [SerializeField]
    private float _ShootingTime = 1.0f;

    public bool CanShoot { get; set; }

    public bool IsShooting { get; protected set; }

    public float MaxShootingTime { get { return _MaxShootingFreq; } set { _MaxShootingFreq = value; } }

    public float MinShootingTime { get { return _MinShootingFreq; } set { _MinShootingFreq = value; } }

    public float ShootingTime { get { return _ShootingTime; } set { _ShootingTime = value; } }

    protected Animator Anim { get { return _Anim; } }


    private Animator _Anim;

    public event EventHandler Fired;


    protected virtual void Start () {
        _Anim = GetComponent<Animator>();
    }
	

	protected virtual void Update () {
	
	}

    protected virtual void FixedUpdate()
    {
        _Anim.SetBool("Shooting", IsShooting);
    }

    public virtual void Shoot(Vector3 direction)
    {
        OnFired();

    }

    public void OnFired()
    {
        if (Fired != null)
        {
            Fired(this, EventArgs.Empty);
        }
    }
}
