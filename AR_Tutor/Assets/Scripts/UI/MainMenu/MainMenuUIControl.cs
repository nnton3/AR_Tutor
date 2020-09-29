using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject gameSelector;
    [SerializeField] private Button returnBtn, settingsBtn;
    [SerializeField] private Button[] gamesBtns = new Button[] { };
    [SerializeField] private GameObject[] gamePanels = new GameObject[] { };
    [SerializeField] private List<EditableElement> editableElements = new List<EditableElement>();

    private MenuTransitionController menuTransition;
    private VariantGameMenu variantGameUI;
    private MenuMode mode = MenuMode.GameSelection;
    #endregion

    public void Initialize()
    {
        menuTransition = FindObjectOfType<MenuTransitionController>();
        variantGameUI = GetComponent<VariantGameMenu>();

        editableElements = FindObjectsOfType<EditableElement>().ToList();
        BindBtns();
        variantGameUI.HidePanels();
        SwitchEditableElemets();

        menuTransition.ActivatePanel(new GameObject[] { gameSelector });
    }

    private void BindBtns()
    {
        BindPanelBtns(gamesBtns, gamePanels);

        returnBtn.onClick.AddListener(() => menuTransition.ReturnToBack());
        settingsBtn.onClick.AddListener(() => SwitchMode());
    }

    private void SwitchMode()
    {
        if (mode == MenuMode.CustomizeMenu) mode = MenuMode.GameSelection;
        else mode = MenuMode.CustomizeMenu;

        SwitchEditableElemets();
    }

    private void SwitchEditableElemets()
    {
        foreach (var element in editableElements)
        {
            if (element == null)
            {
                editableElements.Remove(element);
                continue;
            }
            element.ConfigurateElement(mode);
        }
    }

    public void BindPanelBtns(Button[] btns, GameObject[] panels)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            var btn = btns[i];
            var panel = panels[i];

            btn.onClick.AddListener(() =>
            {
                menuTransition.ActivatePanel(new GameObject[] { panel });
            });
        }
    }

    public void AddEditableElement(EditableElement element)
    {
        editableElements.Add(element);
    }

    public void DeleteEditableElement(EditableElement element)
    {
        editableElements.Remove(element);
    }
}

public enum MenuMode { GameSelection, CustomizeMenu }
