using UnityEngine.Events;

public class CardUnityEvent : UnityEvent<GameName, string, string> { }
public class CategoryEvent : UnityEvent<GameName, string> { }
public class StringBoolEvent : UnityEvent<string, bool> { }
public class StringStringEvent : UnityEvent<string, string> { }
public class StringStringBoolEvent : UnityEvent<string, string, bool> { }
public class StringEvent : UnityEvent<string> { }

public static class Signals
{
    public static StringStringEvent AddCardEvent = new StringStringEvent();
    public static StringEvent AddCategoryEvent = new StringEvent();
    public static StringStringEvent DeleteCardFromCategory = new StringStringEvent();
    public static CardUnityEvent SetImgForCardEvent = new CardUnityEvent();
    public static CategoryEvent SetImgForCategoryEvent = new CategoryEvent();
    public static StringStringBoolEvent SwitchCardVisibleEvent = new StringStringBoolEvent();
    public static StringBoolEvent SwitchCategoryVisibleEvent = new StringBoolEvent();
    public static StringEvent SelectCardFromLibrary = new StringEvent();
    public static StringEvent SelectCategoryFromLibrary = new StringEvent();
    public static StringEvent VariantGameCardSelect = new StringEvent();

    public static void Reset()
    {
        AddCardEvent.RemoveAllListeners();
        SetImgForCardEvent.RemoveAllListeners();
        SwitchCardVisibleEvent.RemoveAllListeners();
        DeleteCardFromCategory.RemoveAllListeners();
        VariantGameCardSelect.RemoveAllListeners();
    }
}
