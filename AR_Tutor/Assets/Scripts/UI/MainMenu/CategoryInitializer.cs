using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CategoryInitializer : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Text title;
    [SerializeField] private Button selectBtn, selectImageBtn, deleteBtn;
    [SerializeField] private Image image;
    private CategoryManager addCard;
    private Sprite lastSprite;
    private PatientDataManager patientManager;
    public int index { get; private set; }
    public GameName game { get; private set; }

    public void Initialize(GameName _game, int categoryIndex, string _title = "new category", Sprite _img = null)
    {
        addCard = FindObjectOfType<CategoryManager>();
        patientManager = FindObjectOfType<PatientDataManager>();

        if (title != null) title.text = _title;
        if (img != null) img.sprite = _img;

        selectImageBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (image == null) return;
            //addCard.PickImage(image, lastSprite);
        });

        deleteBtn.onClick.AddListener(HideCategory);

        index = categoryIndex;
        game = _game;
    }

    private void HideCategory()
    {
        var element = GetComponent<EditableElement>();

        if (element.visible)
        {
            patientManager.HideCategory(game, index);
            element.visible = false;
        }
        else
        {
            patientManager.ShowCategory(game, index);
            element.visible = true;
        }
    }

    public Button GetSelectBtn() { return selectBtn; }
}
