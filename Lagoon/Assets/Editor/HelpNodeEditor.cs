using System.Collections;
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

        GUIStyle headerStyle = new GUIStyle();
        headerStyle.alignment = TextAnchor.MiddleLeft;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = 14;

        GUILayout.Label("Pages", headerStyle);
        if (GUILayout.Button("Main"))
        {
            node.page = 0;

        }
        if (GUILayout.Button("Properties"))
        {
            node.page = 1;
        }
        if (GUILayout.Button("Supply Drop Items"))
        {
            node.page = 2;
        }


        // Update serialized object's representation
        serializedObject.Update();

        switch (node.page)
        {
            case 0:
                {
                    DrawMainPage();
                    break;
                }
            case 1:
                {
                    DrawPropertiesPage();
                    break;
                }
            case 2:
                {
                    DrawAdditionalInfoPage();
                    break;
                }
        
        }

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }



    void DrawMainPage()
    {

    }
    void DrawPropertiesPage()
    {

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 14;


        GUIStyle normalStyle = GUI.skin.box;
        normalStyle.fixedWidth = 560;
        normalStyle.alignment = TextAnchor.UpperLeft;

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
            GUI.skin.box);
        GUILayout.Label("Node Types", titleStyle);
        GUILayout.Box(
            "Dialog Node: Allows conversation to happen between 2 Characters" + System.Environment.NewLine + System.Environment.NewLine +
            "Branch Node: Splits the conversation into 2 paths which the player can choose from" + System.Environment.NewLine + System.Environment.NewLine +
            "Barrier Node: Stops the continueation of the story until the player has completed the objectives" + System.Environment.NewLine + System.Environment.NewLine +
            "Event Node: Triggers an event" + System.Environment.NewLine + System.Environment.NewLine +
            "Root Node: The starting node. Acts like a barrier node." + System.Environment.NewLine + System.Environment.NewLine +
            "Global Properties Node: The properties the nodes will abide to in-game",
            GUI.skin.box);
        GUILayout.Label("Property Programming", titleStyle);
        GUILayout.Box(
            "All TextBoxes are parsed for additional 'Properties'. These change the formatting of the text when it will be shown in-game." + System.Environment.NewLine +
            "Properties are similar to HTML tags." + System.Environment.NewLine +
            "Properties are represented within the [ ] tags" +
            "There are 2 types of properties. Enclosed and Singular." + System.Environment.NewLine +
            "Enclosed: Have a start and an end and encapsulated text has the property. e.g.: not red [Colour(255,0,0) red text [/Colour] not red." + System.Environment.NewLine +
            "Singular: Triggers once when reached. e.g: 3.. [#Delay(1)] 2.. [#Delay(1)] 1.. [#Delay(1)] Go!" + System.Environment.NewLine +
            "To Recognise properties easier, Enclosed starts are green, Enclosed ends are orange, and singulars are purple."

            , GUI.skin.box);
        GUILayout.Label("Property Sheet", titleStyle);

        titleStyle.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("Encapsulated Properties", titleStyle);

        for (int i = 0; i < SpecialText.TextProperties.propertyInfos.Length; i++)
        {
            SpecialText.TextProperties.PropertyInfo info = SpecialText.TextProperties.propertyInfos[i];
            GUILayout.Label(info.name, boldStyle);
            GUILayout.Label("Overview: " + info.description, GUI.skin.box);
            GUILayout.Label("Function: " + info.functionName, GUI.skin.box);
            GUILayout.Label("Parameters: " + info.parameters, GUI.skin.box);
            GUILayout.Label("Example: " + info.example, GUI.skin.box);
        }
        GUILayout.Label("Singular Properties", titleStyle);
        for (int i = 0; i < SpecialText.TextProperties.noExitPropertyInfo.Length; i++)
        {
            SpecialText.TextProperties.PropertyInfo info = SpecialText.TextProperties.noExitPropertyInfo[i];
            GUILayout.Label(info.name, boldStyle);
            GUILayout.Label("Overview: " + info.description, GUI.skin.box);
            GUILayout.Label("Function: " + info.functionName, GUI.skin.box);
            GUILayout.Label("Parameters: " + info.parameters, GUI.skin.box);
            GUILayout.Label("Example: " + info.example, GUI.skin.box);
        }

    }

    void DrawAdditionalInfoPage()
    {

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 14;

        GUIStyle boldStyle = new GUIStyle();

        boldStyle.alignment = TextAnchor.UpperLeft;
        boldStyle.fontSize = 12;
        boldStyle.padding = new RectOffset(10, 10, 5, 0);
        boldStyle.fontStyle = FontStyle.Bold;


        GUILayout.Label("Item Types", titleStyle);
        GUILayout.Label("", titleStyle);
        foreach (KeyValuePair<string, System.Type> pair in InventoryItem.InventoryItemsTypes_Dictionary)
        {
            GUILayout.Label(pair.Key, titleStyle);
        }
    }
}
