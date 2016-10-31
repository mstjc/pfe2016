using UnityEngine;
using System.Collections;
using System;

public class Gun : MonoBehaviour {

    [SerializeField]
    private GameObject _Hand;
    [SerializeField]
    private Rigidbody _Shell;
    [SerializeField]
    private Transform _FireDirection;
    [SerializeField]
    private float _ShellLaunchForce;

    public event EventHandler Fired;

    private bool _IsReloaded = false;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetGunVisibility(bool boolean)
    {
        //_Hand.SetActive(!boolean);
        gameObject.SetActive(boolean);
    }

    public void Fire()
    {
        if(gameObject.activeInHierarchy)
        //if(_IsReloaded)
        {
            _IsReloaded = false;

            Rigidbody shellInstance = Instantiate(_Shell, _FireDirection.position, _FireDirection.rotation) as Rigidbody;

            shellInstance.velocity = _ShellLaunchForce * _FireDirection.forward;

            var particle = GetComponentInChildren<ParticleSystem>();
            Debug.Log("particle: " + particle);
            if(particle != null)
            {
                particle.Play();
            }

            OnFired();

        }
    }

    private void OnFired()
    {
        if(Fired != null)
        {
            Fired(this, EventArgs.Empty);
        }
    }


    public void Reload()
    {
        _IsReloaded = true;
    }
}
