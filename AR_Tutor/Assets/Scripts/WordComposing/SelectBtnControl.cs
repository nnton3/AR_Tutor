using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectBtnControl : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float deltaClick = 0.5f;
    private UnityAction doubleClickAction;
    private CategoryInitializer category;

    private void Awake()
    {
        category = GetComponentInParent<CategoryInitializer>();
    }

    public void SetBtnEvents(UnityAction _doubleClick)
    {
        doubleClickAction = _doubleClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (WordComposingMenuControl.ClauseComplete) return;
        if (category.CategoryKey != "default_3_category34" || category.CategoryKey != "default_3_category37")
            StartCoroutine(SingleClickRoutine());

        if (eventData.clickCount == 2)
        {
            StopAllCoroutines();
            doubleClickAction?.Invoke();
        }
    }

    private IEnumerator SingleClickRoutine()
    {
        yield return new WaitForSeconds(deltaClick);
        Signals.LastWordSelected.Invoke();
        Signals.AddCategoryWord.Invoke(category.GetTitle(), category.GetSprite(), category.Clip);
    }
}
