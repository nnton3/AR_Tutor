using System.Collections;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private AudioSource source;
    private bool isPlaying = false;
    private Coroutine routine = null;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        Signals.PlayAcudioClipEvent.AddListener(PlayAudio);
        Signals.StopPlayAudioEvent.AddListener(StopAudio);
    }

    private void PlayAudio(AudioClip _clip)
    {
        if (isPlaying) return;
        routine = StartCoroutine(PlayRoutine(_clip));
    }

    private IEnumerator PlayRoutine(AudioClip _clip)
    {
        isPlaying = true;
        source.PlayOneShot(_clip);
        yield return new WaitForSeconds(_clip.length);
        isPlaying = false;
    }

    private void StopAudio()
    {
        source.Stop();
        StopCoroutine(routine);
    }
}
