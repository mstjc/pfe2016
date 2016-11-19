using UnityEngine;
using System.Collections;

public class LastBossShooter : TimedShooter
{
	
	protected override void Start ()
    {
        base.Start();
        CanShoot = false;
	}

}
