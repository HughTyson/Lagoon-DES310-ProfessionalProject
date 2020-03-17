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

    public override void OnBodyGUI()
    {
        DialogNode node = target as DialogNode;

        markupStyle.richText = true;
        markupStyle.alignment = TextAnchor.UpperLeft;
        markupStyle.fixedWidth = 272;
        markupStyle.fontStyle = FontStyle.Bold;
        boldStyle.fontStyle = FontStyle.Bold;

        // Update serialized object's representation
        serializedObject.Update();

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
            SerializedProperty dialogText = dialogClass.FindPropertyRelative("dialog_text");

            NodeEditorGUILayout.PropertyField(whoIsTalking);
            NodeEditorGUILayout.PropertyField(dialogText);

            if (displayStrings[i].prevString != dialogText.stringValue)
            {
                displayStrings[i].prevString = dialogText.stringValue;
                displayStrings[i].debuggerText.text = SpecialText.DebuggingParse.ParseTextToDebugMarkUpText(displayStrings[i].prevString);
                displayStrings[i].dialogOnlyText.text = SpecialText.DebuggingParse.ParseTextToDialogOnlyString(displayStrings[i].prevString);
            }

            GUILayout.Box(displayStrings[i].debuggerText, markupStyle);
            GUILayout.Box(displayStrings[i].dialogOnlyText, markupStyle);

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
