using UnityEngine;

public class LastBossShooter : StandardShooter
{

    public bool BossPhaseStarted
    {
        get;
        set;
    }

    protected override void Start ()
    {
        base.Start();
        BossPhaseStarted = false;
	}

    public override void Shoot(Vector3 direction)
    {
        if (BossPhaseStarted)
            base.Shoot(direction);
    }

    protected override void FixedUpdate()
    {
        if (BossPhaseStarted)
            base.FixedUpdate();
    }
}
