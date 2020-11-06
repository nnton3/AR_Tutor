using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        Signals.PlayAcudioClipEvent.AddListener(PlayAudio);
        Signals.StopPlayAudioEvent.AddListener(StopAudio);
    }

    private void PlayAudio(AudioClip _clip)
    {
        source.PlayOneShot(_clip);
    }

    private void StopAudio()
    {
        source.Stop();
    }
}
