using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;


[CustomNodeEditor(typeof(DialogNode))]
public class MyNodeEditor : NodeEditor
{


    public override void OnBodyGUI()
    {
        DialogNode node = target as DialogNode;
    

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;


        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
        GUILayout.EndHorizontal();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("leftCharacter"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("rightCharacter"));
        //UnityEditor.EditorGUILayout.LabelField("The value is " + simpleNode.Sum());
        SerializedProperty dialog_list = serializedObject.FindProperty("Dialog");
        for (int i = 0; i < node.Dialog.Count; i++)
        {
            SerializedProperty test = dialog_list.GetArrayElementAtIndex(i);

           
            NodeEditorGUILayout.PropertyField(test);

            GUILayout.BeginHorizontal();
            if (i != 0)
            {
                if (GUILayout.Button("Move Up"))
                {
                    node.Swap(i, i - 1);
                }

            }
            if (i != node.Dialog.Count -1 )
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


    //public override void OnHeaderGUI()
    //{
    //    GUI.color = Color.white;
    //    DialogNode node = target as DialogNode;
    //    ConvoGraph graph = node.graph as ConvoGraph;
    //   // if (graph.current == node) GUI.color = Color.blue;
    //    string title = target.name;
    //    GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    //    GUI.color = Color.white;

        

      
    //}
}



[CustomNodeEditor(typeof(BranchingNode))]
public class BranchNodeEditor : NodeEditor
{


    public override void OnBodyGUI()
    {
        DialogNode node = target as DialogNode;
        //if (simpleNode == null) simpleNode = node as DialogNode;

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

        testStyle.alignment = TextAnchor.MiddleCenter;

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("leftCharacter"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("rightCharacter"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("LeftDecision"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("outputA"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("RightDecision"));
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