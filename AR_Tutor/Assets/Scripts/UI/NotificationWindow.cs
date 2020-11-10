using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationWindow : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Button closeBtn;
    private Text notification;
    [SerializeField] private float time = 0.5f;

    private void Awake()
    {
        if (notificationPanel != null)
        {
            notification = notificationPanel.GetComponentInChildren<Text>();
            Signals.ShowNotification.AddListener((arg) =>
            {
                ShowNotification(arg);
            });
            closeBtn.onClick.AddListener(() => HideNotification());
        }
    }

    private void HideNotification()
    {
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        iTween.ScaleTo(notificationPanel.gameObject, iTween.Hash(
            "x", 0f,
            "y", 0f,
            "z", 0f,
            "time", time,
            "easetype", iTween.EaseType.easeInBack));
        yield return new WaitForSeconds(time);
        notificationPanel.SetActive(false);
    }

    private void ShowNotification(string text)
    {
        notification.text = text;
        ShowWindow();
    }

    private void ShowWindow()
    {
        notificationPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        notificationPanel.SetActive(true);
        iTween.ScaleTo(notificationPanel.gameObject, iTween.Hash(
            "x", 1f,
            "y", 1f,
            "z", 1f,
            "time", time,
            "easetype", iTween.EaseType.easeOutBack));
    }

    private void OnDestroy()
    {
        Signals.ShowNotification.RemoveListener(ShowNotification);
    }
}
