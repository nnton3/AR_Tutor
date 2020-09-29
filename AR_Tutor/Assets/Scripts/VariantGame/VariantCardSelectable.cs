using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantCardSelectable : MonoBehaviour
{
    #region Variables
    private VariantCardSelector selector;
    private MainMenuUIControl mainUIControl;

    [SerializeField] private Image img;
    [SerializeField] private Button btn;
    [SerializeField] private Color defaultClr = Color.white;
    [SerializeField] private Color selectedClr = Color.grey;
    public bool Selected { get; private set; }
    #endregion

    private void Awake()
    {
        selector = FindObjectOfType<VariantCardSelector>();
        mainUIControl = FindObjectOfType<MainMenuUIControl>();

        btn.onClick.AddListener(OnClickEventHandler);
    }

    private void OnClickEventHandler()
    {
        if (mainUIControl.mode == MenuMode.CustomizeMenu) return;

        if (Selected) Unselect();
        else Select();
    }

    private void Select()
    {
        if (!selector.CanSelect()) return;
        Selected = true;
        img.color = selectedClr;
        selector.SelectEvent.Invoke(GetComponent<CardInitializer>().key);
    }

    public void Unselect()
    {
        Selected = false;
        img.color = defaultClr;
        selector.UnselectEvent.Invoke(GetComponent<CardInitializer>().key);
    }

    public Button GetBtn() { return btn; }
}
