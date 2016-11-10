using UnityEngine;
using System;

public class GunSoundManager : MonoBehaviour {

    [SerializeField]
    private Gun _LeftGun;

    [SerializeField]
    private AudioClip _LeftGunClip;

    [SerializeField]
    private AudioClip _LeftGunReloadClip;

    [SerializeField]
    private AudioClip _LeftGunEmptyClip;

    [SerializeField]
    private Gun _RightGun;

    [SerializeField]
    private AudioClip _RightGunClip;

    [SerializeField]
    private AudioClip _RightGunReloadClip;

    [SerializeField]
    private AudioClip _RightGunEmptyClip;

    [SerializeField]
    private float _SoundLevel = 1.0f;

    private float _pitchRange = 0.05f;

    void OnEnable()
    {
        if (_LeftGun != null)
        {
            _LeftGun.Fired += OnLeftGunFired;
            _LeftGun.Tick += OnLeftGunTick;
            _LeftGun.Reloaded += OnLeftGunReloaded;
        }

        if (_RightGun != null)
        {
            _RightGun.Fired += OnRightGunFired;
            _RightGun.Tick += OnRightGunTick;
            _RightGun.Reloaded += OnRightGunReloaded;
        }

    }

    void OnDisable()
    {
        if (_LeftGun != null)
        {
            _LeftGun.Fired -= OnLeftGunFired;
            _LeftGun.Tick -= OnLeftGunTick;
            _LeftGun.Reloaded -= OnLeftGunReloaded;
        }

        if (_RightGun != null)
        {
            _RightGun.Fired -= OnRightGunFired;
            _RightGun.Tick -= OnRightGunTick;
            _RightGun.Reloaded -= OnRightGunReloaded;
        }
    }

    private void OnLeftGunFired(object sender, EventArgs e)
    {
        PlayClipAtPoint(_LeftGunClip, _LeftGun.transform.position, _SoundLevel, UnityEngine.Random.Range(1 - _pitchRange, 1 + _pitchRange));
    }

    private void OnRightGunFired(object sender, EventArgs e)
    {
        PlayClipAtPoint(_RightGunClip, _RightGun.transform.position, _SoundLevel, UnityEngine.Random.Range(1 - _pitchRange, 1 + _pitchRange));
    }

    private void OnLeftGunReloaded(object sender, EventArgs e)
    {
        PlayClipAtPoint(_LeftGunReloadClip, _LeftGun.transform.position, _SoundLevel, 1);
    }

    private void OnRightGunReloaded(object sender, EventArgs e)
    {
        PlayClipAtPoint(_RightGunReloadClip, _RightGun.transform.position, _SoundLevel, 1);
    }

    private void OnLeftGunTick(object sender, EventArgs e)
    {
        PlayClipAtPoint(_LeftGunEmptyClip, _LeftGun.transform.position, _SoundLevel, 1);
    }

    private void OnRightGunTick(object sender, EventArgs e)
    {
        PlayClipAtPoint(_RightGunEmptyClip, _RightGun.transform.position, _SoundLevel, 1);
    }

    // Source : https://forum.unity3d.com/threads/audiosource-pitch-question-c.164374/
    GameObject PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        GameObject obj = new GameObject();
        obj.transform.position = position;
        obj.AddComponent<AudioSource>();
        var audio = obj.GetComponent<AudioSource>();
        audio.pitch = pitch;
        audio.volume = volume;
        audio.spatialBlend = 1.0f;
        audio.PlayOneShot(clip, volume);
        Destroy(obj, clip.length / pitch);
        return obj;
    }

}
