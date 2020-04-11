using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[NodeEditor.CustomNodeEditor(typeof(AdditionalInfoNode_CertainItemsInNextSupplyDrop))]
public class AdditionalInfoNode_CertainItemsInNextSupplyDropEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        AdditionalInfoNode_CertainItemsInNextSupplyDrop node = target as AdditionalInfoNode_CertainItemsInNextSupplyDrop;
        //if (simpleNode == null) simpleNode = node as DialogNode;



        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("+"))
        {
            node.items.Add(new AdditionalInfoNode_CertainItemsInNextSupplyDrop.Items());
            serializedObject.Update();
        }

        SerializedProperty items = serializedObject.FindProperty("items");

        testStyle.fontStyle = FontStyle.Bold;

        GUIStyle testStyle2 = new GUIStyle(testStyle);
        testStyle2.alignment = TextAnchor.MiddleCenter;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Items", testStyle2);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Type", testStyle);
        GUILayout.Label("Ammount", testStyle);
        GUILayout.EndHorizontal();

        for (int i = 0; i < node.items.Count; i++)
        {
            GUILayout.BeginHorizontal();
            SerializedProperty test = items.GetArrayElementAtIndex(i);

            GUIContent empty = new GUIContent();
            NodeEditorGUILayout.PropertyField(test.FindPropertyRelative("s_type"), empty);
            NodeEditorGUILayout.PropertyField(test.FindPropertyRelative("s_ammount"), empty);
            if (GUILayout.Button("X"))
            {
                node.items[i].FlagToRemove();
            }
            GUILayout.EndHorizontal();
        }

        node.RemoveFlaggedItems();


        AdditionalInfoNode_CertainItemsInNextSupplyDrop.DebugInfo debugInfo = node.DebugParse();


        testStyle.alignment = TextAnchor.MiddleLeft;
        testStyle.fontStyle = FontStyle.Bold;
        testStyle.normal.textColor = new Color(1, 0.3f, 0.3f);

        for (int i = 0; i < debugInfo.Errors.Count; i++)
        {
            GUILayout.Label(debugInfo.Errors[i], testStyle);
            GUILayout.Label("");
        }


        serializedObject.ApplyModifiedProperties();
    }

}
