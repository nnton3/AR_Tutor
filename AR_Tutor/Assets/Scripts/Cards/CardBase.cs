using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
    #region Variables
    [SerializeField] protected Text title;
    [SerializeField] protected Button selectBtn, selectImageBtn, switchVisibleBtn;
    [SerializeField] protected Image image;
    [SerializeField] private Sprite deleteSprite, addSprite;
    private EditableElement editableElement;
    private Texture2D texture;
    private AudioClip audioClip;
    private AudioSource source;
    protected GameName game;
    protected string categoryKey;
    public string Key { get; protected set; }
    #endregion

    public virtual void Initialize(GameName _game, string _categoryKey, string cardKey, CardData data)
    {
        source = FindObjectOfType<AudioSource>();
        editableElement = GetComponent<EditableElement>();

        ConfigurateUI(data);
        categoryKey = _categoryKey;
        Key = cardKey;
        
        BindBtns(data);
    }

    protected void BindBtns(CardData data)
    {
        selectImageBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            Signals.SetImgForCardEvent.Invoke(game, categoryKey, Key);
        });

        if (source != null)
            selectBtn.onClick.AddListener(() => source.PlayOneShot(audioClip));

        switchVisibleBtn.onClick.AddListener(SwitchVisible);
    }

    private void ConfigurateUI(CardData data)
    {
        if (string.IsNullOrWhiteSpace(data.Title))
        {
            if (title == null) return;
            title.text = "Add new card";
            return;
        }
        else title.text = data.Title;

        if (image != null)
            if (data.img1 != null)
                image.sprite = data.img1;

        if (data.audioClip1 != null)
            audioClip = data.audioClip1;
    }

    protected virtual void SwitchVisible()
    {
        editableElement.Visible = !editableElement.Visible;
        switchVisibleBtn.GetComponent<Image>().sprite = (editableElement.Visible) ? deleteSprite : addSprite;
        Signals.SwitchCardVisibleEvent.Invoke(categoryKey, Key, editableElement.Visible);
    }

    public Button GetSelectBtn() { return selectBtn; }

    public void UpdateImg(Sprite _img)
    {
        image.sprite = _img;
        Debug.Log("Update card image");
    }
}
