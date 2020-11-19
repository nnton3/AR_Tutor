using UnityEngine;

public class MainSceneHintControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private AudioClip 
        startHint,
        startButtons,
        startButtonsGame,
        buttonsModeSelect,
        startVariant,
        varianCategoriesSelect,
        variantCardsSelect,
        variantGame,
        variantSelectCard,
        variantCategorySetting,
        variantCardSetting,
        startWordComposing,
        wordComposingCategorySelect,
        wordComposingCardSelect,
        wordComposingCategorySetting,
        wordComposingCardSetting,
        startWordbook,
        wordbookShowContent,
        wordbookSetting,
        selectCreateMethodCategory,
        openCreateCategory,
        openCategoryLibrary,
        selectCreateMethodCard,
        openCreateCard,
        openCardLibrary;
    #endregion

    private void Awake()
    {
        Signals.StartMainSceneEvent.AddListener(() =>
            PlayHint(startHint, "startMainScene"));

        Signals.StartButtonsEvent.AddListener(() =>
            PlayHint(startButtons, "startButtons"));

        Signals.StartButtonsGameEvent.AddListener(() =>
            PlayHint(startButtonsGame, "startButtonsGame"));

        Signals.StartVariantEvent.AddListener(() =>
            PlayHint(startVariant, "startVariant"));

        Signals.VariantCategoriesSelectEvent.AddListener(() =>
            PlayHint(varianCategoriesSelect, "variantCategorySelect"));

        Signals.VariantCardsSelectEvent.AddListener(() =>
            PlayHint(variantCardsSelect, "variantCardSelect"));

        Signals.VariantGameEvent.AddListener(() =>
            PlayHint(variantGame, "variantGame"));

        Signals.VariantGameCardSelect.AddListener((key) =>
            PlayHint(variantSelectCard, "variantGameCardSelect"));

        Signals.StartWordComposingEvent.AddListener(() =>
            PlayHint(startWordComposing, "startWordcomposing"));

        Signals.WordComposingCategoriesSelectEvent.AddListener(() =>
            PlayHint(wordComposingCategorySelect, "wordcomposingCategorySelect"));

        Signals.WordComposingCardSelectEvent.AddListener(() =>
            PlayHint(wordComposingCardSelect, "wordcomposingCardSelect"));

        Signals.StartWordbookEvent.AddListener(() =>
            PlayHint(startWordbook, "startWordbook"));

        Signals.WordbookShowContentEvent.AddListener(() =>
            PlayHint(wordbookShowContent, "wordbookShowContent"));

        Signals.ButtonsModeSelect.AddListener(() =>
            PlayHint(buttonsModeSelect, "buttonsModeSelect"));

        Signals.VarianCategorySetting.AddListener(() =>
            PlayHint(variantCategorySetting, "variantCategorySetting"));

        Signals.VarianCardSetting.AddListener(() =>
            PlayHint(variantCardSetting, "varianCardSetting"));

        Signals.WordcomposingCategorySetting.AddListener(() =>
            PlayHint(wordComposingCategorySetting, "wordcomposingCategorySetting"));

        Signals.WordcomposingCardSetting.AddListener(() =>
            PlayHint(wordComposingCardSetting, "wordcomposingCardSetting"));

        Signals.WordbookSetting.AddListener(() =>
            PlayHint(wordbookSetting, "wordbookSetting"));

        Signals.SelectCreateMethodCategory.AddListener(() =>
            PlayHint(selectCreateMethodCategory, "selectCreateMethodCategory"));

        Signals.OpenCreateCategoryEvent.AddListener(() =>
            PlayHint(openCreateCategory, "openCreateCategoryEvent"));

        Signals.OpenCategoryLibraryEvent.AddListener(() =>
            PlayHint(openCategoryLibrary, "openCategoryLibraryEvent"));

        Signals.SelectCreateMethodCard.AddListener(() =>
            PlayHint(selectCreateMethodCard, "selectCreateMethodCard"));

        Signals.OpenCreateCardEvent.AddListener(() =>
            PlayHint(openCreateCard, "openCreateCardEvent"));

        Signals.OpenCardLibraryEvent.AddListener(() =>
            PlayHint(openCardLibrary, "openCardLibraryEvent"));
    }

    private void PlayHint(AudioClip _clip, string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            Signals.ForcePlayAudioEvent.Invoke(_clip);
            PlayerPrefs.SetInt(key, 1);
        }
    }
}
