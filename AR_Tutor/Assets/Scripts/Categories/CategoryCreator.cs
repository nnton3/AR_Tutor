using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryCreator : MonoBehaviour
{
    #region Variables
    [SerializeField] private Button playAudioBtn, recAudio1Btn, recAudio2Btn, setUpImgBtn, applyBtn;
    [SerializeField] private InputField titleInputField;
    [SerializeField] private bool recording;

    [SerializeField] private AudioClip clip, clipTmp;
    [SerializeField] private Image image, recClipIcon;
    [SerializeField] private Texture2D imageData;
    [SerializeField] private Sprite recImg, stopRecImg;
    [SerializeField] private string title;
    private AudioSource source;
    #endregion

    private void Awake()
    {
        source = FindObjectOfType<AudioSource>();

        BindUI();
    }

    private void BindUI()
    {
        titleInputField.onValueChanged.AddListener((value) => title = value);

        recAudio1Btn.onClick.AddListener(() => RecordBtnOnClick());
        recAudio2Btn.onClick.AddListener(() => RecordBtnOnClick());
        playAudioBtn.onClick.AddListener(() => PlayAudio(clip));
        setUpImgBtn.onClick.AddListener(() => PickImage(image, imageData));
        applyBtn.onClick.AddListener(() => CreateCategory());
    }

    private void CreateCategory()
    {
        if (DataIsValid())
        {

        }
    }

    #region Audio
    private void RecordBtnOnClick()
    {
        if (recording)
        {
            StopAllCoroutines();
            WriteAudioClip();
        }
        else StartCoroutine(RecordClipRoutine());
    }

    private IEnumerator RecordClipRoutine()
    {
        Debug.Log("Start record");
        recording = true;
        recClipIcon.sprite = stopRecImg;
        clipTmp = Microphone.Start(null, true, 3, 44100);
        yield return new WaitForSeconds(3f);
        WriteAudioClip();
    }

    private void WriteAudioClip()
    {
        Microphone.End(null);
        recording = false;

        clip = clipTmp;
        recClipIcon.sprite = recImg;
        Debug.Log("End record");
    }

    private void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    #endregion

    public NativeGallery.Permission PickImage(Image _targetImg, Texture2D _texture)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, -1, false);
                if (texture == null)
                    return;

                var size = (texture.width < texture.height) ? texture.width : texture.height;
                _targetImg.sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);

                _texture = texture;
            }
        });
        return permission;
    }

    public void ResetFields()
    {
        titleInputField.text = "";
        clip = null;
        image = null;
    }

    private bool DataIsValid()
    {
        if (titleInputField.text == "") return false;
        if (imageData == null) return false;
        if (clip == null) return false;

        return true;
    }
}
