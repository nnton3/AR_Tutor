using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoadCardFromCloudUI : MonoBehaviour
{
    [SerializeField] private Button loadBtn;
    [SerializeField] private InputField cardKeyInputField;

    private ContentLoader contentLoader;

    private void Awake()
    {
        contentLoader = FindObjectOfType<ContentLoader>();

        BindUI();
    }

    private void BindUI()
    {
        loadBtn.onClick.AddListener(() =>
        {
            if (cardKeyInputField.text == null) return;
            if (cardKeyInputField.text == "") return;

            StartCoroutine(contentLoader.LoadCardFromCloud("testCard"));
            cardKeyInputField.text = "";
        });
    }
}
