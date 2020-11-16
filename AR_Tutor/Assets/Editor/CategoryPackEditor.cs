using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CategoriesSO))]
public class CategoryPackEditor : Editor
{
    private CategoriesSO config;
    public int categoryIndex = 0;
    public int cardIndex = 0;

    private void OnEnable()
    {
        config = (CategoriesSO)target;
    }

    public override void OnInspectorGUI()
    {
        categoryIndex = EditorGUILayout.IntField("Category index", categoryIndex);
        cardIndex = EditorGUILayout.IntField("Card index", cardIndex);

        if (GUILayout.Button("Delete category by index"))
            config.DeleteElement(categoryIndex);

        if (GUILayout.Button("Delete card from category"))
            config.DeleteCardFromCategory(categoryIndex, cardIndex);

        DrawDefaultInspector();
    }
}