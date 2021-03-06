﻿using System;
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
    [SerializeField] private Sprite recImg, stopRecImg, defaultImg;
    [SerializeField] private string title;
    private AudioSource source;
    private CategoryData categoryData;

    private PatientDataManager patientDataManager;
    private CategoryManager categoryManager;
    private CategoryStorage storage;
    private SaveSystem saveSystem;
    private MenuTransitionController transitionController;
    #endregion

    private void Awake()
    {
        source = FindObjectOfType<AudioSource>();
        patientDataManager = FindObjectOfType<PatientDataManager>();
        categoryManager = FindObjectOfType<CategoryManager>();
        saveSystem = FindObjectOfType<SaveSystem>();
        storage = FindObjectOfType<CategoryStorage>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        BindUI();
    }

    private void BindUI()
    {
        titleInputField.onValueChanged.AddListener((value) => title = value);
        recAudio1Btn.onClick.AddListener(() => RecordBtnOnClick());
        recAudio2Btn.onClick.AddListener(() => RecordBtnOnClick());
        playAudioBtn.onClick.AddListener(() => PlayAudio(clip));
        setUpImgBtn.onClick.AddListener(() => PickImage(image));
        applyBtn.onClick.AddListener(() =>
        {
            if (DataIsValid())
            {
                CreateCategory(title, imageData, clip);
                transitionController.ReturnToBack(2);
            }
        });
    }

    public void CreateCategory(string _categoryKey, Sprite _categoryImg)
    {
        var data = storage.Categories[_categoryKey];
        CreateCategory(data.title, _categoryImg.texture, data.clip, data.cardKeys, data.cardsVisible);
    }

    private void CreateCategory(string _title, Texture2D _img, AudioClip _clip, List<string> _cardKeys = null, List<bool> _cardVisibles = null)
    {
        var categoryKey = $"{patientDataManager.GetUserLogin()}{_title}{saveSystem.GetCustomCategoryData().keys.Count}";
        var image1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCategoryData().keys.Count}image1";
        var audio1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCategoryData().keys.Count}audio1";

        var size = (_img.height > _img.width) ? _img.width : _img.height;
        var rect = new Rect(0, 0, size, size);

        categoryData = new CategoryData(
            (int)categoryManager.gameName,
            _title,
            Sprite.Create(_img, rect, Vector2.zero),
            _clip,
            true,
            (_cardKeys == null) ? new List<string>() : _cardKeys,
            (_cardVisibles == null) ? new List<bool>() : _cardVisibles,
            true);

        saveSystem.SaveCustomCategoryFromLocal(categoryData, categoryKey, image1Key, audio1Key);

        storage.AddCategoryToBase(categoryData, categoryKey, image1Key, audio1Key);
        categoryManager.AddCategory(categoryKey);
        Reset();
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

    public void PickImage(Image _targetImg)
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, -1, false);
                if (texture == null)
                    return;

                var size = (texture.width < texture.height) ? texture.width : texture.height;
                _targetImg.sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);

                imageData = texture;
            }
        });
    }

    public void Reset()
    {
        titleInputField.text = "";
        clip = null;
        image.sprite = defaultImg;
        imageData = null;
    }

    private bool DataIsValid()
    {
        if (string.IsNullOrEmpty(title)) return false;
        if (imageData == null) return false;
        if (clip == null) return false;

        return true;
    }

}
