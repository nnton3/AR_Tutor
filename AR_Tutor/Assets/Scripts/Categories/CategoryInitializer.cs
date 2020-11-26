using UnityEngine;
using UnityEngine.UI;

public class CategoryInitializer : MonoBehaviour, ISwitchedDeleteBtnImg
{
    #region Variables
    [SerializeField] protected Image img;
    [SerializeField] protected Text title;
    [SerializeField] protected Button selectBtn, selectImageBtn, switchVisibleBtn;
    [SerializeField] private Sprite deleteSprite, addSprite;
    public AudioClip Clip { get; private set; }
    protected EditableElement editableElement;
    public string CategoryKey { get; private set; }
    public GameName game { get; private set; }
    public bool IsCustom { get; private set; }
    #endregion

    public void Initialize(GameName _game, string _categoryKey, CategoryData _categoryData)
    {
        editableElement = GetComponent<EditableElement>();

        if (title != null) title.text = _categoryData.title;
        if (img != null) img.sprite = _categoryData.img;
        if (_categoryData.clip != null) Clip = _categoryData.clip;

        BindBtn();

        CategoryKey = _categoryKey;
        game = _game;
        IsCustom = _categoryData.IsCustom;
    }

    public Sprite GetSprite()
    {
        return img.sprite;
    }

    public string GetTitle()
    {
        return title.text;
    }

    private void BindBtn()
    {
        selectBtn.onClick.AddListener(() =>
        {
            if (Clip != null)
                Signals.PlayAudioClipEvent.Invoke(Clip);
        });

        selectImageBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            Signals.SetImgForCategoryEvent.Invoke(game, CategoryKey);
        });

        switchVisibleBtn.onClick.AddListener(HideCategory);
    }

    private void HideCategory()
    {
        var element = GetComponent<EditableElement>();

        element.Visible = !element.Visible;
        switchVisibleBtn.GetComponent<Image>().sprite = (element.Visible) ? deleteSprite : addSprite;

        if (IsCustom) Signals.DeleteCategoryFromGame.Invoke(CategoryKey);
        else Signals.SwitchCategoryVisibleEvent.Invoke(CategoryKey, element.Visible);
    }

    public void SwitchImgForDeleteBtn()
    {
        switchVisibleBtn.GetComponent<Image>().sprite = (editableElement.Visible) ? deleteSprite : addSprite;
    }

    public Button GetSelectBtn() { return selectBtn; }

    public void UpdateImg(Sprite _sprite)
    {
        img.sprite = _sprite;
    }
}
