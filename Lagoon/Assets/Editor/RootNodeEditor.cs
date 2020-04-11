using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(RootNode))]
public class RootNodeEditor : NodeEditor
{


    public override void OnBodyGUI()
    {
        EditorStyles.textArea.fontStyle = FontStyle.Bold;
        EditorStyles.textField.fontStyle = FontStyle.Bold;

        RootNode node = target as RootNode;
        //if (simpleNode == null) simpleNode = node as DialogNode;


        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

        if (node.barriers.Count == 0)
        {
            node.AddBarrier();
        }


        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));

        SerializedProperty barrier_list = serializedObject.FindProperty("barriers");

        testStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Barriers", testStyle);
        for (int i = 0; i < node.barriers.Count; i++)
        {
            SerializedProperty test = barrier_list.GetArrayElementAtIndex(i);
            NodeEditorGUILayout.PropertyField(test, GUIContent.none);
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            node.AddBarrier();
        }

        if (node.barriers.Count > 1)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Remove"))
            {
                node.RemoveBarrier();
            }
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();


                testStyle.alignment = TextAnchor.MiddleCenter;
                testStyle.fontStyle = FontStyle.Bold;
                testStyle.normal.textColor = new Color(1, 0, 0);
                GUILayout.Label("Output has to connenct to a dialog node.", testStyle);
       

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
