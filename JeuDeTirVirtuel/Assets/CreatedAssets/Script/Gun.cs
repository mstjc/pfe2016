using UnityEngine;
using System.Collections;
using System;
using Leap.Unity;

public class Gun : MonoBehaviour {

    [SerializeField]
    private IHandModel _HandModel;
    [SerializeField]
    private Transform _MunitionBody;
    [SerializeField]
    private Transform _Munition;
    [SerializeField]
    private Rigidbody _Bullet;
    [SerializeField]
    private Rigidbody _ShellBullet;
    [SerializeField]
    private Transform _FireDirection;
    [SerializeField]
    private float _ShellLaunchForce;

    [SerializeField]
    private int _NbBulletsInShell = 8;

    private int _NbBullets;

    public event EventHandler Fired;
    public event EventHandler Reloaded;
    public event EventHandler FinishedReloading;
    public event EventHandler Tick;

    private bool _TriggerReload = false;
    private bool _TriggerShoot = false;
    private bool _IsReloaded = true;
    private bool _IsReloading = false;
    private bool _IsRotatingShell = false;

    float _CurAngle = 0;
    private float _EndAngle = -45F;

    private Vector3 _OriginalPosition;
    private Vector3 _OriginalRotation;

    ArrayList _MunitionBullets = new ArrayList();

    private Animator _Animator;

    public int NbBullets
    {
        get { return _NbBullets; }
    }

    public int NbBulletsInShell
    {
        get { return _NbBulletsInShell; }
    }

    void Awake()
    {
        _OriginalPosition = gameObject.transform.position;
        _OriginalRotation = gameObject.transform.localEulerAngles;
        _NbBullets = _NbBulletsInShell;
        _IsReloaded = true;
        _Animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(_IsReloading)
        {
            StartCoroutine(FinishReloading());
        }
    }

    private void OnDisable()
    {
    }

    public void SetGunVisibility(bool boolean)
    {
        gameObject.SetActive(boolean);
    }

    public void Update()
    {
        if(_IsRotatingShell)
            RotateShell();
    }

    public void FixedUpdate()
    {
        if(_Animator != null && isActiveAndEnabled)
        {
            if(_TriggerReload)
            {
                _Animator.SetTrigger("Reload");
                _TriggerReload = false;
            }
            else if(_TriggerShoot)
            {
                _Animator.SetTrigger("Shoot");
                _TriggerShoot = false;
            }
        }
    }

    public void Fire()
    {
        if(gameObject.activeInHierarchy)
        {
            if (!_IsReloading && _IsReloaded)
            {
                Rigidbody shellInstance = Instantiate(_Bullet, _FireDirection.position, _FireDirection.rotation) as Rigidbody;

                shellInstance.velocity = _ShellLaunchForce * _FireDirection.forward;

                var particle = GetComponentInChildren<ParticleSystem>();
                if (particle != null)
                {
                    particle.Play();
                }

                if (_Animator != null && !_IsReloading)
                {
                    _TriggerShoot = true;
                }

                _NbBullets--;

                OnFired();

                if (_NbBullets <= 0)
                {
                    _IsReloaded = false;
                }

            }
            else
            {
                Reload();
            }
        }
    }

    private void OnFired()
    {
        if(Fired != null)
        {
            Fired(this, EventArgs.Empty);
        }
    }

    private void OnReload()
    {
        if (Reloaded != null)
        {
            Reloaded(this, EventArgs.Empty);
        }
    }


    private void OnFinishReloading()
    {
        if (FinishedReloading != null)
        {
            FinishedReloading(this, EventArgs.Empty);
        }
    }

    private void OnTick()
    {
        if (Tick != null)
        {
            Tick(this, EventArgs.Empty);
        }
    }

    public void Reload()
    {
        lock(_Animator)
        {
            if(_Animator != null && !_IsReloading)
            {
                _IsReloading = true;
                _TriggerReload = true;
                _MunitionBody.localRotation = Quaternion.Euler(0, 0, 0);
                OnReload();
                StartCoroutine(FinishReloading());
            }
        }
    }

    public void InstiantiateMunitionBullets()
    {
        float angle = 0;
        while (angle <= 2 * Mathf.PI)
        {
            float r = 0.011f;
            float xOffset = 0;
            float zOffset = r * Mathf.Cos(angle);
            float yOffset = r * Mathf.Sin(angle);
            Vector3 pos = _Munition.TransformPoint(new Vector3(xOffset, yOffset, zOffset));
            Rigidbody shellInstance = Instantiate(_ShellBullet, pos, Quaternion.identity) as Rigidbody;
            shellInstance.transform.parent = _Munition;

            _MunitionBullets.Add(shellInstance);

            angle += (Mathf.PI / 4);
        }
    }

    public void DropMunitionBullets()
    {
        foreach(var shellObject in _MunitionBullets)
        {
            var shell = shellObject as Rigidbody;
            if(shell != null)
            {
                StartCoroutine(DropBullet(shell));
            }
        }
        _MunitionBullets.Clear();
    }

    public void BeginRotateShell()
    {
        if(_IsRotatingShell)
        {
            _CurAngle = _EndAngle;
            _EndAngle -= 45F;
        }
        _IsRotatingShell = true;
    }

    private void RotateShell()
    {
        if(_CurAngle > _EndAngle)
        {
            var delta = Time.deltaTime * 400;
            _MunitionBody.transform.Rotate(-delta, 0, 0);
            _CurAngle -= delta;
        }
        else
        {
            _CurAngle = _EndAngle;
            _EndAngle -= 45F;
            _IsRotatingShell = false;
        }
    }

    private IEnumerator DropBullet(Rigidbody shell)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.05f));
        if(shell != null)
        {
            shell.transform.parent = null;
            shell.isKinematic = false;
        }

    }

    private IEnumerator FinishReloading()
    {
        yield return new WaitForSeconds(1.0f);
        _NbBullets = _NbBulletsInShell;
        _IsReloaded = true;
        _IsReloading = false;
        OnFinishReloading();
    }
}
