using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;

public abstract class BulletBase : MonoBehaviour, IBullet {

    public float _MaxRange = 50.0f;
    public float _TimeAliveAfterCollision = 2.0f;
    protected bool _Destructing = false;

    public AudioMixerGroup _Mixer;
    public AudioClip _BulletImpact;
    public AudioClip _GroundImpact;
    public AudioClip _FleshImpact;
    public AudioClip _MetalImpact;


    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Destruct()
    {
        _Destructing = true;
        GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(StartDestruction());
    }

    public virtual bool IsLost()
    {
        return Mathf.Abs(transform.position.x) > _MaxRange || Mathf.Abs(transform.position.z) > _MaxRange;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<Collider>().isTrigger)
        {
            Destruct();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Destruct();
    }

    private IEnumerator StartDestruction()
    {
        yield return new WaitForSeconds(_TimeAliveAfterCollision);
        Destroy(gameObject);
    }

    public void PlayBulletImpact()
    {
        if(_Mixer != null && _BulletImpact != null)
        {
            SoundUtil.PlayClipAtPoint(_BulletImpact, _Mixer, gameObject.transform.position, 0.5f, 1.0f);
        }
    }

    public void PlayGroundImpact()
    {
        if(_Mixer != null && _GroundImpact != null)
        {
            SoundUtil.PlayClipAtPoint(_GroundImpact, _Mixer, gameObject.transform.position, 0.5f, 1.0f);
        }
    }

    public void PlayFleshImpact(float volume)
    {
        if(_Mixer != null && _FleshImpact != null)
        {
            SoundUtil.PlayClipAtPoint(_FleshImpact, _Mixer, gameObject.transform.position, volume, 1.0f);
        }
    }

    public void PlayMetalImpact()
    {
        if (_Mixer != null && _FleshImpact != null)
        {
            SoundUtil.PlayClipAtPoint(_MetalImpact, _Mixer, gameObject.transform.position, 1.0f, 1.0f);
        }
    }
}
