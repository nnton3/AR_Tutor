using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordComposingPanelSizeControl : MonoBehaviour
{
    [SerializeField] private Sprite backForCustomize, backForPlay;
    [SerializeField] private Image backImg;
    [SerializeField] private RectTransform categoryContentRect;
    [SerializeField] private GameObject firstRankParent, secondRankCardParent, categoryParent;

    public void ConfigurateForPlay(
        GameObject firstRankCard, GameObject secondRankCard1, GameObject secondRankCard2,
        List<GameObject> _categoryPanels)
    {
        backImg.sprite = backForPlay;
        SetAnchorsForPlay(categoryContentRect);

        foreach (var category in _categoryPanels)
            SetAnchorsForPlay(category.transform.Find("Mask").GetComponent<RectTransform>());

        SetParenForPlay(firstRankCard, secondRankCard1, secondRankCard2);
    }

    public void ConfigurateForCustomize(
        GameObject firstRankCard, GameObject secondRankCard1, GameObject secondRankCard2, 
        List<GameObject> _categoryPanels)
    {
        backImg.sprite = backForCustomize;
        SetAnchorsForCustomize(categoryContentRect);

        foreach (var category in _categoryPanels)
            SetAnchorsForCustomize(category.transform.Find("Mask").GetComponent<RectTransform>());

        SetParentForCustomize(firstRankCard, secondRankCard1, secondRankCard2);
    }

    private void SetAnchorsForCustomize(RectTransform _transform)
    { _transform.anchorMin = new Vector2(0, 0); }

    private void SetAnchorsForPlay(RectTransform _transform)
    { _transform.anchorMin = new Vector2(0, 0.35f); }

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
