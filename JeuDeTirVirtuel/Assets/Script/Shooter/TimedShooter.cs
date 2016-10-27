using UnityEngine;
using System.Collections;

public class TimedShooter : ShooterBase {

    private RandomTimer _ShootTimer;

    public void Enable()
    {
        _ShootTimer.OnTimerTick += OnShootTimerTick;
        _ShootTimer.StartTimer();
    }

    public void OnDisable()
    {
        _ShootTimer.OnTimerTick -= OnShootTimerTick;
        _ShootTimer.StopTimer();
    }

    private void OnShootTimerTick()
    {
        IsShooting = true;
        StartCoroutine(UpdateShooting(ShootingTime));
    }

    protected override void Start () {
        base.Start();
        _ShootTimer = new RandomTimer(MinShootingTime, MaxShootingTime);
        Enable();
    }
	
	protected override void Update () {
        base.Update();
        _ShootTimer.Update(Time.deltaTime);
    }

    private IEnumerator UpdateShooting(float time)
    {
        yield return new WaitForSeconds(time);

        if(IsShooting)
            Shoot(transform.forward);

        if (!_ShootTimer.Started)
        {
            _ShootTimer.StartTimer();
        }

        IsShooting = false;
    }
}
