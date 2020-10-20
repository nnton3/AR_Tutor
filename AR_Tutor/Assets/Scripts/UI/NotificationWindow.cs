using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationWindow : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Button closeBtn;
    private Text notification;

    private void Awake()
    {
        if (notificationPanel != null)
        {
            notification = notificationPanel.GetComponentInChildren<Text>();
            Signals.ShowNotification.AddListener(ShowNotification);
            closeBtn.onClick.AddListener(() => notificationPanel.SetActive(false));
        }
    }

    private void ShowNotification(string text)
    {
        notification.text = text;
        notificationPanel.SetActive(true);
    }
}
