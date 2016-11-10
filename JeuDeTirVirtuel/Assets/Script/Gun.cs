using UnityEngine;
using System.Collections;
using System;
using Leap.Unity;

public class Gun : MonoBehaviour {

    [SerializeField]
    private IHandModel _HandModel;
    [SerializeField]
    private Rigidbody _Shell;
    [SerializeField]
    private Transform _FireDirection;
    [SerializeField]
    private float _ShellLaunchForce;

    [SerializeField]
    private int _NbBulletsInShell = 8;

    private int _NbBullets;

    public event EventHandler Fired;
    public event EventHandler Reloaded;
    public event EventHandler Tick;

    private bool _IsReloaded = true;
    private bool _IsReloading = false;
    private Vector3 _OriginalRotation;

    private Animator _Animator;

    void Awake()
    {
        _OriginalRotation = gameObject.transform.localEulerAngles;
        _NbBullets = _NbBulletsInShell;
        _IsReloaded = true;
        _Animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void SetGunVisibility(bool boolean)
    {
        gameObject.SetActive(boolean);
    }

    public void Fire()
    {
        if(gameObject.activeInHierarchy)
        {
            if (!_IsReloading && _IsReloaded)
            {
                Rigidbody shellInstance = Instantiate(_Shell, _FireDirection.position, _FireDirection.rotation) as Rigidbody;

                shellInstance.velocity = _ShellLaunchForce * _FireDirection.forward;

                var particle = GetComponentInChildren<ParticleSystem>();
                if (particle != null)
                {
                    particle.Play();
                }

                if (_Animator != null && !_IsReloading)
                {
                    _Animator.SetTrigger("Shoot");
                }

                OnFired();

                // TODO
                //_NbBullets--;
                if (_NbBullets <= 0)
                {
                    _IsReloaded = false;
                }

            }
            else
            {
                OnTick();
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
                _Animator.SetTrigger("Reload");
                OnReload();
            }
        }
    }

    public void FinishReloading()
    {
        Debug.Log("Finishing Reloading");
        _NbBullets = _NbBulletsInShell;
        _IsReloaded = true;
        _IsReloading = false;
    }
}
