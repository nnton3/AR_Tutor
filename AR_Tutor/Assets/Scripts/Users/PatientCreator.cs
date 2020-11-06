using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PatientCreator : MonoBehaviour
{
    #region Variables
    private UserSaveSystem saveSystem;
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
        saveSystem = FindObjectOfType<UserSaveSystem>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        BinFields();

        createBtn.onClick.AddListener(() =>
        {
            if (PatientDataIsValid())
            {
                CreatePatient();
                transitionController.ReturnToBack();
            }
        });
        addImgBtn.onClick.AddListener(() => PickImage(img));
    }

    private bool PatientDataIsValid()
    {
        if (string.IsNullOrEmpty(patientName))
        {
            Signals.ShowNotification.Invoke("Ошибка! Некорректный ввод имени");
            return false;
        }
        if (string.IsNullOrEmpty(patientAge))
        {
            Signals.ShowNotification.Invoke("Ошибка! Некорректный ввод возраста");
            return false;
        }
        if (!int.TryParse(patientAge, out int num))
        {
            Signals.ShowNotification.Invoke("Ошибка! Некорректный ввод возраста");
            return false;
        }
        if (patientAge.Length > 4)
        {
            Signals.ShowNotification.Invoke("Ошибка! Некорректный ввод возраста");
            return false;
        }
        if (!Regex.IsMatch(patientLogin, @"[0-9a-zA-Z]{6}"))
        {
            Signals.ShowNotification.Invoke("Ошибка! Некорректный ввод идентификатора. Идентификатор должен содержать только английские буквы и цифры");
            return false;
        }

        return true;
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

        Signals.AddPatientEvent.Invoke(patientData, patientLogin);
        saveSystem.SavePatientsFromLocal(patientLogin, patientData, imageKey);
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
