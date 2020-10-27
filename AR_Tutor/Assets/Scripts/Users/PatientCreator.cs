using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientCreator : MonoBehaviour
{
    #region Variables
    private LoginUIControl uiControl;
    private UserSaveSystem saveSystem;
    private LoginManager loginManager;
    private MenuTransitionController transitionController;
    [SerializeField]
    private Button
        createBtn,
        addImgBtn;
    [SerializeField]
    private InputField
        patientNameField,
        patientAgeField,
        patientLoginField;
    [SerializeField] private Image img;
    [SerializeField] private Texture2D imgData;
    [SerializeField] private string patientName, patientAge, patientLogin;
    [SerializeField] private Sprite defaultSprite;
    #endregion


    private void Awake()
    {
        uiControl = FindObjectOfType<LoginUIControl>();
        saveSystem = FindObjectOfType<UserSaveSystem>();
        loginManager = FindObjectOfType<LoginManager>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        BinFields();

        createBtn.onClick.AddListener(() =>
        {
            CreatePatient();
            transitionController.ReturnToBack();
        });
        addImgBtn.onClick.AddListener(() => PickImage(img));
    }

    private void BinFields()
    {
        patientNameField.onValueChanged.AddListener((value) => patientName = value);
        patientAgeField.onValueChanged.AddListener((value) => patientAge = value);
        patientLoginField.onValueChanged.AddListener((value) => patientLogin = value);
    }

    private void CreatePatient()
    {
        var imageKey = $"{patientLogin}image";

        Sprite sprite = null;
        if (imgData != null)
        {
            var size = (imgData.width > imgData.height) ? imgData.height : imgData.width;
            var rect = new Rect(0, 0, size, size);
            sprite = Sprite.Create(imgData, rect, Vector2.zero);
        }

        var patientData = new PatientData(
            patientName,
            patientAge, 
            sprite);

        uiControl.AddPatientCardInSelector(patientData, patientLogin);
        saveSystem.SavePatientsFromLocal(patientLogin, patientData, imageKey);
        loginManager.AddPatientToUser(patientLogin);
        Reset();
    }

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

                imgData = texture;
            }
        });
    }

    private void Reset()
    {
        patientNameField.text = "";
        patientAgeField.text = "";
        patientLoginField.text = "";
        img.sprite = defaultSprite;
    }
}
