using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject gameSelector;
    [SerializeField] private Button returnBtn, settingsBtn;
    [SerializeField] private Button[] gamesBtns = new Button[] { };
    [SerializeField] private GameObject[] defaultGamePanels = new GameObject[] { };
    [SerializeField] private GameObject[] settingsGamePanels = new GameObject[] { };
    [SerializeField] private List<EditableElement> editableElements = new List<EditableElement>();

    private MenuTransitionController transitionController;
    public MenuMode Mode { get; private set; } = MenuMode.GameSelection;    
    #endregion

    public void Initialize()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();

        BindBtns();
        SwitchEditableElemets();

        transitionController.ActivatePanel(gameSelector);
        transitionController.ReturnToMainMenuEvent.AddListener(() => settingsBtn.gameObject.SetActive(true));
    }

    private void BindBtns()
    {
        for (int i = 0; i < gamesBtns.Length; i++)
        {
            var btn = gamesBtns[i];
            var panel = defaultGamePanels[i];
            var panel_settings = settingsGamePanels[i];

            btn.onClick.AddListener(() =>
            {
                settingsBtn.gameObject.SetActive(false);


                if (Mode == MenuMode.GameSelection)
                    transitionController.ActivatePanel(panel);
                else
                    transitionController.ActivatePanel(panel_settings);
            });
            if (i == 2) btn.onClick.AddListener(() =>
            {
                Signals.WordComposingActivate.Invoke();
                transitionController.AddEventToReturnBtn(() => Signals.WordComposingDisable.Invoke());
            });
        }

        settingsBtn.onClick.AddListener(() => SwitchMode());
    }

    private void SwitchMode()
    {
        if (Mode == MenuMode.CustomizeMenu) Mode = MenuMode.GameSelection;
        else Mode = MenuMode.CustomizeMenu;

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
            element.ConfigurateElement(Mode);
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
