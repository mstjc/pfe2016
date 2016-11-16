using UnityEngine;
using System.Collections;

public class ArcMovement : MovementBase
{
    #region Fields
    private float _MaxAngle = Mathf.PI;
    private float _SmallestAngle; // always the smallest angle
    private float _BiggestAngle; // always the biggest angle
    private float _CurrentSpeed;
    private float _CurrentAngle;
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
        // cheat ?
        BoxCollider coll = gameObject.GetComponent<BoxCollider>();
        coll.isTrigger = true;
        // We set a start position
        float startAngle = Mathf.Deg2Rad * Vector2.Angle(Vector2.right, new Vector2(Monster.transform.position.x, Monster.transform.position.z));
        
        // We check a random value for wether going to right or left
        if (Random.value < 0.5f)
        {
            _RightMoving = false;
            _SmallestAngle = startAngle;
            _BiggestAngle = _SmallestAngle + _ArcAngle;
            _CurrentAngle = _SmallestAngle;
        }
        else
        {
            _RightMoving = true;
            _BiggestAngle = startAngle;
            _SmallestAngle = _BiggestAngle - _ArcAngle;
            _CurrentAngle = _BiggestAngle;
        }
        // We check if angle limits are broken. in which case, we adjust our angles
        if(_SmallestAngle < 0)
        {
            _BiggestAngle += Mathf.Abs(_SmallestAngle);
            _SmallestAngle = 0;

        }else if(_BiggestAngle > _MaxAngle)
        {
            _SmallestAngle -= _BiggestAngle - Mathf.PI;
            _BiggestAngle = Mathf.PI;
        }
        _CurrentSpeed = _ArcAngle / _MaxSpeed;
       
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
                var oldAngle = _CurrentAngle;

                if (_CurrentAngle <= _SmallestAngle)
                {
                    _RightMoving = false;
                }else if (_CurrentAngle >= _BiggestAngle)
                {
                    _RightMoving = true;
                }

                if(_RightMoving)
                {
                    _CurrentAngle -= _CurrentSpeed * Time.deltaTime;
                }
                else
                {
                    _CurrentAngle += _CurrentSpeed * Time.deltaTime;
                }

                // Rotate around center, by (_CurrentAngle-oldAngle).
                Monster.transform.RotateAround(Vector3.zero, Vector3.down, Mathf.Rad2Deg * (_CurrentAngle-oldAngle));
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
