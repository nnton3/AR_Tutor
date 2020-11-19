using UnityEngine;
using UnityEngine.Events;

public class CardUnityEvent : UnityEvent<GameName, string, string> { }
public class CategoryEvent : UnityEvent<GameName, string> { }
public class StringBoolEvent : UnityEvent<string, bool> { }
public class StringStringEvent : UnityEvent<string, string> { }
public class StringStringBoolEvent : UnityEvent<string, string, bool> { }
public class StringEvent : UnityEvent<string> { }
public class BoolEvent : UnityEvent<bool> { }
public class AudioEvent : UnityEvent<AudioClip> { }
public class MultipleAudioEvent : UnityEvent<AudioClip, AudioClip> { }
public class PatientEvent : UnityEvent<PatientData, string> { }
public class AddWordEvent : UnityEvent<string, Sprite, AudioClip> { }

public static class Signals
{
    public static StringEvent ShowNotification = new StringEvent();
    public static UnityEvent ShowLoadScreen = new UnityEvent();
    public static AudioEvent PlayAudioClipEvent = new AudioEvent();
    public static AudioEvent ForcePlayAudioEvent = new AudioEvent();
    public static MultipleAudioEvent PlayTwoAudioEvent = new MultipleAudioEvent();
    public static MultipleAudioEvent ForcePlayTwoAudioEvent = new MultipleAudioEvent();
    public static UnityEvent StopPlayAudioEvent = new UnityEvent();

    #region LoginSceneEvents
    public static UnityEvent ResetPasswordEvent = new UnityEvent();
    public static PatientEvent AddPatientEvent = new PatientEvent();

    public static UnityEvent ApplicationStartEvent = new UnityEvent();
    public static UnityEvent EnterToAuthWindowEvent = new UnityEvent();
    public static UnityEvent SignInEvent = new UnityEvent();
    public static UnityEvent SignUpEvent = new UnityEvent();
    public static UnityEvent OpenCreatePatientPanelEvent = new UnityEvent();
    #endregion

    #region Main scene events
    public static UnityEvent StartMainSceneEvent = new UnityEvent();
    public static UnityEvent ReturnToMainMenuEvent = new UnityEvent();
    public static UnityEvent StartVariantEvent = new UnityEvent();
    public static UnityEvent VariantCategoriesSelectEvent = new UnityEvent();
    public static UnityEvent VariantCardsSelectEvent = new UnityEvent();
    public static UnityEvent VariantGameEvent = new UnityEvent();
    public static UnityEvent VarianCategorySetting = new UnityEvent();
    public static UnityEvent VarianCardSetting = new UnityEvent();
    public static UnityEvent StartButtonsEvent = new UnityEvent();
    public static UnityEvent StartButtonsGameEvent = new UnityEvent();
    public static UnityEvent ButtonsModeSelect = new UnityEvent();
    public static UnityEvent StartWordComposingEvent = new UnityEvent();
    public static UnityEvent WordComposingCategoriesSelectEvent = new UnityEvent();
    public static UnityEvent WordComposingCardSelectEvent = new UnityEvent();
    public static UnityEvent WordcomposingCategorySetting = new UnityEvent();
    public static UnityEvent WordcomposingCardSetting = new UnityEvent();
    public static UnityEvent StartWordbookEvent = new UnityEvent();
    public static UnityEvent WordbookShowContentEvent = new UnityEvent();
    public static UnityEvent WordbookSetting = new UnityEvent();

    public static UnityEvent SelectCreateMethodCategory = new UnityEvent();
    public static UnityEvent OpenCreateCategoryEvent = new UnityEvent();
    public static UnityEvent OpenCategoryLibraryEvent = new UnityEvent();
    public static UnityEvent SelectCreateMethodCard = new UnityEvent();
    public static UnityEvent OpenCreateCardEvent = new UnityEvent();
    public static UnityEvent OpenCardLibraryEvent = new UnityEvent();
    #endregion

    #region Customization
    public static StringStringEvent AddCardEvent = new StringStringEvent();
    public static StringEvent AddCategoryEvent = new StringEvent();
    public static StringEvent DeleteCategoryFromGame = new StringEvent();
    public static StringStringEvent DeleteCardFromCategory = new StringStringEvent();
    public static CardUnityEvent SetImgForCardEvent = new CardUnityEvent();
    public static CategoryEvent SetImgForCategoryEvent = new CategoryEvent();
    public static StringStringBoolEvent SwitchCardVisibleEvent = new StringStringBoolEvent();
    public static StringBoolEvent SwitchCategoryVisibleEvent = new StringBoolEvent();
    public static StringEvent SelectCardFromLibrary = new StringEvent();
    public static StringEvent SelectCategoryFromLibrary = new StringEvent();
    public static BoolEvent CardLoadEnd = new BoolEvent();
    #endregion

    #region Variant
    public static StringEvent VariantGameCardSelect = new StringEvent();
    #endregion

    #region WordComposing
    public static StringEvent AddWordToClause = new StringEvent();
    public static AddWordEvent AddCategoryWord = new AddWordEvent();
    public static UnityEvent RemoveWordFromClause = new UnityEvent();
    public static UnityEvent ResetWordComposingMenu = new UnityEvent();
    public static UnityEvent LastWordSelected = new UnityEvent();
    public static UnityEvent RemoveFirstWord = new UnityEvent();
    public static UnityEvent ReturnAllWordInClause = new UnityEvent();
    public static UnityEvent ReturnSecondRankCard = new UnityEvent();
    public static UnityEvent RemoveSecondRankWord = new UnityEvent();
    #endregion

    #region Wordbook
    public static StringEvent WordBookCardSelect = new StringEvent();
    public static UnityEvent LeftSwipeEvent = new UnityEvent();
    public static UnityEvent RightSwipeEvent = new UnityEvent();
    public static UnityEvent UpSwipeEvent = new UnityEvent();
    public static UnityEvent DownSwipeEvent = new UnityEvent();
    #endregion

    public static void Reset()
    {
        AddCardEvent.RemoveAllListeners();
        SetImgForCardEvent.RemoveAllListeners();
        SwitchCardVisibleEvent.RemoveAllListeners();
        DeleteCardFromCategory.RemoveAllListeners();
        VariantGameCardSelect.RemoveAllListeners();
    }
}
