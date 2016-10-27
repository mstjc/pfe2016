using UnityEngine;
using System.Collections;

public interface IShooter {
    bool CanShoot { get; set; }
    bool IsShooting { get; }
    float MinShootingTime { get; set; }
    float MaxShootingTime { get; set; }
    float ShootingTime { get; set; }

    void Shoot(Vector3 direction);
}
