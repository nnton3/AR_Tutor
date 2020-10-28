using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordComposingCardInitializer : MonoBehaviour
{
    [SerializeField] private Button selectBtn;

    private void Start()
    {
        selectBtn.onClick.AddListener(() =>
        {
            Signals.AddWordToClause.Invoke(GetComponent<CardBase>().Key);
        });
    }
}
