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

    GUIStyle labelStyle = new GUIStyle(GUI.skin.label);


    public override int GetWidth()
    {
        BranchingNode node = target as BranchingNode;
        int padding = 32;
        return (int)node.nodeWidth + padding;
    }

    public override void OnBodyGUI()
    {
        BranchingNode node = target as BranchingNode;

      
        serializedObject.Update();

        markupStyle.richText = true;
        markupStyle.alignment = TextAnchor.UpperLeft;
        markupStyle.fixedWidth = node.nodeWidth;
        markupStyle.fontStyle = FontStyle.Bold;
        labelStyle.fontStyle = FontStyle.Bold;


        GUILayout.Label("Scale Width Of Node", labelStyle);
        node.nodeWidth = GUILayout.HorizontalSlider(node.nodeWidth, 300, 1000);

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("additionalInfo"));

        if (LeftPrevText != node.LeftDecision)
        {
            LeftPrevText = node.LeftDecision;
            leftDecisionDebugText.text = SpecialText.DebuggingParse.ParseTextToDebugMarkUpText(LeftPrevText, ((ConvoGraph)node.graph).GlobalProperties);
            leftDecisionDialogText.text = SpecialText.DebuggingParse.ParseTextToDialogOnlyString(LeftPrevText);
        }

        GUILayout.Label("Left Choice", labelStyle);
        node.LeftDecision = GUILayout.TextArea(node.LeftDecision, markupStyle);

        GUI.color = Color.grey;
        GUILayout.Box(
            leftDecisionDebugText, markupStyle
            );
        
        GUILayout.Box(
            leftDecisionDialogText, markupStyle
            );
        GUI.color = Color.white;

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("outputA"));



        if (rightPrevText != node.RightDecision)
        {
            rightPrevText = node.RightDecision;
            rightDecisionDebugText.text = SpecialText.DebuggingParse.ParseTextToDebugMarkUpText(rightPrevText, ((ConvoGraph)node.graph).GlobalProperties);
            rightDecisionDialogText.text = SpecialText.DebuggingParse.ParseTextToDialogOnlyString(rightPrevText);
        }

        GUILayout.Label("Right Choice", labelStyle);
        node.RightDecision = GUILayout.TextArea(node.RightDecision, markupStyle);

        GUI.color = Color.grey;
        GUILayout.Box(
            rightDecisionDebugText, markupStyle
            );
        GUILayout.Box(
            rightDecisionDialogText, markupStyle
            );
        GUI.color = Color.white;

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
