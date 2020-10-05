using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardStorage : MonoBehaviour
{
    #region Variables
    private SaveSystem saveSystem;
    private LibraryUIControl libraryControl;

    [SerializeField] private CardPack startCardPack;
    public Dictionary<string, CardData> cards = new Dictionary<string, CardData>();
    #endregion

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        libraryControl = FindObjectOfType<LibraryUIControl>();

        FillCardBase();
    }

    private void FillCardBase()
    {
        AddDefaultCards();
        AddCustomCards();
    }

    private void AddDefaultCards()
    {
        for (int i = 0; i < startCardPack.cards.Length; i++)
        {
            var cardKey = $"startPack_card_{i}";
            cards.Add(cardKey, startCardPack.cards[i]);
        }
    }

    private void AddCustomCards()
    {
        var customCardsData = saveSystem.GetCustomCardsData();
        for (int i = 0; i < customCardsData.keys.Count; i++)
        {
            var img = saveSystem.LoadImage(customCardsData.imageAddres[i]);
            Rect imgRect = new Rect(0, 0, img.width, img.height);
            if (img.width < img.height)
                imgRect = new Rect(0, 0, img.width, img.width);
            else imgRect = new Rect(0, 0, img.height, img.height);

            var card = new CardData(
                customCardsData.cardTitles[i],
                customCardsData.cardTitleForms[i],
                Sprite.Create(
                    saveSystem.LoadImage(customCardsData.imageAddres[i]),
                    new Rect(0, 0, img.width, img.height),
                    Vector2.zero),
                saveSystem.LoadAudio(customCardsData.audioAddres[i]),
                true);
            AddCustomCard(customCardsData.keys[i], card);
        }
    }

    private void AddCustomCard(string key, CardData card)
    {
        if (!cards.ContainsKey(key))
            cards.Add(key, card);
    }

    /// <summary>
    /// Добавить карточку в хранилище и сохранить локально
    /// </summary>
    /// <param name="card"></param>
    /// <param name="key"></param>
    /// <param name="imageKey"></param>
    /// <param name="audioKey"></param>
    public void AddNewCardToBase(CardData card, string key, string imageKey, string audioKey)
    {
        if (cards.ContainsKey(key)) return;

        AddCustomCard(key, card);
        saveSystem.SaveCustomCardFromLocal(card, key, imageKey, audioKey);
    }

    /// <summary>
    /// Обновить картинку для карточки в хранилище и сохранить эти изменения локально
    /// </summary>
    /// <param name="_cardKey"></param>
    /// <param name="_cardImg"></param>
    public void UpdateCustomCardImage(string _cardKey, Sprite _cardImg)
    {
        // Local save
        var cardIndex = saveSystem.GetCustomCardsData().keys.IndexOf(_cardKey);
        var addres = saveSystem.GetCustomCardsData().imageAddres[cardIndex];
        saveSystem.SaveImage(_cardImg.texture, addres);

        // Update actual data
        var newData = cards[_cardKey];
        newData.img = _cardImg;
        cards[_cardKey] = newData;
        Debug.Log("Update storage and save");
    }

    public void DeleteCardFromBase(int cardIndex) { }
}
