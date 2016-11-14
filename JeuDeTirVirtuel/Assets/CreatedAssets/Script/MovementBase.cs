using UnityEngine;
using System.Collections;
using System;

public abstract class MovementBase : MonoBehaviour, IMovement {

    [NonSerialized]
    public GameObject _Target;


    public bool CanMove { get; set; }

    public GameObject Monster { get { return gameObject; } }


    public Animator MonsterAnimator
    {
        get
        {
            return Monster != null ? Monster.GetComponent<Animator>() : null;
        }
    }

    public GameObject Target
    {
        get { return _Target; }
        set { _Target = value; }
    }

    private float _TimeToCheckRotation = 1.0f;


    public virtual void Move()
    {
    }

    public virtual void LookAtTarget()
    {
        if (gameObject == null || Target == null)
            throw new NullReferenceException();

        gameObject.transform.LookAt(Target.transform);

        var rotationVector = gameObject.transform.rotation.eulerAngles;
        rotationVector.x = 0;

        gameObject.transform.rotation = Quaternion.Euler(rotationVector);
    }

    public virtual void MoveToInitialPosition()
    {
        if (gameObject == null || Target == null)
            throw new NullReferenceException();

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 4.0f, gameObject.transform.position.z);
        LookAtTarget();
    }

    public virtual void Start () {
        MoveToInitialPosition();
    }
	
	public virtual void Update () {

        if(Time.time >= _TimeToCheckRotation)
        {
            _TimeToCheckRotation++;

            float angleBetweenMonsterAndTarget = Vector3.Angle(_Target.transform.forward, transform.position - _Target.transform.position);
            if (angleBetweenMonsterAndTarget >= 1)
                LookAtTarget();
        }
	}

    public virtual void FixedUpdate()
    {

    }
}
