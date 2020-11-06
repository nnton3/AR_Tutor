using UnityEngine;
using System.Collections;

public class AudioHintControl : MonoBehaviour
{
    [SerializeField]
    private AudioClip
        helloClip,
        startHint,
        authScreenHint,
        signInClip,
        signInHint,
        signUpHint;

    private LoginManager loginMenuControl;
    private Coroutine coroutine;

    private void Awake()
    {
        loginMenuControl = FindObjectOfType<LoginManager>();
    }

    private void Start()
    {
        coroutine = StartCoroutine(StartAudio());
    }

    public IEnumerator StartAudio()
    {
        Signals.PlayAcudioClipEvent.Invoke(helloClip);
        if (!loginMenuControl.HasEnter())
        {
            yield return new WaitForSeconds(helloClip.length);
            Signals.PlayAcudioClipEvent.Invoke(startHint);
        }
    }

    public void StopAudio()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        Signals.StopPlayAudioEvent.Invoke();
    }

    public void PlayAuthHint()
    {
        StopAudio();
        Signals.PlayAcudioClipEvent.Invoke(authScreenHint);
    }

    public void PlaySignInHint()
    {
        coroutine = StartCoroutine(PlaySignInHintRoutine());
    }

    public IEnumerator PlaySignInHintRoutine()
    {
        StopAudio();
        Signals.PlayAcudioClipEvent.Invoke(signInClip);
        if (!loginMenuControl.HasEnter())
        {
            yield return new WaitForSeconds(signInClip.length);
            Signals.PlayAcudioClipEvent.Invoke(signInHint);
        }
    }

    public void PlaySingUpHint()
    {
        StopAudio();
        Signals.PlayAcudioClipEvent.Invoke(signUpHint);
    }
}
