using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SpawnCube))]
public class CustomSpawnCube : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("υσι");
        DrawDefaultInspector();

        SpawnCube s = (SpawnCube)target;

        if (GUILayout.Button("Inst", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            s.Create();
        }
    }
}
