using UnityEngine;
using System.Collections;

public class ArcMovement : MovementBase
{
    #region Fields
    private float _MaxAngle = Mathf.PI / 2;
    private float _StartPosition;
    private float _EndPosition;
    private float _CurrentSpeed;
    private float _CurrentAngle;
    private float _Radius = 50.0f;
    private bool _RightMoving = true;
    private bool _Moving;
    private RandomTimer _WalkTimer;

    [SerializeField]
    protected float _ArcAngle = Mathf.PI/3;

    [SerializeField]
    protected float _MaxSpeed;

    [SerializeField]
    private float _MinMovingFreq;

    [SerializeField]
    private float _MaxMovingFreq;

    #endregion

    #region Properties


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

    #endregion

    #region Public Methods
    public override void Start()
    {
        base.Start();
        // We assign the start position
        _StartPosition = Mathf.Atan2(Monster.transform.position.x, Monster.transform.position.z);
        // We check a random value for wether going to right or left and we assing the end position
        _EndPosition = _StartPosition + _ArcAngle;
        if (Random.value < 0.5f)
        {
            _EndPosition = _StartPosition - _ArcAngle;
            _RightMoving = false;
        }
        // We check if angle limits are broken. in which case, we reverse the arc movement
        /*if( (_EndPosition < _StartPosition && _RightMoving) || (_StartPosition > _EndPosition && !_RightMoving) )
        {
             _RightMoving = !(_RightMoving);
        }*/
        // We set the speed and the starting angle 
        _CurrentSpeed = _ArcAngle / _MaxSpeed;
        _CurrentAngle = _StartPosition;
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
                _CurrentSpeed = _ArcAngle / _MaxSpeed;
            }

            if (_Moving)
            {
                // We check the position at each frame. if it has done its angular movement, we make the monster go back in a loop
                if(_CurrentAngle > _EndPosition)
                {
                    _RightMoving = false;
                }
                else if(_CurrentAngle < _StartPosition)
                {
                    _RightMoving = true;
                }
                if(_RightMoving)
                {
                    _CurrentAngle += _CurrentSpeed * Time.deltaTime;
                }
                else
                {
                    _CurrentAngle -= _CurrentSpeed * Time.deltaTime;
                }
                Monster.transform.position = new Vector3(Mathf.Sin(_CurrentAngle) * _Radius, Monster.transform.position.y, Mathf.Cos(_CurrentAngle) * _Radius);
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
