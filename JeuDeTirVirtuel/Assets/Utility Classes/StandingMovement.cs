using UnityEngine;
using System;

public class StandingMovement : IMovement
{
    #region Fields

    private GameObject _Monster;
    private GameObject _Target;

    #endregion

    #region Properties

    public float MinDistanceToTarget
    {
        get
        {
            return 0.0f;
        }
        set
        {
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

    public bool CanMove()
    {
        return false;
    }

    public void Disable()
    {
    }

    public void Enable()
    {
        MoveToInitialPosition();
    }

    public void InitializeValues(GameObject monster, GameObject target, float minDistanceToTarget)
    {
        _Monster = monster;
        _Target = target;
    }

    public void Move(float delta)
    {
    }

    public void MoveToInitialPosition()
    {
        if (_Monster == null || _Target == null)
            throw new NullReferenceException();

        _Monster.transform.LookAt(_Target.transform);
        _Monster.transform.position = new Vector3(_Monster.transform.position.x, 4.0f, _Monster.transform.position.z);

        var rotationVector = _Monster.transform.rotation.eulerAngles;
        rotationVector.x = 0;

        _Monster.transform.rotation = Quaternion.Euler(rotationVector);
    }

    public void UpdateAnimation()
    {
    }
}
