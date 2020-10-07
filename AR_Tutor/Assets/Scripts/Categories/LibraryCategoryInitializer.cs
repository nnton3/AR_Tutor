using UnityEngine;
using UnityEngine.UI;

public class LibraryCategoryInitializer : MonoBehaviour
{
    private GameName game;
    public string categoryKey { get; private set; }
    [SerializeField] private Image img;
    [SerializeField] private Text title;
    [SerializeField] private Button btn;

    public void Initialize(string _categoryKey, CategoryData _data)
    {
        game = (GameName)_data.game;
        categoryKey = _categoryKey;
        img.sprite = _data.img;
        title.text = _data.title;
    }

    public void EnableSelectable()
    {
        btn.onClick.AddListener(() =>
        {
            Signals.SelectCategoryFromLibrary.Invoke(categoryKey);
        });
    }

    public void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void UpdateImg(Sprite _img)
    {
        if (img != null) img.sprite = _img;
    }
}
