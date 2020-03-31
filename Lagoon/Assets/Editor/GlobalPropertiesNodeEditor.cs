using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;





[CustomNodeEditor(typeof(GlobalPropertiesNode))]
public class GlobalPropertiesNodeEditor : NodeEditor
{


    public override void OnBodyGUI()
    {
        DialogNode node = target as DialogNode;

        // Update serialized object's representation
        serializedObject.Update();

        GUIStyle testStyle = new GUIStyle();

        GUIContent empty_text = new GUIContent();

        GUILayout.BeginVertical();
        GUILayout.Label("Default Speed Per Text Character");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultSpeedPerTextCharacter"), empty_text);
        GUILayout.Label("Default Text Colour");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultColour"), empty_text);
        GUILayout.EndVertical();


        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}