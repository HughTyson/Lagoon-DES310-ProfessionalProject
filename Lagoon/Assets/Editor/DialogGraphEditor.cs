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

        GUIContent empty = new GUIContent();
        for (int i = 0; i < node.Dialog.Count; i++)
        {
            SerializedProperty test = dialog_list.GetArrayElementAtIndex(i);


            NodeEditorGUILayout.PropertyField(test, empty);

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





        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));

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

[CustomNodeEditor(typeof(RootNode))]
public class RootNodeEditor : NodeEditor
{

    
    public override void OnBodyGUI()
    {
        RootNode node = target as RootNode;
        //if (simpleNode == null) simpleNode = node as DialogNode;

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

        if (node.barriers.Count == 0)
        {
            node.AddBarrier();
        }



            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));

        SerializedProperty barrier_list = serializedObject.FindProperty("barriers");

        testStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Barriers", testStyle);
        for (int i = 0; i < node.barriers.Count; i++)
        {
            SerializedProperty test = barrier_list.GetArrayElementAtIndex(i);

            GUIContent empty = new GUIContent();
            NodeEditorGUILayout.PropertyField(test, empty);

        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            node.AddBarrier();
        }

        if (node.barriers.Count > 1)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Remove"))
            {
                node.RemoveBarrier();
            }
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();

        testStyle.alignment = TextAnchor.MiddleCenter;
        testStyle.fontStyle = FontStyle.Bold;
        testStyle.normal.textColor = new Color(1, 0, 0);
        GUILayout.Label("Output has to connenct to a dialog node.", testStyle);
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

[CustomNodeEditor(typeof(BarrierNode))]
public class BarrierNodeEditor : NodeEditor
{


    public override void OnBodyGUI()
    {
        BarrierNode node = target as BarrierNode;
        //if (simpleNode == null) simpleNode = node as DialogNode;

        // Update serialized object's representation
        serializedObject.Update();

        // NodeEditorGUILayout.DrawPortHandle(new Rect(0, 0, 5, 5), Color.green, Color.yellow) ;
        GUIStyle testStyle = new GUIStyle();

        if (node.barriers.Count == 0)
        {
            node.AddBarrier();
        }


        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
        GUILayout.EndHorizontal();

        SerializedProperty barrier_list = serializedObject.FindProperty("barriers");

        testStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Barriers", testStyle);
        for (int i = 0; i < node.barriers.Count; i++)
        {
            SerializedProperty test = barrier_list.GetArrayElementAtIndex(i);

            GUIContent empty = new GUIContent();
            NodeEditorGUILayout.PropertyField(test, empty);

        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            node.AddBarrier();
        }

        if (node.barriers.Count > 1)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Remove"))
            {
                node.RemoveBarrier();
            }
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();

        testStyle.alignment = TextAnchor.MiddleCenter;
        testStyle.fontStyle = FontStyle.Bold;
        testStyle.normal.textColor = new Color(1, 0, 0);
        GUILayout.Label("Output has to connenct to a dialog node.", testStyle);
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
            "Event Node: Triggers an event" +System.Environment.NewLine + System.Environment.NewLine +
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
        GUILayout.EndVertical();


        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}