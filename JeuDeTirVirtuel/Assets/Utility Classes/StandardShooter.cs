using UnityEngine;
using System.Collections;

public class StandardShooter : TimedShooter {

    public Rigidbody _Bullet;
    public float _ForceApplied = 5000.0f;

    protected override void Start () {
        base.Start();
        MaxShootingTime = 5.0f;
        MinShootingTime = 3.0f;
        ShootingTime = 1.0f;
    }

    public override void Shoot(Vector3 direction)
    {
        base.Shoot(direction);
        if (CanShoot && _Bullet != null)
        {
            var initPos = transform.position + transform.forward*5;
            initPos.y += 5.0f;
            Rigidbody shot = Instantiate(_Bullet, initPos, transform.rotation) as Rigidbody;
            shot.AddForce(transform.forward * _ForceApplied);
        }
    }
}
