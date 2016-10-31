using UnityEngine;
using System;

public interface IShooter {

    event EventHandler Fired;

    bool CanShoot { get; set; }
    bool IsShooting { get; }
    float MinShootingTime { get; set; }
    float MaxShootingTime { get; set; }
    float ShootingTime { get; set; }

    void Shoot(Vector3 direction);
}
