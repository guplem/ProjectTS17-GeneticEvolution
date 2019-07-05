using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Cell cell = (Cell)target;

        GUILayout.Space(40);

        if (GUILayout.Button("Preview configuration")) {
            cell.ApplyCellProperties();
        }
    }
}
