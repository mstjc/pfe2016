using UnityEngine;
using System.Collections;
using System;

public class MonsterManager : MonoBehaviour {

    #region SerializedFields

    [SerializeField]
    private float _MaximumHealth = 100;

    [SerializeField]
    private float _MinDistanceFromTarget = 5.0f;

    [SerializeField]
    private MovementEnum _MovementType;

    [SerializeField]
    private IMovement _Movement;

    //[SerializeField]
    private IShooter _Shooter;

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
    private bool _IsDead = false;
    private float _CurrentHealth;
    private Rigidbody _RigidBody;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody>();

        _Movement = MonsterFactory.CreateMovement(_MovementType);
        _Shooter = GetComponent(typeof(IShooter)) as IShooter;
    }

    private void OnEnable()
    {
        _RigidBody.isKinematic = false;
    }

    private void OnDisable()
    {
        _RigidBody.isKinematic = true;


        _Movement.Disable();
    }

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();

        _CurrentHealth = _MaximumHealth;

        _Movement.InitializeValues(transform.root.gameObject, _Target, _MinDistanceFromTarget);
        _Movement.Enable();
    }
	
	// Update is called once per frame
	void Update () {

        UpdateHit();

        var isShooting = false;
        if(_Shooter != null)
        {
            _Shooter.CanShoot = !_BeenHit && _CurrentHealth > 0.0f;
            isShooting = _Shooter.IsShooting;
        }
        if (!_BeenHit && !isShooting && _CurrentHealth > 0.0f)
            _Movement.Move(Time.deltaTime);
	}

    void FixedUpdate()
    {
       
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

    private void SetHealthUI()
    {
        // Ajuster le slider de vie du monstre
    }

    private void OnDeath()
    {
        // Le monstre est mort, on le met mort et on le desactive.
        _IsDead = true;
        gameObject.SetActive(false);
    }

    #endregion

    public void TakeDamage(float amount)
    {
        _CurrentHealth -= amount;

        SetHealthUI();

        if(_CurrentHealth <= 0f && _IsDead)
        {
            OnDeath();
        }
    }
}
