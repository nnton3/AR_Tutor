using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VariantCategoryPanelControl : MonoBehaviour
{
    [SerializeField] private Button closeBtn;

    public void Initialize(UnityAction _closeAction)
    {
        if (closeBtn != null) closeBtn.onClick.AddListener(_closeAction);
    }
}
