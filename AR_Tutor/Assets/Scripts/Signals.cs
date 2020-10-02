using UnityEngine.Events;

public class CardUnityEvent : UnityEvent<GameName, int, string> { }
public class UnityStringEvent : UnityEvent<string> { }

public static class Signals
{
    public static CardUnityEvent AddCardEvent = new CardUnityEvent();
    public static CardUnityEvent SetImageEvent = new CardUnityEvent();
    public static CardUnityEvent SwitchCardVisibleEvent = new CardUnityEvent();
    public static CardUnityEvent DeleteCardFromCategory = new CardUnityEvent();
    public static StringUnityEvent SelectCardFromLibrary = new StringUnityEvent();

    public static void Reset()
    {
        AddCardEvent.RemoveAllListeners();
        SetImageEvent.RemoveAllListeners();
        SwitchCardVisibleEvent.RemoveAllListeners();
        DeleteCardFromCategory.RemoveAllListeners();
    }
}
