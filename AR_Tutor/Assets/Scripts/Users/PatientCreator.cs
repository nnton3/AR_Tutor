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
    private Texture2D tex;
    #endregion

    private string patientName, patientAge, patientLogin;

    private void Awake()
    {
        uiControl = FindObjectOfType<LoginUIControl>();
        saveSystem = FindObjectOfType<UserSaveSystem>();
        loginManager = FindObjectOfType<LoginManager>();

        BinFields();

        createBtn.onClick.AddListener(CreatePatient);
        addImgBtn.onClick.AddListener(() => PickImage(img, tex));
    }

    private void BinFields()
    {
        patientNameField.onValueChanged.AddListener((value) => patientName = value);
        patientAgeField.onValueChanged.AddListener((value) => patientAge = value);
        patientLoginField.onValueChanged.AddListener((value) => patientLogin = value);
    }

    private void CreatePatient()
    {
        string imageKey = $"{patientLogin}image";

        Sprite sprite = null;
        if (tex != null)
            sprite = img.sprite;

        var patientData = new PatientData(
            patientName,
            patientAge, 
            sprite);

        uiControl.AddPatientCardInSelector(patientData, patientLogin);
        saveSystem.SavePatientsFromLocal(patientLogin, patientData, imageKey);
        loginManager.AddPatientToUser(patientLogin);
    }

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
}
