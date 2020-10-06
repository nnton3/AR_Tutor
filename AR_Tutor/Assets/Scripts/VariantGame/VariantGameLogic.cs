using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantGameLogic : MonoBehaviour
{
    [SerializeField] private GameObject cardPref, parentObj;

    private CardStorage storage;
    [SerializeField] private List<GameObject> currentCards = new List<GameObject>();

    private void Awake()
    {
        storage = FindObjectOfType<CardStorage>();

        Signals.VariantGameCardSelect.AddListener(HideUnselectedCards);
    }

    public void ClearOldData()
    {
        foreach (var card in currentCards)
            Destroy(card);
        currentCards.Clear();
    }

    public void FillPanel(string[] _cardKeys)
    {
        for (int i = 0; i < _cardKeys.Length; i++)
        {
            var data = storage.cards[_cardKeys[i]];
            CreateCard(_cardKeys[i], data, i, _cardKeys.Length);
        }
    }

    private void CreateCard(string _key, CardData data, int index, int count)
    {
        var instance = Instantiate(cardPref, parentObj.transform);
        currentCards.Add(instance);
        ConfigurateElement(index, count);
        var initializer = instance.GetComponent<VariantCardInitializer>();
        initializer.Initialize(0, "", _key, data);
    }

    private void ConfigurateElement(int index, int count)
    {
        int size = 0;
        Vector2 pos = Vector2.zero;

        switch (count)
        {
            case 2:
                size = 180;
                pos = new Vector2(-100 + 200 * index, 0);
                break;
            case 4:
                size = 150;
                pos = new Vector2(-90 + 180 * ((index < 2) ? index : index - 2), (index > 1) ? -90 : 90    );
                break;
            case 6:
                size = 150;
                pos = new Vector2(-170 + 170 * ((index > 2) ? index - 3 : index), (index > 2) ? -90 : 90);
                break;
            default:
                break;
        }
        
        var targetRect = currentCards[index].GetComponent<RectTransform>();
        SetSize(targetRect, new Vector2(size, size));
        SetPositionOfPivot(targetRect, pos);
    }

    public void SetSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    public void SetPositionOfPivot(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    private void HideUnselectedCards(string key)
    {
        foreach (var card in currentCards)
        {
            Debug.Log(card.GetComponent<VariantCardInitializer>().Key != key);
            if (card.GetComponent<VariantCardInitializer>().Key != key) 
                card.SetActive(false);
        }
    }
}
