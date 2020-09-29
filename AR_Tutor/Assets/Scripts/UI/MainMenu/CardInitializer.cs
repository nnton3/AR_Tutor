using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInitializer : MonoBehaviour
{
    #region Variables
    [SerializeField] protected Text title;
    [SerializeField] protected Button selectBtn, selectImageBtn, deleteBtn;
    [SerializeField] protected Image image;
    private CategoryManager categoryManager;
    private CardCreator cardCreator;
    private SaveSystem saveSystem;
    protected PatientDataManager patientManager;
    private Texture2D texture;
    private AudioClip audioClip;
    private AudioSource source;
    public int categoryIndex { get; private set; }
    public string key { get; private set; }
    public GameName game { get; private set; }
    #endregion

    public virtual void Initialize(GameName _game, int _categoryIndex, string cardKey, CardData data)
    {
        categoryManager = FindObjectOfType<CategoryManager>();
        patientManager = FindObjectOfType<PatientDataManager>();
        saveSystem = FindObjectOfType<SaveSystem>();
        cardCreator = FindObjectOfType<CardCreator>();
        source = FindObjectOfType<AudioSource>();

        ConfigurateUI(data);
        categoryIndex = _categoryIndex;
        key = cardKey;

        BindBtns(data);
    }

    private void BindBtns(CardData data)
    {
        selectImageBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            categoryManager.SetUpNewImage(game, categoryIndex, key);
        });

        if (source != null)
            selectBtn.onClick.AddListener(() => source.PlayOneShot(audioClip));

        deleteBtn.onClick.AddListener(HideCard);
    }

    private void ConfigurateUI(CardData data)
    {
        if (data.Title == null)
        {
            title.text = "Add new card";
            return;
        }

        if (title != null) title.text = data.Title;

        if (image != null)
            if (data.img != null)
                image.sprite = data.img;

        if (data.audioClip != null)
            audioClip = data.audioClip;
    }

    protected virtual void HideCard()
    {
        var element = GetComponent<EditableElement>();

        if (element.visible)
        {
            patientManager.HideCard(game, categoryIndex, key);
            element.visible = false;
        }
        else
            element.visible = true;
    }

    public Button GetSelectBtn() { return selectBtn; }

    public void UpdateImg(Sprite _img)
    {
        image.sprite = _img;
        Debug.Log("Update card image");
    }
}
