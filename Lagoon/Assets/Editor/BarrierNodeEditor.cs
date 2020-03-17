using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;

[CustomNodeEditor(typeof(BarrierNode))]
public class BarrierNodeEditor : NodeEditor
{


    public override void OnBodyGUI()
    {
        BarrierNode node = target as BarrierNode;
        //if (simpleNode == null) simpleNode = node as DialogNode;

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

        if (node.barriers.Count == 0)
        {
            node.AddBarrier();
        }


        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
        GUILayout.EndHorizontal();

        SerializedProperty barrier_list = serializedObject.FindProperty("barriers");

        testStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Barriers", testStyle);
        for (int i = 0; i < node.barriers.Count; i++)
        {
            SerializedProperty test = barrier_list.GetArrayElementAtIndex(i);

            GUIContent empty = new GUIContent();
            NodeEditorGUILayout.PropertyField(test, empty);

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


    //public override void OnHeaderGUI()
    //{
    //    GUI.color = Color.white;
    //    DialogNode node = target as DialogNode;
    //    ConvoGraph graph = node.graph as ConvoGraph;
    //    // if (graph.current == node) GUI.color = Color.blue;
    //    string title = target.name;
    //    GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    //    GUI.color = Color.white;




    //}
}
