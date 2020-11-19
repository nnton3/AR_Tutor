using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VariantCategoryPanelControl : MonoBehaviour, IEditableElement
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject indicator;

    public void ConfigurateElement(MenuMode _mode)
    {
        if (_mode == MenuMode.CustomizeMenu) indicator.SetActive(false);
        else indicator.SetActive(true);
    }

    public void Initialize(UnityAction _closeAction)
    {
        if (closeBtn != null) closeBtn.onClick.AddListener(_closeAction);
    }
}
