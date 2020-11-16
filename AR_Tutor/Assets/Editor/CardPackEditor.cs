using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardPack))]
public class CardPackEditor : Editor
{
    private CardPack config;
    public string cardTitle = "";
    public int cardIndex = 0;

    private void OnEnable()
    {
        config = (CardPack)target;
    }

    public override void OnInspectorGUI()
    {
        cardTitle = EditorGUILayout.TextField("Card title", cardTitle);
        cardIndex = EditorGUILayout.IntField("Card index", cardIndex);

        if (GUILayout.Button("Get card index by name"))
            config.GetIndex(cardTitle);

        if (GUILayout.Button("Add card by index"))
            config.AddElement(cardIndex);

        if (GUILayout.Button("Delete card by index"))
            config.DeleteElement(cardIndex);

        DrawDefaultInspector();
    }
}