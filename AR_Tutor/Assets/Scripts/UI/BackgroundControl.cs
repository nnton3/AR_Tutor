using UnityEngine;
using UnityEngine.UI;

public class BackgroundControl : MonoBehaviour
{
    [SerializeField] private Sprite defaultBack, buttonsBack, variantBack, wordcomposingBack, wordbookBack;
    [SerializeField] private Image background;

    private void Awake()
    {
        Signals.StartVariantEvent.AddListener(() => SetBackground(variantBack));
        Signals.VarianCategorySetting.AddListener(() => SetBackground(variantBack));

        Signals.StartButtonsEvent.AddListener(() => SetBackground(buttonsBack));
        Signals.ButtonsModeSelect.AddListener(() => SetBackground(buttonsBack));

        Signals.StartWordComposingEvent.AddListener(() => SetBackground(wordcomposingBack));
        Signals.WordcomposingCategorySetting.AddListener(() => SetBackground(wordcomposingBack));

        Signals.StartWordbookEvent.AddListener(() => SetBackground(wordbookBack));
        Signals.WordbookSetting.AddListener(() => SetBackground(wordbookBack));

        Signals.ReturnToMainMenuEvent.AddListener(() => SetBackground(defaultBack));
    }

    private void SetBackground(Sprite _sprite)
    {
        background.sprite = _sprite;
    }
}
