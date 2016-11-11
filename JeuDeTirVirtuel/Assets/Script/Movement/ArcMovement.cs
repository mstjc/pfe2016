using UnityEngine;
using System.Collections;

public class ArcMovement : MovementBase
{
    [SerializeField]
    protected float _MaxSpeed;

    [SerializeField]
    private float _MinMovingFreq;

    [SerializeField]
    private float _MaxMovingFreq;

    [SerializeField]
    protected float _MaxAngle = Mathf.PI/6;

    [SerializeField]
    protected float _MaxFOV = Mathf.PI / 2;

    private const float _Radius = 50f;
    private float _StartAngle;
    private float _CurrentSpeed;
    private float _CurrentAngle = 0;
    private bool _Moving;
    private bool _RightSidedArcMovement = true;
    private RandomTimer _WalkTimer;

    public float MaxSpeed
    {
        get { return _MaxSpeed; }
        set { _MaxSpeed = value; }
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
        _CurrentSpeed = _MaxAngle / _MaxSpeed;
        _StartAngle = Mathf.Sin(Monster.transform.position.z) / Mathf.Cos(Monster.transform.position.x);
        if ((_StartAngle + _MaxAngle) > _MaxFOV)
        {
            _RightSidedArcMovement = false;
        }
        else if((_StartAngle - _MaxAngle) < _MaxFOV)
        {

        }
        else if (Random.value < 0.5f)
        {
            _RightSidedArcMovement = false;
        }
    }

    private void OnWalkTimerTick()
    {
        if (MaxSpeed > 0.0f)
            _Moving = !_Moving;
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
                _CurrentSpeed = MaxSpeed;
            }

            if (_Moving)
            {
                if(_RightSidedArcMovement)
                {
                    _CurrentAngle += _MaxSpeed * Time.deltaTime;
                }
                else
                {
                    _CurrentAngle -= _MaxSpeed * Time.deltaTime;
                }
                Monster.transform.position = new Vector3(Mathf.Cos(_CurrentAngle) * _Radius, Monster.transform.position.y, Mathf.Sin(_CurrentAngle) * _Radius);
            }
        }
        else
        {
            _Moving = false;
            _CurrentSpeed = 0.0f;
        }
    }
}
