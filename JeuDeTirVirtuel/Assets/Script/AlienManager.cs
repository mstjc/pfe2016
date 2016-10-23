using UnityEngine;
using System.Collections;
using System;

public class AlienManager : MonoBehaviour {

    #region SerializedFields
    [SerializeField]
    private float _ForwardSpeed = 1.0f;

    [SerializeField]
    private float _MaximumHealth = 100;

    [SerializeField]
    private float _MinDistanceFromTarget = 5.0f;

    [SerializeField]
    private float _MinIntervalShot = 3.0f;

    [SerializeField]
    private float _MaxIntervalShot = 5.0f;

    [SerializeField]
    private float _MinIntervalWalk = 3.0f;

    [SerializeField]
    private float _MaxIntervalWalk = 5.0f;

    // Strength from 1 to 100
    //   1 : One shot kill
    // 100 : One hundred shots to kill
    [SerializeField]
    private int _Strength = 5;

    [SerializeField]
    private Transform _Target;

    #endregion

    #region Fields

    Animator _anim;
    private bool _BeenHit = false;
    private float _CurrentSpeed = 0.0f;
    private float _CurrentHealth;
    private bool _Firing = false;
    private bool _Forward = false;
    private Rigidbody _RigidBody;

    private RandomTimer _ShootTimer;
    private RandomTimer _WalkTimer;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _RigidBody.isKinematic = false;
    }

    private void OnDisable()
    {
        _RigidBody.isKinematic = true;

        _ShootTimer.OnTimerTick -= OnShootTimerTick;
        _WalkTimer.OnTimerTick -= OnWalkTimerTick;

        _ShootTimer.StopTimer();
        _WalkTimer.StopTimer();
    }

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
        InitPositionAndRotation();

        _CurrentHealth = _MaximumHealth;

        _ShootTimer = new RandomTimer(_MinIntervalShot, _MaxIntervalShot);
        _WalkTimer = new RandomTimer(_MinIntervalWalk, _MaxIntervalWalk);

        _ShootTimer.OnTimerTick += OnShootTimerTick;
        _WalkTimer.OnTimerTick += OnWalkTimerTick;

        _ShootTimer.StartTimer();
        _WalkTimer.StartTimer();
    }
	
	// Update is called once per frame
	void Update () {

        _ShootTimer.Update(Time.deltaTime);
        _WalkTimer.Update(Time.deltaTime);

        UpdateHit();
        UpdateMovement();
	}

    void FixedUpdate()
    {
        _anim.SetFloat("Speed", _Forward ? 1.0f : 0.0f);
        _anim.SetBool("Shooting", _Firing);
        _anim.SetBool("BeenHit", _BeenHit);
        _anim.SetFloat("Health", _CurrentHealth);

        if(!_ShootTimer.Started)
        {
            _ShootTimer.StartTimer();
            _Firing = false;
        }

        if (!_WalkTimer.Started)
            _WalkTimer.StartTimer();
    }

    #endregion

    #region Private Methods

    // Indicates if we can still move forward, or we are at too close to the player.
    private bool CanMoveForward()
    {
        var distanceFromTarget = Vector3.Distance(transform.position, _Target.transform.position);
        return distanceFromTarget > _MinDistanceFromTarget;
    }

    private bool HasCollision()
    {
        // TODO real collision with collider
        return Input.GetKeyDown(KeyCode.Space);
    }

    // Look at the target and set the current position and rotation so we move toward it.
    private void InitPositionAndRotation()
    {
        if (_Target != null)
        {
            transform.LookAt(_Target);
            transform.position = new Vector3(transform.position.x, _Target.position.y + 0.5f, transform.position.z);

            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.x = 0;

            transform.rotation = Quaternion.Euler(rotationVector);
        }
    }

    private void UpdateHit()
    {
        if (HasCollision() && _CurrentHealth >= 0 && !_BeenHit)
        {
            _BeenHit = true;
            StartCoroutine(UpdateBeenHit(0.2f));
        }
    }

    private IEnumerator UpdateBeenHit(float time)
    {
        yield return new WaitForSeconds(time);
        float strength = _Strength;
        _CurrentHealth -= 100.0f / (strength > 0 ? strength : 1);
        _BeenHit = false;
    }

    private void UpdateMovement()
    {
        if (CanMoveForward())
        {
            if (!_Forward)
            {
                _CurrentSpeed = _ForwardSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                _CurrentSpeed = 0.0f;
            }

            if (_Forward)
            {
                transform.position += transform.forward * _CurrentSpeed * Time.deltaTime;
            }
        }
        else
        {
            _Forward = false;
            _CurrentSpeed = 0.0f;
        }

    }

    #endregion

    #region Event Handlers

    private void OnShootTimerTick()
    {
        _Firing = true;
    }

    private void OnWalkTimerTick()
    {
        _Forward = !_Forward;
    }

    #endregion

}
