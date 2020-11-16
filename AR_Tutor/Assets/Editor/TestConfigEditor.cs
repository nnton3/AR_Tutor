using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestConfig))]
public class TestConfigEditor : Editor
{
    TestConfig config;

    private void OnEnable()
    {
        config = (TestConfig)target;

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        config.index = EditorGUILayout.IntField("Index", config.index);

        if (GUILayout.Button("Test btn"))
            config.DeleteElement(config.index);
    }
}
