using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject gameSelector;
    [SerializeField] private Button exitBtn, settingsBtn, siteBtn, returnToPlayBtn, returnToMenuBtn;
    [SerializeField] private Button[] gamesBtns = new Button[] { };
    [SerializeField] private GameObject[] defaultGamePanels = new GameObject[] { };
    [SerializeField] private GameObject[] settingsGamePanels = new GameObject[] { };
    [SerializeField] private List<IEditableElement> editableElements = new List<IEditableElement>();

    public static MenuMode Mode { get; private set; } = MenuMode.Play;    
    #endregion

    public void Initialize()
    {
        BindBtns();
        SwitchEditableElemets();

        Signals.ReturnToMainMenuEvent.AddListener(() =>
        {
            returnToMenuBtn.gameObject.SetActive(false);
            gameSelector.SetActive(true);
        });
    }

    private void Start()
    {
        Signals.StartMainSceneEvent.Invoke();
        gameSelector.SetActive(true);
    }

    private void BindBtns()
    {
        returnToMenuBtn.onClick.AddListener(() =>
        {
            Signals.ReturnToMainMenuEvent.Invoke();
        });

        for (int i = 0; i < gamesBtns.Length; i++)
        {
            var index = i;
            var btn = gamesBtns[index];
            var panel = defaultGamePanels[index];
            var panel_settings = settingsGamePanels[index];

            btn.onClick.AddListener(() =>
            {
                if (Mode == MenuMode.Play) panel.SetActive(true);
                else panel_settings.SetActive(true);

                switch (index)
                {
                    case 0:
                        if (Mode == MenuMode.Play) Signals.StartVariantEvent.Invoke();
                        else Signals.VarianCategorySetting.Invoke();
                        break;
                    case 1:
                        if (Mode == MenuMode.Play) Signals.StartButtonsEvent.Invoke();
                        else Signals.ButtonsModeSelect.Invoke();
                        break;
                    case 2:
                        if (Mode == MenuMode.Play) Signals.StartWordComposingEvent.Invoke();
                        else Signals.WordcomposingCategorySetting.Invoke();
                        break;
                    case 3:
                        if (Mode == MenuMode.Play) Signals.StartWordbookEvent.Invoke();
                        else Signals.WordbookSetting.Invoke();
                        break;
                    default:
                        break;
                }

                returnToMenuBtn.gameObject.SetActive(true);
                gameSelector.SetActive(false);
            });
        }

        settingsBtn.onClick.AddListener(SwitchToCustomizeMode);
        returnToPlayBtn.onClick.AddListener(SwitchToPlayMode);
        siteBtn.onClick.AddListener(OpenSite);
        exitBtn.onClick.AddListener(ExitToLoginScene);
    }

    private void SwitchToPlayMode()
    {
        Mode = MenuMode.Play;
        siteBtn.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);
        settingsBtn.gameObject.SetActive(true);
        returnToPlayBtn.gameObject.SetActive(false);

        SwitchEditableElemets();
    }

    private void SwitchToCustomizeMode()
    {
        Mode = MenuMode.CustomizeMenu;
        siteBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);
        settingsBtn.gameObject.SetActive(false);
        returnToPlayBtn.gameObject.SetActive(true);

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

    private void OpenSite()
    {
        Application.OpenURL("https://artutor.ru/");
    }

    private void ExitToLoginScene()
    {
        StartCoroutine(LoadLoginSceneRoutine());
    }

    private IEnumerator LoadLoginSceneRoutine()
    {
        Signals.ShowLoadScreen.Invoke();
        exitBtn.GetComponent<Animator>().SetTrigger("press");
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

public enum MenuMode { Play, CustomizeMenu }
