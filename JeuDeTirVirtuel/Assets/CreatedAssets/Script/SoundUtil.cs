using UnityEngine;
using UnityEngine.Audio;

public class SoundUtil {
    // Source : https://forum.unity3d.com/threads/audiosource-pitch-question-c.164374/
    public static GameObject PlayClipAtPoint(AudioClip clip, AudioMixerGroup mixer, Vector3 position, float volume, float pitch)
    {
        GameObject obj = new GameObject();
        obj.transform.position = position;
        obj.AddComponent<AudioSource>();
        var audio = obj.GetComponent<AudioSource>();
        audio.pitch = pitch;
        audio.volume = volume;
        audio.spatialBlend = 1.0f;
        audio.outputAudioMixerGroup = mixer;
        audio.PlayOneShot(clip, volume);
        Object.Destroy(obj, clip.length / pitch);
        return obj;
    }
}
