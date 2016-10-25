using UnityEngine;
using System.Collections;
using System;

public class MonsterManager : MonoBehaviour {

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
    private float _ShootingTime = 1.0f;

    [SerializeField]
    private MovementEnum _MovementType;

    [SerializeField]
    private IMovement _Movement;

    // Strength from 1 to 100
    //   1 : One shot kill
    // 100 : One hundred shots to kill
    [SerializeField]
    private int _Strength = 5;

    [SerializeField]
    public GameObject _Target;

    #endregion

    #region Fields

    Animator _anim;
    private bool _BeenHit = false;
    private float _CurrentHealth;
    private bool _Firing = false;
    private Rigidbody _RigidBody;

    private RandomTimer _ShootTimer;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody>();

        _Movement = MonsterFactory.CreateMovement(_MovementType);
    }

    private void OnEnable()
    {
        _RigidBody.isKinematic = false;
    }

    private void OnDisable()
    {
        _RigidBody.isKinematic = true;

        _ShootTimer.OnTimerTick -= OnShootTimerTick;
        _Movement.Disable();

        _ShootTimer.StopTimer();
    }

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();

        _CurrentHealth = _MaximumHealth;

        _ShootTimer = new RandomTimer(_MinIntervalShot, _MaxIntervalShot);

        _ShootTimer.OnTimerTick += OnShootTimerTick;

        _ShootTimer.StartTimer();

        _Movement.InitializeValues(transform.root.gameObject, _Target, _MinDistanceFromTarget);
        _Movement.Enable();
    }
	
	// Update is called once per frame
	void Update () {

        _ShootTimer.Update(Time.deltaTime);


        UpdateHit();

        if (!_BeenHit && !_Firing && _CurrentHealth > 0.0f)
            _Movement.Move(Time.deltaTime);
	}

    void FixedUpdate()
    {
        
        _anim.SetBool("Shooting", _Firing);
        _anim.SetBool("BeenHit", _BeenHit);
        _anim.SetFloat("Health", _CurrentHealth);

        _Movement.UpdateAnimation();
    }

    #endregion

    #region Private Methods

    private bool HasCollision()
    {
        // TODO real collision with collider
        return Input.GetKeyDown(KeyCode.Space);
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

    private IEnumerator UpdateShooting(float time)
    {
        yield return new WaitForSeconds(time);

        if (!_ShootTimer.Started)
        {
            _ShootTimer.StartTimer();
        }

        _Firing = false;
    }

    #endregion

    #region Event Handlers

    private void OnShootTimerTick()
    {
        _Firing = true;
        StartCoroutine(UpdateShooting(_ShootingTime));
    }

    #endregion

}
