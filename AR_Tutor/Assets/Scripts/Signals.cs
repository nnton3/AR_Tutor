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

public static class Signals
{
    public static StringEvent ShowNotification = new StringEvent();
    public static AudioEvent PlayAcudioClipEvent = new AudioEvent();
    public static UnityEvent StopPlayAudioEvent = new UnityEvent();
    public static UnityEvent ResetPasswordEvent = new UnityEvent();

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
    public static StringEvent VariantGameCardSelect = new StringEvent();
    public static BoolEvent CardLoadEnd = new BoolEvent();

    public static StringEvent AddWordToClause = new StringEvent();
    public static UnityEvent RemoveWordFromClause = new UnityEvent();
    public static UnityEvent ResetWordComposingMenu = new UnityEvent();
    public static UnityEvent LastWordSelected = new UnityEvent();
    public static UnityEvent RemoveFirstWord = new UnityEvent();
    public static UnityEvent ReturnAllWordInClause = new UnityEvent();
    public static UnityEvent ReturnSecondRankCard = new UnityEvent();
    public static UnityEvent RemoveSecondRankWord = new UnityEvent();

    public static StringEvent WordBookCardSelect = new StringEvent();
    public static UnityEvent LeftSwipeEvent = new UnityEvent();
    public static UnityEvent RightSwipeEvent = new UnityEvent();
    public static UnityEvent UpSwipeEvent = new UnityEvent();
    public static UnityEvent DownSwipeEvent = new UnityEvent();

    public static void Reset()
    {
        AddCardEvent.RemoveAllListeners();
        SetImgForCardEvent.RemoveAllListeners();
        SwitchCardVisibleEvent.RemoveAllListeners();
        DeleteCardFromCategory.RemoveAllListeners();
        VariantGameCardSelect.RemoveAllListeners();
    }
}
