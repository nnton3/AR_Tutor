using UnityEngine;

public class MainSceneHintControl : MonoBehaviour
{
    [SerializeField] private AudioClip 
        startHint,
        startButtons,
        startButtonsGame,
        startVariant,
        varianCategoriesSelect,
        variantCardsSelect,
        variantGame,
        variantSelectCard,
        startWordComposing,
        wordComposingCategorySelect,
        wordComposingCardSelect,
        startWordbook,
        wordbookShowContent;

    private void Awake()
    {
        Signals.StartMainSceneEvent.AddListener(() =>
            PlayHint(startHint));

        Signals.StartButtonsEvent.AddListener(() =>
            PlayHint(startButtons));

        Signals.StartButtonsGameEvent.AddListener(() =>
            PlayHint(startButtonsGame));

        Signals.StartVariantEvent.AddListener(() =>
            PlayHint(startVariant));

        Signals.VariantCategoriesSelectEvent.AddListener(() =>
            PlayHint(varianCategoriesSelect));

        Signals.VariantCardsSelectEvent.AddListener(() =>
            PlayHint(variantCardsSelect));

        Signals.VariabntGameEvent.AddListener(() =>
            PlayHint(variantGame));

        Signals.VariantGameCardSelect.AddListener((key) =>
            PlayHint(variantSelectCard));

        Signals.StartWordComposingEvent.AddListener(() =>
            PlayHint(startWordComposing));

        Signals.WordComposingCategoriesSelectEvent.AddListener(() =>
            PlayHint(wordComposingCategorySelect));

        Signals.WordComposingCardSelectEvent.AddListener(() =>
            PlayHint(wordComposingCardSelect));

        Signals.StartWordbookEvent.AddListener(() =>
            PlayHint(startWordbook));

        Signals.WordbookShowContentEvent.AddListener(() =>
            PlayHint(wordbookShowContent));
    }

    private void PlayHint(AudioClip _clip)
    {
        if (!LoginManager.HasEnter)
            Signals.ForcePlayAudioEvent.Invoke(_clip);
    }
}
