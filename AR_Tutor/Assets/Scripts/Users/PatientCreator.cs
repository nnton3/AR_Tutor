using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PatientCreator : MonoBehaviour
{
    #region Variables
    private UserSaveSystem saveSystem;
    private MenuTransitionController transitionController;
    private bool recording;
    private AudioClip clip, clipTmp;
    private string patientName, patientLogin, patientGender;
    [SerializeField] private GameObject patientPhoto;
    [SerializeField] private Button createBtn, addImgBtn, selectGirl, selectBoy, recordBtn, playAudioBtn;
    [SerializeField] private InputField patientNameField, patientLoginField;
    [SerializeField] private Image img, recImg;
    [SerializeField] private Texture2D imgData;
    [SerializeField] private Sprite defaultPatientBoySprite, defaultPatientGirlSprite, startRecordSprite, endRecordSprite, addImageDefaultSprite;
    #endregion

    private void Awake()
    {
        saveSystem = FindObjectOfType<UserSaveSystem>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        BinFields();
        BindBtn();
    }

    private void BindBtn()
    {
        createBtn.onClick.AddListener(() =>
        {
            if (PatientDataIsValid())
            {
                CreatePatient();
                transitionController.ReturnToBack();
            }
        });
        addImgBtn.onClick.AddListener(() => PickImage(img));
        selectGirl.onClick.AddListener(() =>
        {
            SetGender("girl");
            selectGirl.GetComponent<Image>().color = Color.grey;
            selectBoy.GetComponent<Image>().color = Color.white;
        });
        selectBoy.onClick.AddListener(() =>
        {
            SetGender("boy");
            selectBoy.GetComponent<Image>().color = Color.grey;
            selectGirl.GetComponent<Image>().color = Color.white;
        });
        recordBtn.onClick.AddListener(RecordBtnOnClick);
        playAudioBtn.onClick.AddListener(() =>
        {
            if (!recording) Signals.ForcePlayAudioEvent.Invoke(clip);
        });
    }

    private bool PatientDataIsValid()
    {
        if (string.IsNullOrEmpty(patientName))
        {
            Signals.ShowNotification.Invoke("Ошибка! Некорректный ввод имени");
            return false;
        }
        if (patientGender != "girl" && patientGender != "boy")
        {
            Signals.ShowNotification.Invoke("Ошибка! Выберите пол");
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
        patientLoginField.onValueChanged.AddListener((value) => patientLogin = value);
    }

    private void CreatePatient()
    {
        var imageKey = $"{patientLogin}image";
        var clipKey = $"{patientLogin}clipName";

        Sprite sprite = null;
        if (imgData != null)
        {
            var size = (imgData.width > imgData.height) ? imgData.height : imgData.width;
            var rect = new Rect(0, 0, size, size);
            sprite = Sprite.Create(imgData, rect, Vector2.zero);
        }
        else
        {
            if (patientGender == "boy") sprite = defaultPatientBoySprite;
            else sprite = defaultPatientGirlSprite;

            imageKey = null;
        }

        var patientData = new PatientData(
            patientName,
            patientGender, 
            sprite,
            clip);

        Signals.AddPatientEvent.Invoke(patientData, patientLogin);
        saveSystem.SavePatientsFromLocal(patientLogin, patientData, imageKey, clipKey);
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
                patientPhoto.SetActive(true);
            }
        });
    }

    public void SetGender(string _gender)
    {
        patientGender = _gender;
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
        recImg.sprite = endRecordSprite;
        clipTmp = Microphone.Start(null, true, 3, 44100);
        yield return new WaitForSeconds(3f);
        WriteAudioClip();
    }

    private void WriteAudioClip()
    {
        Microphone.End(null);
        recording = false;

        clip = clipTmp;
        recImg.sprite = startRecordSprite;
        Debug.Log("End record");
    }
    #endregion

    private void Reset()
    {
        patientNameField.text = "";
        patientLoginField.text = "";
        selectGirl.GetComponent<Image>().color = Color.white;
        selectBoy.GetComponent<Image>().color = Color.white;
        patientPhoto.SetActive(false);
    }
}
