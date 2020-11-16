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

        Signals.PlayAudioClipEvent.AddListener(PlayAudio);
        Signals.ForcePlayAudioEvent.AddListener(ForcePlayAudio);
        Signals.PlayTwoAudioEvent.AddListener(PlayTwoAudio);
        Signals.ForcePlayTwoAudioEvent.AddListener(ForcePlayTwoAudio);
        Signals.StopPlayAudioEvent.AddListener(StopAudio);
    }

    private void PlayAudio(AudioClip _clip)
    {
        if (isPlaying) return;
        routine = StartCoroutine(PlayRoutine(_clip));
    }

    private void PlayTwoAudio(AudioClip _clip, AudioClip _clip2)
    {
        if (isPlaying) return;
        routine = StartCoroutine(PlayRoutine(_clip, _clip2));
    }

    private void ForcePlayAudio(AudioClip _clip)
    {
        StopAudio();
        PlayAudio(_clip);
    }

    private void ForcePlayTwoAudio(AudioClip _clip, AudioClip _clip2)
    {
        StopAudio();
        PlayTwoAudio(_clip, _clip2);
    }

    private IEnumerator PlayRoutine(AudioClip _clip, AudioClip _clip2 = null)
    {
        isPlaying = true;
        source.PlayOneShot(_clip);
        yield return new WaitForSeconds(_clip.length);

        if (_clip2 != null)
        {
            source.PlayOneShot(_clip2);
            yield return new WaitForSeconds(_clip2.length);
        }

        isPlaying = false;
    }

    private void StopAudio()
    {
        source.Stop();
        if (routine != null) StopCoroutine(routine);
        isPlaying = false;
    }
}
