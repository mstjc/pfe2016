using UnityEngine;
using System;

public class StraightMovement : IMovement
{
    #region Fields

    private float _CurrentSpeed;
    private float _MinDistanceToTarget;
    private GameObject _Monster;
    private bool _Moving;
    private GameObject _Target;

    private RandomTimer _WalkTimer;

    #endregion

    #region Properties

    /// <summary>
    /// Get the max speed of the monster
    /// </summary>
    public float MaxSpeed
    {
        get;
        protected set;
    }

    public float MinMovingFreq
    {
        get;
        protected set;
    }

    public float MaxMovingFreq
    {
        get;
        protected set;
    }

    public float MinDistanceToTarget
    {
        get
        {
            return _MinDistanceToTarget;
        }
        set
        {
            _MinDistanceToTarget = value;
        }
    }

    public GameObject Monster
    {
        get
        {
            return _Monster;
        }
        set
        {
            _Monster = value;
        }
    }

    public Animator MonsterAnimator
    {
        get
        {
            return _Monster != null ? _Monster.GetComponent<Animator>() : null;
        }
    }

    public GameObject Target
    {
        get
        {
            return _Target;
        }
        set
        {
            _Target = value;
        }
    }

    #endregion

    #region Public Methods

    public bool CanMove()
    {
        if (_Monster == null || _Target == null)
            throw new NullReferenceException();

        var distanceFromTarget = Vector3.Distance(_Monster.transform.position, _Target.transform.position);
        return distanceFromTarget > _MinDistanceToTarget;
    }

    public void Disable()
    {
        if(_WalkTimer != null)
        {
            _WalkTimer.StopTimer();
            _WalkTimer.OnTimerTick -= OnWalkTimerTick;
        }
    }

    public void Enable()
    {
        _WalkTimer = new RandomTimer(MinMovingFreq, MaxMovingFreq);
        _WalkTimer.OnTimerTick += OnWalkTimerTick;
        _WalkTimer.StartTimer();

        MoveToInitialPosition();
    }

    public virtual void InitializeValues(GameObject monster, GameObject target, float minDistanceToTarget)
    {
        _Monster = monster;
        _Target = target;
        _MinDistanceToTarget = minDistanceToTarget;
        MinMovingFreq = 3.0f;
        MaxMovingFreq = 5.0f;
        MaxSpeed = 2.0f;
    }

    public void Move(float delta)
    {
        _WalkTimer.Update(delta);

        if (CanMove())
        {
            if (!_Moving)
            {
                _CurrentSpeed = MaxSpeed;
            }

            if (_Moving)
            {
                _Monster.transform.position += _Monster.transform.forward * _CurrentSpeed * Time.deltaTime;
            }
        }
        else
        {
            _Moving = false;
            _CurrentSpeed = 0.0f;
        }
    }

    public void MoveToInitialPosition()
    {
        if (_Monster == null ||_Target == null)
            throw new NullReferenceException();

            _Monster.transform.LookAt(_Target.transform);
            _Monster.transform.position = new Vector3(_Monster.transform.position.x, 4.0f, _Monster.transform.position.z);

            var rotationVector = _Monster.transform.rotation.eulerAngles;
            rotationVector.x = 0;

            _Monster.transform.rotation = Quaternion.Euler(rotationVector);
    }

    public void UpdateAnimation()
    {
        MonsterAnimator.SetFloat("Speed", _Moving ? _CurrentSpeed : 0.0f);

        if (!_WalkTimer.Started)
            _WalkTimer.StartTimer();
    }

    #endregion

    #region Event Handlers

    private void OnWalkTimerTick()
    {
        _Moving = !_Moving;
    }

    #endregion
}
