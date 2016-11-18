using UnityEngine;
using System.Collections;

public class LastBossMovement : MovementBase
{
    #region Fields
    private float _MaxAngle = Mathf.PI;
    private float _LowerLimit; 
    private float _UpperLimit; 
    private float _CurrentSpeed;
    private float _CurrentAngle;
    private bool _RightMoving = true;
    private bool _Moving;
    private RandomTimer _WalkTimer;

    [SerializeField]
    protected float _TimeToCompleteArc;

    [SerializeField]
    private float _MinMovingFreq;

    [SerializeField]
    private float _MaxMovingFreq;

    #endregion

    #region Properties

    public float TimeToCompleteArc
    {
        get { return _TimeToCompleteArc; }
        set { _TimeToCompleteArc = value; }
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

    public override void Start()
    {
        base.Start();
        CanMove = false;
        // We set a start position
        _UpperLimit = 180f * Mathf.Deg2Rad;
        _LowerLimit = 0f * Mathf.Deg2Rad;

        // We check a random value for wether going to right or left
        if (Random.value < 0.5f)
        {
            _RightMoving = false;
        }

        _CurrentAngle = Mathf.Deg2Rad * Vector2.Angle(Vector2.right, new Vector2(Monster.transform.position.x, Monster.transform.position.z));
        _CurrentSpeed = _UpperLimit / _TimeToCompleteArc;

    }
    public void OnDisable()
    {
        if (_WalkTimer != null)
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
        if (CanMove)
        {
            if (!_Moving)
            {
                _CurrentSpeed = _UpperLimit / _TimeToCompleteArc;
            }

            if (_Moving)
            {
                // We check the position at each frame. if it has done its angular movement, we make the monster go back in a loop
                var oldAngle = _CurrentAngle;

                if (_CurrentAngle <= _LowerLimit)
                {
                    _RightMoving = false;
                }
                else if (_CurrentAngle >= _UpperLimit)
                {
                    _RightMoving = true;
                }

                if (_RightMoving)
                {
                    _CurrentAngle -= _CurrentSpeed * Time.deltaTime;
                }
                else
                {
                    _CurrentAngle += _CurrentSpeed * Time.deltaTime;
                }

                // Rotate around center, by (_CurrentAngle-oldAngle).
                Monster.transform.RotateAround(Vector3.zero, Vector3.down, Mathf.Rad2Deg * (_CurrentAngle - oldAngle));
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
        if (TimeToCompleteArc > 0.0f)
            _Moving = !_Moving;
    }

    #endregion
}
