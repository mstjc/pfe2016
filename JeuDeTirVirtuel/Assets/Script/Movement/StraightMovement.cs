using UnityEngine;
using System;

public class StraightMovement : MovementBase
{
    #region Fields

    private float _CurrentSpeed;
    private bool _Moving;
    private RandomTimer _WalkTimer;

    [SerializeField]
    protected float _MaxSpeed;

    [SerializeField]
    private float _MinDistanceToTarget;

    [SerializeField]
    private float _MinMovingFreq;

    [SerializeField]
    private float _MaxMovingFreq;

    #endregion

    #region Properties

    public virtual bool IsFarEnough
    {
        get
        {
            return Monster != null && Target != null && Vector3.Distance(Monster.transform.position, Target.transform.position) > MinDistanceToTarget;
        }
    }


    public float MaxSpeed
    {
        get { return _MaxSpeed; }
        set { _MaxSpeed = value; }
    }

    public virtual float MinDistanceToTarget
    {
        get { return _MinDistanceToTarget; }
        set { _MinDistanceToTarget = value; }
    }

    public virtual float MinMovingFreq
    {
        get { return _MinMovingFreq; }
        set { _MinMovingFreq = value; }
    }

    public virtual float MaxMovingFreq
    {
        get { return _MaxMovingFreq; }
        set { _MaxMovingFreq = value; }
    }

    #endregion

    #region Public Methods

    public void OnDisable()
    {
        if(_WalkTimer != null)
        {
            _WalkTimer.StopTimer();
            _WalkTimer.OnTimerTick -= OnWalkTimerTick;
        }
    }

    public void OnEnable()
    {
        _WalkTimer = new RandomTimer(MinMovingFreq, MaxMovingFreq);
        _WalkTimer.OnTimerTick += OnWalkTimerTick;
        _WalkTimer.StartTimer();
    }

    public override void Update()
    {
        base.Update();
        _WalkTimer.Update(Time.deltaTime);
        Move();
    }

    public override void Move()
    {
        base.Move();
        if (CanMove && IsFarEnough)
        {
            if (!_Moving)
            {
                _CurrentSpeed = MaxSpeed;
            }

            if (_Moving)
            {
                Monster.transform.position += Monster.transform.forward * _CurrentSpeed * Time.deltaTime;
            }
        }
        else
        {
            _Moving = false;
            _CurrentSpeed = 0.0f;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MonsterAnimator.SetFloat("Speed", _Moving ? _CurrentSpeed : 0.0f);

        if (!_WalkTimer.Started)
            _WalkTimer.StartTimer();
    }

    #endregion

    #region Event Handlers

    private void OnWalkTimerTick()
    {
        if (MaxSpeed > 0.0f)
            _Moving = !_Moving;
    }

    #endregion
}
