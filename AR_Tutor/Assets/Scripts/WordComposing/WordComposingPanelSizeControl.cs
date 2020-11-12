using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordComposingPanelSizeControl : MonoBehaviour
{
    [SerializeField] private Sprite backForCustomize, backForPlay;
    [SerializeField] private Image backImg;
    [SerializeField] private RectTransform categorySelectorRect;
    [SerializeField] private GameObject firstRankParent, secondRankCardParent, categoryParent;

    public void ConfigurateForPlay(GameObject firstRankCard, GameObject secondRankCard1, GameObject secondRankCard2, List<GameObject> _categoryPanels)
    {
        backImg.sprite = backForPlay;
        SetAnchorsForPlay();
        SetParenForPlay(firstRankCard, secondRankCard1, secondRankCard2);
    }

    public void ConfigurateForCustomize(GameObject firstRankCard, GameObject secondRankCard1, GameObject secondRankCard2, List<GameObject> _categoryPanels)
    {
        backImg.sprite = backForCustomize;
        SetAnchorsForCustomize();
        SetParentForCustomize(firstRankCard, secondRankCard1, secondRankCard2);
    }

    private void SetAnchorsForCustomize()
    {
        var anchorValue = categorySelectorRect.anchorMin;
        anchorValue.y = 0.02941176f;
        categorySelectorRect.anchorMin = anchorValue;
        anchorValue = categorySelectorRect.anchorMax;
        anchorValue.y = 0.9852941f;
        categorySelectorRect.anchorMax = anchorValue;
    }

    private void SetAnchorsForPlay()
    {
        var anchorValue = categorySelectorRect.anchorMin;
        anchorValue.y = 0.3675882f;
        categorySelectorRect.anchorMin = anchorValue;
        anchorValue = categorySelectorRect.anchorMax;
        anchorValue.y = 0.9852941f;
        categorySelectorRect.anchorMax = anchorValue;
    }

    private void SetParenForPlay(GameObject firstRankCard, GameObject secondRankCard1, GameObject secondRankCard2)
    {
        firstRankCard.transform.SetParent(firstRankParent.transform);
        secondRankCard1.transform.SetParent(secondRankCardParent.transform);
        secondRankCard2.transform.SetParent(secondRankCardParent.transform);
    }

    private void SetParentForCustomize(GameObject firstRankCard, GameObject secondRankCard1, GameObject secondRankCard2)
    {
        secondRankCard2.transform.SetParent(categoryParent.transform);
        secondRankCard2.transform.SetAsFirstSibling();

        secondRankCard1.transform.SetParent(categoryParent.transform);
        secondRankCard1.transform.SetAsFirstSibling();

        firstRankCard.transform.SetParent(categoryParent.transform);
        firstRankCard.transform.SetAsFirstSibling();
    }
}
