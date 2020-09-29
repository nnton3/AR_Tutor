using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;
using System;

public class MainMenuUIControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject gameSelector;
    [SerializeField] private Button returnBtn, settingsBtn;
    [SerializeField] private Button[] gamesBtns = new Button[] { };
    [SerializeField] private GameObject[] gamePanels = new GameObject[] { };
    [SerializeField] private List<EditableElement> editableElements = new List<EditableElement>();

    private MenuTransitionController transitionController;
    private VariantGameMenu variantGameUI;
    public MenuMode mode { get; private set; } = MenuMode.GameSelection;
    public bool SettingsBtnActiveSelf
    {
        get { return settingsBtn.gameObject.activeSelf; }
        set { settingsBtn.gameObject.SetActive(value); }
    }
    #endregion

    public void Initialize()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();
        variantGameUI = GetComponent<VariantGameMenu>();

        editableElements = FindObjectsOfType<EditableElement>().ToList();
        BindBtns();
        variantGameUI.HidePanels();
        SwitchEditableElemets();

        transitionController.ActivatePanel(new GameObject[] { gameSelector });
        transitionController.ReturnToMainMenuEvent.AddListener(() => SettingsBtnActiveSelf = true);
    }

    private void BindBtns()
    {
        BindPanelBtns(gamesBtns, gamePanels, () => SettingsBtnActiveSelf = false);

        returnBtn.onClick.AddListener(() => transitionController.ReturnToBack());
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

    public void BindPanelBtns(Button[] btns, GameObject[] panels, UnityAction action = null)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            var btn = btns[i];
            var panel = panels[i];

            btn.onClick.AddListener(() =>
            {
                transitionController.ActivatePanel(new GameObject[] { panel });
                action?.Invoke();
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
