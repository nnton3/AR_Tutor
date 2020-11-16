using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardStorage : MonoBehaviour
{
    #region Variables
    private SaveSystem saveSystem;

    [SerializeField] private CardPack startCardPack;
    public Dictionary<string, CardData> cards = new Dictionary<string, CardData>();
    #endregion

    #region Initialize
    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();

        FillCardBase();
    }

    private void FillCardBase()
    {
        AddDefaultCards();
        AddCustomCards();
    }

    private void AddDefaultCards()
    {
        for (int i = 0; i < startCardPack.cards.Count; i++)
        {
            var cardKey = $"startPack_card_{i}";
            cards.Add(cardKey, startCardPack.cards[i]);
        }
    }

    private void AddCustomCards()
    {
        var customCardsData = saveSystem.GetCustomCardsData();

        if (customCardsData.keys == null) return;

        for (int i = 0; i < customCardsData.keys.Count; i++)
        {
            var img1 = saveSystem.LoadImage(customCardsData.image1Addres[i]);
            var img2 = saveSystem.LoadImage(customCardsData.image2Addres[i]);
            var img3 = saveSystem.LoadImage(customCardsData.image3Addres[i]);
            Rect imgRect = new Rect(0, 0, img1.width, img1.height);
            var size = (img1.width < img1.height) ? img1.width : img1.height;
            imgRect = new Rect(0, 0, size, size);

            var card = new CardData(
                customCardsData.cardTitles[i],
                customCardsData.cardTitleForms[i],
                Sprite.Create(img1, imgRect, Vector2.zero),
                Sprite.Create(img2, imgRect, Vector2.zero),
                Sprite.Create(img3, imgRect, Vector2.zero),
                saveSystem.LoadAudio(customCardsData.audio1Addres[i]),
                saveSystem.LoadAudio(customCardsData.audio2Addres[i]),
                true);
            AddCustomCard(customCardsData.keys[i], card);
        }
    }

    private void AddCustomCard(string key, CardData card)
    {
        if (!cards.ContainsKey(key))
            cards.Add(key, card);
    }
    #endregion

    /// <summary>
    /// Добавить карточку в хранилище и сохранить локально
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <param name="_imageAddress"></param>
    /// <param name="_clipAddress"></param>
    public void AddNewCardToBase(CardData data, string key)
    {
        if (cards.ContainsKey(key)) return;

        AddCustomCard(key, data);
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
        var addres = saveSystem.GetCustomCardsData().image1Addres[cardIndex];
        saveSystem.SaveImage(_cardImg.texture, addres);

        // Update actual data
        var newData = cards[_cardKey];
        newData.img1 = _cardImg;
        cards[_cardKey] = newData;
        Debug.Log("Update storage and save");
    }

    public void DeleteCardFromBase(int cardIndex) { }
}
