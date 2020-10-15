using UnityEngine;
using UnityEngine.UI;

public class LoadCardFromCloudUI : MonoBehaviour
{
    [SerializeField] private Button loadBtn;
    [SerializeField] private InputField cardKeyInputField;
    [SerializeField] private string cardKey = "";
    private ContentLoader contentLoader;

    private void Awake()
    {
        contentLoader = FindObjectOfType<ContentLoader>();

        BindUI();

        Signals.CardLoadEnd.AddListener((value) => ResetFields());
    }

    private void BindUI()
    {
        cardKeyInputField.onValueChanged.AddListener((value) => cardKey = value);

        loadBtn.onClick.AddListener(() =>
        {
            if (cardKey == null) return;
            if (cardKey == "") return;
            Debug.Log("Start load card");
            StartCoroutine(contentLoader.LoadCardFromCloud(cardKey));
        });
    }

    private void ResetFields()
    {
        cardKey = "";
        cardKeyInputField.text = "";
    }
}
