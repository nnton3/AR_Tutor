using UnityEngine;
using System.Collections;

public class LoginHintControl : MonoBehaviour
{
    [SerializeField]
    private AudioClip
        helloClip,
        startHint,
        authScreenHint,
        signInClip,
        signInHint,
        signUpHint,
        createPatientClip,
        createPatientHint;

    private void Awake()
    {
        Signals.ApplicationStartEvent.AddListener(() =>
            PlayMultipleAudio(helloClip, startHint));

        Signals.EnterToAuthWindowEvent.AddListener(() =>
            Signals.ForcePlayAudioEvent.Invoke(authScreenHint));

        Signals.SignInEvent.AddListener(() =>
            PlayMultipleAudio(signInClip, signInHint));

        Signals.SignUpEvent.AddListener(() =>
            Signals.ForcePlayAudioEvent.Invoke(signUpHint));

        Signals.OpenCreatePatientPanelEvent.AddListener(() =>
            PlayMultipleAudio(createPatientClip, createPatientHint));
    }

    private void PlayMultipleAudio(AudioClip _clip, AudioClip _clip2)
    {
        if (LoginManager.HasEnter)
            Signals.ForcePlayAudioEvent.Invoke(_clip);
        else
            Signals.ForcePlayTwoAudioEvent.Invoke(_clip, _clip2);
    }
}
