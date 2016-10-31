using UnityEngine;
using System;

public class GunSoundManager : MonoBehaviour {

    [SerializeField]
    private Gun _LeftGun;

    [SerializeField]
    private AudioClip _LeftGunClip;

    [SerializeField]
    private Gun _RightGun;

    [SerializeField]
    private AudioClip _RightGunClip;

    private float _pitchRange = 0.05f;

    void OnEnable()
    {
        if (_LeftGun != null)
        {
            _LeftGun.Fired += OnLeftGunFired;
        }

        if (_RightGun != null)
        {
            _RightGun.Fired += OnRightGunFired;
        }
    }

    void OnDisable()
    {
        if (_LeftGun != null)
        {
            _LeftGun.Fired -= OnLeftGunFired;
        }

        if (_RightGun != null)
        {
            _RightGun.Fired -= OnRightGunFired;
        }
    }

    private void OnLeftGunFired(object sender, EventArgs e)
    {
        PlayClipAtPoint(_LeftGunClip, _LeftGun.transform.position, 0.5f, UnityEngine.Random.Range(1 - _pitchRange, 1 + _pitchRange));
    }

    private void OnRightGunFired(object sender, EventArgs e)
    {
        PlayClipAtPoint(_RightGunClip, _RightGun.transform.position, 0.5f, UnityEngine.Random.Range(1 - _pitchRange, 1 + _pitchRange));
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
