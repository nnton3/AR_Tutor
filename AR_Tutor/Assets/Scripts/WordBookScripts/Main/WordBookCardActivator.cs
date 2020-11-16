
using UnityEngine;
using UnityEngine.UI;

public class WordBookCardActivator : MonoBehaviour
{
    [SerializeField] private Button selectBtn;

    public void Awake()
    {
        selectBtn.onClick.AddListener(Active);
    }

    public void Active()
    {
        Signals.PlayAcudioClipEvent.Invoke(GetComponent<CardBase>().Clip);
        Signals.WordBookCardSelect.Invoke(GetComponent<CardBase>().Key);
    }
}
