using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    [SerializeField]
    private GameObject _Hand;
    [SerializeField]
    private Rigidbody _Shell;
    [SerializeField]
    private Transform _FireDirection;
    [SerializeField]
    private float _ShellLaunchForce;

    private bool _IsReloaded = false;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetGunVisibility(bool boolean)
    {
        _Hand.SetActive(!boolean);
        gameObject.SetActive(boolean);
    }

    public void Fire()
    {
        //if(_IsReloaded)
        {
            _IsReloaded = false;

            Rigidbody shellInstance = Instantiate(_Shell, _FireDirection.position, _FireDirection.rotation) as Rigidbody;

            shellInstance.velocity = _ShellLaunchForce * _FireDirection.forward;
        }
    }

    public void Reload()
    {
        _IsReloaded = true;
    }
}
