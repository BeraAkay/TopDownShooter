#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(GridPlanner))]
public class GridPlannerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridPlanner gp = (GridPlanner)target;
        if (GUILayout.Button("Generate Grid"))
        {
            gp.FillGrid();
        }
        if (GUILayout.Button("Clear Grid"))
        {
            gp.ClearGrid();
        }

    }
}

#endif