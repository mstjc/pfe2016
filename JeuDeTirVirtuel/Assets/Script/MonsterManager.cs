using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MonsterManager : MonoBehaviour {

    #region SerializedFields

    [SerializeField]
    private float _MaximumHealth = 100;

    private Slider _Slider;
    private Image _SliderImage;

    private IMovement _Movement;

    private IShooter _Shooter;

    // Strength from 1 to 100
    //   1 : One shot kill
    // 100 : One hundred shots to kill
    [SerializeField]
    private int _Strength = 5;

    [NonSerialized]
    public GameObject _Target;

    [SerializeField]
    private GameManager _GM;

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

        _Movement = GetComponent(typeof(IMovement)) as IMovement;
        _Shooter = GetComponent(typeof(IShooter)) as IShooter;
    }

    private void OnEnable()
    {
        _RigidBody.isKinematic = false;
    }

    private void OnDisable()
    {
        _RigidBody.isKinematic = true;
    }

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
        _Slider = GetComponentInChildren<Slider>();
        FindImage();
        
        if(_Movement != null)
        {
            _Movement.Target = _Target;
        }

        _CurrentHealth = _MaximumHealth;
        SetHealthUI();
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

        if(_Movement != null)
        {
            _Movement.CanMove = !_BeenHit && !isShooting && _CurrentHealth > 0.0f;
        }
	}

    void FixedUpdate()
    {
        _anim.SetBool("BeenHit", _BeenHit);
        _anim.SetFloat("Health", _CurrentHealth);
    }

    #endregion

    #region Private Methods

    private void FindImage()
    {
        if (_Slider != null)
        {
            var children = _Slider.GetComponentsInChildren<Image>();
            foreach (var child in children)
            {
                if (child.name == "Fill")
                {
                    _SliderImage = child;
                    break;
                }
            }
        }
    }

    private bool HasCollision()
    {
        // TODO real collision with collider
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void UpdateHit()
    {
        if (HasCollision() && _CurrentHealth >= 0 && !_BeenHit)
        {
            TakeDamage(1);
        }
    }

    private IEnumerator UpdateBeenHit(float time)
    {
        yield return new WaitForSeconds(time);
        _BeenHit = false;
    }

    private void SetHealthUI()
    {
        if(_Slider != null)
        {
            _Slider.value = _CurrentHealth;
            if (_SliderImage != null)
            {
                _SliderImage.color = Color.Lerp(Color.red, Color.green, _CurrentHealth / 100.0f);
            }
        }
    }

    private IEnumerator OnDeath()
    {
        // Le monstre est mort, on le met mort et on le desactive.
        _IsDead = true;
        _GM.EnnemiDied();
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    #endregion

    public void TakeDamage(float amount)
    {
        _BeenHit = true;

        float strength = _Strength;
        _CurrentHealth -= 100.0f / (strength > 0 ? strength * amount : 1);

        SetHealthUI();

        if(_CurrentHealth <= 0.0f && !_IsDead)
        {
            StartCoroutine(OnDeath());
        }

        StartCoroutine(UpdateBeenHit(0.2f));
    }
}
