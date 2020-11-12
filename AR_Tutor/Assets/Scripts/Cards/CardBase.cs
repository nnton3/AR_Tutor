using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour, ISwitchedDeleteBtnImg
{
    #region Variables
    [SerializeField] protected Text title;
    [SerializeField] protected Button selectBtn, selectImageBtn, switchVisibleBtn;
    [SerializeField] protected Image image;
    [SerializeField] protected Sprite deleteSprite, addSprite;
    protected EditableElement editableElement;
    private Texture2D texture;
    public AudioClip Clip { get; private set; }
    public AudioClip Clip2 { get; private set; }
    protected GameName game;
    protected string categoryKey;
    public string Key { get; protected set; }
    #endregion

    public virtual void Initialize(GameName _game, string _categoryKey, string cardKey, CardData data)
    {
        editableElement = GetComponent<EditableElement>();

        ConfigurateUI(data);
        categoryKey = _categoryKey;
        Key = cardKey;
        if (data.audioClip1 != null) Clip = data.audioClip1;
        
        BindBtns(data);
    }

    protected void BindBtns(CardData data)
    {
        if (selectImageBtn != null)
            selectImageBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                Signals.SetImgForCardEvent.Invoke(game, categoryKey, Key);
            });

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
            Clip = data.audioClip1;

        if (data.audioClip2 != null)
            Clip2 = data.audioClip2;
    }

    protected virtual void SwitchVisible()
    {
        editableElement.Visible = !editableElement.Visible;
        SwitchImgForDeleteBtn();
        Signals.SwitchCardVisibleEvent.Invoke(categoryKey, Key, editableElement.Visible);
    }

    public void SwitchImgForDeleteBtn()
    {
        switchVisibleBtn.GetComponent<Image>().sprite = (editableElement.Visible) ? deleteSprite : addSprite;
    }

    public Button GetSelectBtn() { return selectBtn; }

    public void UpdateImg(Sprite _img)
    {
        image.sprite = _img;
        Debug.Log("Update card image");
    }
}
