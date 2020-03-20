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
        //if (simpleNode == null) simpleNode = node as DialogNode;

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

       // GUI.contentColor // use this to make intelisense parser!

        GUIContent empty_text = new GUIContent();

        GUILayout.BeginVertical();
        GUILayout.Label("Default Speed Per Text Character");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultSpeedPerTextCharacter"), empty_text);
        GUILayout.Label("Default Duration OF Text Character Transition In");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultDurationOfTextCharacterTransitionIn"), empty_text);
        GUILayout.EndVertical();


        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}