using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(DialogNode))]
public class DialogNodeEditor : NodeEditor
{
    class DisplayStrings
    {
        public string prevString = "";
        public GUIContent debuggerText = new GUIContent();
        public GUIContent dialogOnlyText = new GUIContent();
    }
    GUIStyle markupStyle = new GUIStyle(GUI.skin.box);
    GUIStyle boldStyle = new GUIStyle();
    List<DisplayStrings> displayStrings = new List<DisplayStrings>();

    public override int GetWidth()
    {
        DialogNode node = target as DialogNode;
        int padding = 32;
        return (int)node.nodeWidth + padding;
    }
    public override void OnBodyGUI()
    {
        DialogNode node = target as DialogNode;

        markupStyle.richText = true;
        markupStyle.alignment = TextAnchor.UpperLeft;
        markupStyle.fixedWidth = node.nodeWidth;
        markupStyle.fontStyle = FontStyle.Bold;
        boldStyle.fontStyle = FontStyle.Bold;



        // Update serialized object's representation
        serializedObject.Update();

        GUILayout.Label("Scale Width Of Node", boldStyle);
        node.nodeWidth = GUILayout.HorizontalSlider(node.nodeWidth, 300, 1000);

        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
        GUILayout.EndHorizontal();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("leftCharacter"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("rightCharacter"));
        SerializedProperty dialog_list = serializedObject.FindProperty("Dialog");

        GUIContent empty = new GUIContent();

        while (displayStrings.Count < node.Dialog.Count)
        {
            displayStrings.Add(new DisplayStrings());
        }
        while (displayStrings.Count > node.Dialog.Count)
        {
            displayStrings.RemoveAt(displayStrings.Count - 1);
        }


        for (int i = 0; i < node.Dialog.Count; i++)
        {
            SerializedProperty dialogClass = dialog_list.GetArrayElementAtIndex(i);

            GUILayout.Label("Dialog " + (i + 1).ToString(), boldStyle);

            SerializedProperty whoIsTalking = dialogClass.FindPropertyRelative("whoIsTalking");
            string dialogText = dialogClass.FindPropertyRelative("dialog_text").stringValue;

            NodeEditorGUILayout.PropertyField(whoIsTalking);

            if (displayStrings[i].prevString != dialogText)
            {
                displayStrings[i].prevString = dialogText;
                displayStrings[i].debuggerText.text = SpecialText.DebuggingParse.ParseTextToDebugMarkUpText(displayStrings[i].prevString, ((ConvoGraph)node.graph).GlobalProperties);
                displayStrings[i].dialogOnlyText.text = SpecialText.DebuggingParse.ParseTextToDialogOnlyString(displayStrings[i].prevString);
            }

            
            node.Dialog[i].dialog_text = GUILayout.TextArea(node.Dialog[i].dialog_text, markupStyle);
            GUI.color = Color.grey;
            GUILayout.Box(displayStrings[i].debuggerText, markupStyle);
            GUILayout.Box(displayStrings[i].dialogOnlyText, markupStyle);
            GUI.color = Color.white;

            GUILayout.BeginHorizontal();
            if (i != 0)
            {
                if (GUILayout.Button("Move Up"))
                {
                    node.Swap(i, i - 1);
                }

            }
            if (i != node.Dialog.Count - 1)
            {
                if (GUILayout.Button("Move Down"))
                {
                    node.Swap(i, i + 1);
                }
            }

            GUI.color = Color.red;
            if (GUILayout.Button("Remove"))
            {

                node.Dialog.RemoveAt(i);

            }
            GUI.color = Color.white;

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            node.AddDialogStruct();
        }




        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
