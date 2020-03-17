﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;


[CustomNodeEditor(typeof(HelpNode))]
public class HelpNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        HelpNode node = target as HelpNode;

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 14;


        GUIStyle normalStyle = new GUIStyle();

        normalStyle.alignment = TextAnchor.UpperLeft;
        normalStyle.fontSize = 12;
        normalStyle.wordWrap = true;
        normalStyle.clipping = TextClipping.Overflow;
        normalStyle.padding = new RectOffset(10, 10, 10, 10);
        normalStyle.stretchWidth = false;
        normalStyle.stretchHeight = false;

        GUIStyle boldStyle = new GUIStyle();

        boldStyle.alignment = TextAnchor.UpperLeft;
        boldStyle.fontSize = 12;
        boldStyle.padding = new RectOffset(10, 10, 5, 0);
        boldStyle.fontStyle = FontStyle.Bold;

        GUILayout.Label("Overview", titleStyle);
        GUILayout.Box(
            "This is a tool which allows the creation of a story based on Nodes",
            normalStyle);
        GUILayout.Label("Node Types", titleStyle);
        GUILayout.Box(
            "Dialog Node: Allows conversation to happen between 2 Characters" + System.Environment.NewLine + System.Environment.NewLine +
            "Branch Node: Splits the conversation into 2 paths which the player can choose from" + System.Environment.NewLine + System.Environment.NewLine +
            "Barrier Node: Stops the continueation of the story until the player has completed the objectives" + System.Environment.NewLine + System.Environment.NewLine +
            "Event Node: Triggers an event" + System.Environment.NewLine + System.Environment.NewLine +
            "(REQUIRED) Root Node: The starting node. Acts like a barrier node." + System.Environment.NewLine + System.Environment.NewLine +
            "(REQUIRED) Global Properties Node: The properties the nodes will abide to in-game",
            normalStyle);
        GUILayout.Label("Property Programming", titleStyle);
        GUILayout.Box(
            "All TextBoxes parsed for additional 'Properties'. These change the formatting of the text when it will be shown in game." + System.Environment.NewLine +
            "Properties are similar to HTML tags." + System.Environment.NewLine +
            "To give a peice of text a proprty, the text must be inside a:" + System.Environment.NewLine +
            "   <#> & </#>" + System.Environment.NewLine +
            "where the # is the property and the & is the text." + System.Environment.NewLine +
            "Below are all the possible properties."
            , normalStyle);
        GUILayout.Label("Property Sheet", titleStyle);

        GUILayout.Label("", boldStyle);
        GUILayout.Label("Delay", boldStyle);
        GUILayout.Box(
            "Overview: Stops showing text for a length of time" + System.Environment.NewLine +
            "Tags: <delay(#d)>  </delay>" + System.Environment.NewLine +
            "Parameters:" + System.Environment.NewLine +
            "   #d: duration of delay" + System.Environment.NewLine +
            "Example: <delay(0.3)> exampleText </delay>"
            , normalStyle);

        GUILayout.Label("Shiver", boldStyle);

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
