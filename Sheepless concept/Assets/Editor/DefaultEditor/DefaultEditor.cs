using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class DefaultEditor<T> : Editor where T : MonoBehaviour
{
    bool showDefaultInspector = false;

    private void OnEnable()
    {
        OnCustomEnable();
    }

    public override void OnInspectorGUI()
    {
        showDefaultInspector = EditorGUILayout.Toggle("Show the default editor", showDefaultInspector);

        if (showDefaultInspector)
            base.OnInspectorGUI();
        else
            OnCustomInspectorGUI();
    }

    public abstract void OnCustomInspectorGUI();

    public abstract void OnCustomEnable();
}
