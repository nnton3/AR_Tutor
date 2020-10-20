﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardCreator : MonoBehaviour
{
    #region Variables
    private CardStorage storage;
    private SaveSystem saveSystem;
    private PatientDataManager patientDataManager;
    private CategoryManager categoryManager;
    private MenuTransitionController transitionController;
    [SerializeField] private bool recording;

    [SerializeField] private AudioSource source;

    [SerializeField] private InputField cardNameField, cardNameFormField;
    [SerializeField] private Button addImage1Btn, addImage2Btn, addImage3Btn, toRight, toLeft, recBtn, rec2Btn, playBtn, play2Btn, applyBtn;
    [SerializeField] private Image recClipIcon, recClipFormIcon;
    [SerializeField] Sprite recImg, stopRecImg;
    [SerializeField] private Image image1, image2, image3;
    [SerializeField] private Transform imageContent;
    [SerializeField] private CardData data;

    [SerializeField] private string cardName, cardNameForm;
    [SerializeField] private Texture2D image1data, image2data, image3data;
    [SerializeField] private AudioClip audioClip, audioClipForm, clipTmp;
    #endregion

    private void Awake()
    {
        storage = FindObjectOfType<CardStorage>();
        saveSystem = FindObjectOfType<SaveSystem>();
        patientDataManager = FindObjectOfType<PatientDataManager>();
        categoryManager = FindObjectOfType<CategoryManager>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        BindUI();
    }

    private void BindUI()
    {
        cardNameField.onValueChanged.AddListener((value) => cardName = value);
        cardNameFormField.onValueChanged.AddListener((value) => cardNameForm = value);

        addImage1Btn.onClick.AddListener(() => PickImage(image1, image1data));
        addImage2Btn.onClick.AddListener(() => PickImage(image2, image2data));
        addImage3Btn.onClick.AddListener(() => PickImage(image3, image3data));

        playBtn.onClick.AddListener(() => PlayAudio(audioClip));
        play2Btn.onClick.AddListener(() => PlayAudio(audioClipForm));
        recBtn.onClick.AddListener(() => RecordBtnOnClick(true));
        rec2Btn.onClick.AddListener(() => RecordBtnOnClick(false));

        toRight.onClick.AddListener(() => SwitchImage(1));
        toLeft.onClick.AddListener(() => SwitchImage(-1));

        applyBtn.onClick.AddListener(() =>
        {
            if (CardDataIsValid())
            {
                CreateCard(cardName, cardNameForm, image1.sprite, audioClip);
                transitionController.ReturnToBack(2);
            }
        });
    }

    public void CreateCard(string baseCardKey, Sprite newImg)
    {
        var oldData = storage.cards[baseCardKey];
        CreateCard(oldData.Title, oldData.TitleForm, newImg, oldData.audioClip);
    }

    private void CreateCard(string _title, string _titleForm, Sprite _image1data, AudioClip _audioClip)
    {
        var cardKey = $"{patientDataManager.GetUserLogin()}_{_title}_{saveSystem.GetCustomCardsData().keys.Count}";
        var image1Key = $"{patientDataManager.GetUserLogin()}_{saveSystem.GetCustomCardsData().keys.Count}_image1";
        var audio1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}audio1";

        data = new CardData(
            _title,
            _titleForm,
            _image1data,
            _audioClip,
            true);

        saveSystem.SaveCustomCardFromLocal(data,cardKey,image1Key,audio1Key);

        storage.AddNewCardToBase(data, cardKey);
        categoryManager.AddCard(cardKey);
        ResetFields();
    }

    public void ResetFields()
    {
        cardNameField.text = null;
        cardNameFormField.text = null;
        cardName = "";
        cardNameForm = "";

        image1data = null;
        image2data = null;
        image3data = null;
        image1.sprite = null;
        image2.sprite = null;
        image3.sprite = null;

        audioClip = null;
        audioClipForm = null;
        clipTmp = null;
    }

    #region Audio
    private void RecordBtnOnClick(bool _mainClip)
    {
        if (recording)
        {
            StopAllCoroutines();
            WriteAudioClip(_mainClip);
        }
        else StartCoroutine(RecordClipRoutine(_mainClip));
    }

    private IEnumerator RecordClipRoutine(bool _mainClip)
    {
        Debug.Log("Start record");
        recording = true;
        if (_mainClip) recClipIcon.sprite = stopRecImg;
        else recClipFormIcon.sprite = stopRecImg;
        clipTmp = Microphone.Start(null, true, 3, 44100);
        yield return new WaitForSeconds(3f);
        WriteAudioClip(_mainClip);
    }

    private void WriteAudioClip(bool _mainClip)
    {
        Microphone.End(null);
        recording = false;
        if (_mainClip)
        {
            audioClip = clipTmp;
            recClipIcon.sprite = recImg;
        }
        else
        {
            audioClipForm = clipTmp;
            recClipFormIcon.sprite = recImg;
        }
        Debug.Log("End record");
    }

    private void PlayAudio(AudioClip clip)
    {
        Debug.Log($"Clip is null? {clip == null}");
        Debug.Log("Play audio");
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

    private void SwitchImage(int direction)
    {
        var pos = imageContent.localPosition;

        if (direction > 0)
            if (imageContent.localPosition.x > -375)
                pos.x -= 150;

        if (direction < 0)
            if (imageContent.localPosition.x < -75)
                pos.x += 150;

        imageContent.localPosition = pos;
    }

    private bool CardDataIsValid()
    {
        if (cardName == "") return false;
        if (cardNameForm == null) return false;
        if (image1data == null) return false;
        //if (image2data == null) return false;
        //if (image3data == null) return false;
        if (audioClip == null) return false;
        //if (audioClipForm == null) return false;

        return true;
    }
}
