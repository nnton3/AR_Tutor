using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CategoryInitializer : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image img;
    [SerializeField] private Text title;
    [SerializeField] private Button selectBtn, selectImageBtn, deleteBtn;
    [SerializeField] private Image image;
    private Sprite lastSprite;
    private PatientDataManager patientManager;
    public string categoryKey { get; private set; }
    public GameName game { get; private set; }
    #endregion

    public void Initialize(GameName _game, string _categoryKey, CategoryData _categoryData)
    {
        patientManager = FindObjectOfType<PatientDataManager>();

        if (title != null) title.text = _categoryData.title;
        if (img != null) img.sprite = _categoryData.img;

        BindBtn();

        categoryKey = _categoryKey;
        game = _game;
    }

    private void BindBtn()
    {
        selectImageBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            Signals.SetImgForCategoryEvent.Invoke(game, categoryKey);
        });

        deleteBtn.onClick.AddListener(HideCategory);
    }

    private void HideCategory()
    {
        var element = GetComponent<EditableElement>();

        if (element.Visible)
        {
            patientManager.SwitchCategoryVisible(categoryKey, false);
            element.Visible = false;
        }
        else
        {
            patientManager.SwitchCategoryVisible(categoryKey, true);
            element.Visible = true;
        }
    }

    public Button GetSelectBtn() { return selectBtn; }

    public void UpdateImg(Sprite _sprite)
    {
        img.sprite = _sprite;
    }
}
