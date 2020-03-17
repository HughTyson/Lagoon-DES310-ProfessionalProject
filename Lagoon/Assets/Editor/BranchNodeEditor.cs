using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;

[CustomNodeEditor(typeof(BranchingNode))]
public class BranchNodeEditor : NodeEditor
{
    GUIStyle markupStyle = new GUIStyle(GUI.skin.box);
    GUIContent leftDecisionDebugText = new GUIContent();
    GUIContent leftDecisionDialogText = new GUIContent();
    string LeftPrevText = "";

    GUIContent rightDecisionDebugText = new GUIContent();
    GUIContent rightDecisionDialogText = new GUIContent();
    string rightPrevText = "";
    public override void OnBodyGUI()
    {
        BranchingNode node = target as BranchingNode;

        serializedObject.Update();


        markupStyle.richText = true;
        markupStyle.alignment = TextAnchor.UpperLeft;
        markupStyle.fixedWidth = 272;
        markupStyle.fontStyle = FontStyle.Bold;


        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));


        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("LeftDecision"));
        if (LeftPrevText != serializedObject.FindProperty("LeftDecision").stringValue)
        {
            LeftPrevText = serializedObject.FindProperty("LeftDecision").stringValue;
            leftDecisionDebugText.text = SpecialText.DebuggingParse.ParseTextToDebugMarkUpText(LeftPrevText);
            leftDecisionDialogText.text = SpecialText.DebuggingParse.ParseTextToDialogOnlyString(LeftPrevText);
        }


        GUILayout.Box(
            leftDecisionDebugText, markupStyle
            );
        GUILayout.Box(
            leftDecisionDialogText, markupStyle
            );
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("outputA"));



        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("RightDecision"));
        if (rightPrevText != serializedObject.FindProperty("RightDecision").stringValue)
        {
            rightPrevText = serializedObject.FindProperty("RightDecision").stringValue;
            rightDecisionDebugText.text = SpecialText.DebuggingParse.ParseTextToDebugMarkUpText(rightPrevText);
            rightDecisionDialogText.text = SpecialText.DebuggingParse.ParseTextToDialogOnlyString(rightPrevText);
        }


        GUILayout.Box(
            rightDecisionDebugText, markupStyle
            );
        GUILayout.Box(
            rightDecisionDialogText, markupStyle
            );

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("outputB"));


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
