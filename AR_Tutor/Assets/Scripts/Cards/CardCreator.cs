using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [SerializeField] private AudioSource source;

    [SerializeField] private InputField cardNameField, cardNameFormField;
    [SerializeField] private Button addImage1Btn, addImage2Btn, addImage3Btn, toRight, toLeft, recBtn, rec2Btn, playBtn, play2Btn, applyBtn;
    [SerializeField] private Image image1, image2, image3;
    [SerializeField] private Transform imageContent;
    [SerializeField] private CardData data;

    [SerializeField] private string cardName, cardNameForm;
    [SerializeField] private Texture2D image1data, image2data, image3data;
    [SerializeField] private AudioClip audioClip, audioClipForm;
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
        recBtn.onClick.AddListener(() => RecordAudio());
        rec2Btn.onClick.AddListener(() => RecordAudioForm());

        toRight.onClick.AddListener(() => SwitchImage(1));
        toLeft.onClick.AddListener(() => SwitchImage(-1));

        applyBtn.onClick.AddListener(() =>
        {
            if (CardDataIsValid())
            {
                CreateCard(cardName, cardNameForm, image1.sprite);
                transitionController.ReturnToBack(2);
            }
        });
    }

    public void CreateCard(string baseCardKey, Sprite newImg)
    {
        var oldData = storage.cards[baseCardKey];
        CreateCard(oldData.Title, oldData.TitleForm, newImg);
    }

    private void CreateCard(string _title, string _titleForm, Sprite _image1data)
    {
        var image1Key = $"{patientDataManager.GetPatientLogin()}_{saveSystem.GetCustomCardsData().keys.Count}_image1";
        var cardKey = $"{patientDataManager.GetPatientLogin()}_{_title}_{saveSystem.GetCustomCardsData().keys.Count}";

        data = new CardData(
            _title,
            _titleForm,
            _image1data,
            true);

        saveSystem.SaveImage(_image1data.texture, image1Key);

        storage.AddNewCardToBase(data, cardKey, image1Key);
        categoryManager.AddCard(cardKey);
    }

    #region Audio
    private void RecordAudio()
    {
        StartCoroutine(RecordRoutine(audioClip));
    }

    private void RecordAudioForm()
    {
        StartCoroutine(RecordRoutine(audioClipForm));
    }

    private IEnumerator RecordRoutine(AudioClip clip)
    {
        clip = Microphone.Start(Microphone.devices[0], true, 5, 44100);
        yield return new WaitForSeconds(5f);
        Microphone.End(Microphone.devices[0]);
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

                if (texture.width < texture.height)
                    _targetImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.width), Vector2.zero);
                else _targetImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.height, texture.height), Vector2.zero);

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
        //if (image1data == null) return false;
        //if (image2data == null) return false;
        //if (image3data == null) return false;
        //if (audioClip == null) return false;
        //if (audioClipForm == null) return false;

        return true;
    }
}
