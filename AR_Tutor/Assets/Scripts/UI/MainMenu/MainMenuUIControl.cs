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
    [SerializeField] private GameObject[] returnBtns = new GameObject[] { };
    [SerializeField] private List<IEditableElement> editableElements = new List<IEditableElement>();

    private MenuTransitionController transitionController;
    public static MenuMode Mode { get; private set; } = MenuMode.Play;    
    #endregion

    public void Initialize()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();

        BindBtns();
        SwitchEditableElemets();

        transitionController.ActivatePanel(gameSelector);
        transitionController.ReturnToMainMenuEvent.AddListener(() => settingsBtn.gameObject.SetActive(true));
    }

    private void Start()
    {
        Signals.StartMainSceneEvent.Invoke();
    }

    private void BindBtns()
    {
        for (int i = 0; i < gamesBtns.Length; i++)
        {
            var btn = gamesBtns[i];
            var panel = defaultGamePanels[i];
            var panel_settings = settingsGamePanels[i];
            var returnBtn = returnBtns[i];
            var index = i;

            btn.onClick.AddListener(() =>
            {
                settingsBtn.gameObject.SetActive(false);

                if (Mode == MenuMode.Play)
                    panel.SetActive(true);
                else
                    panel_settings.SetActive(true);

                switch (index)
                {
                    case 0:
                        if (Mode == MenuMode.Play)
                            Signals.StartVariantEvent.Invoke();
                        break;
                    case 1:
                        if (Mode == MenuMode.Play)
                            Signals.StartButtonsEvent.Invoke();
                        break;
                    case 2:
                        if (Mode == MenuMode.Play)
                            Signals.StartWordComposingEvent.Invoke();
                        break;
                    case 3:
                        if (Mode == MenuMode.Play)
                            Signals.StartWordbookEvent.Invoke();
                        break;
                    default:
                        break;
                }

                if (returnBtn != null)
                {
                    returnBtn.SetActive(true);
                    returnBtn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        gameSelector.SetActive(true);
                        settingsBtn.gameObject.SetActive(true);
                    });
                }
                gameSelector.SetActive(false);
            });
        }

        settingsBtn.onClick.AddListener(() => SwitchMode());
    }

    private void SwitchMode()
    {
        if (Mode == MenuMode.CustomizeMenu) Mode = MenuMode.Play;
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

    public void AddEditableElement(IEditableElement element)
    {
        editableElements.Add(element);
    }

    public void DeleteEditableElement(IEditableElement element)
    {
        editableElements.Remove(element);
    }
}

public enum MenuMode { Play, CustomizeMenu }
