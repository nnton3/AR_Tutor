using System;
using UnityEngine;
using UnityEngine.UI;

public class LibraryCardInitializer : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Image img;
    public string cardKey { get; private set; }
    private CategoryManager cardCreator;
    private LibraryCardSelector cardSelector;
    private Button btn;

    public void Initialize(string _cardKey, CardData data)
    {
        if (title != null) title.text = data.Title;
        if (img != null) img.sprite = data.img;
        cardKey = _cardKey;

        cardCreator = FindObjectOfType<CategoryManager>();
        cardSelector = FindObjectOfType<LibraryCardSelector>();
        btn = GetComponent<Button>();
    }

    public void EnableSelectable()
    {
        btn.onClick.AddListener(() =>
        {
            cardSelector.CardSelectedEvent.Invoke(cardKey);
        });
    }

    public void ClearEvent()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void UpdateImg(Sprite cardImg)
    {
        if (img != null) img.sprite = cardImg;
    }
}
