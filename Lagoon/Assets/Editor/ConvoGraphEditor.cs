using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using System;
using XNode;

[CustomNodeGraphEditor(typeof(ConvoGraph))]
public class ConvoGraphEditor : NodeGraphEditor
{

    public override void OnCreate()
    {
    //    ConvoGraph convo_graph = target as ConvoGraph;

        CreateNode(typeof(GlobalPropertiesNode), Vector2.zero);
        CreateNode(typeof(RootNode), Vector2.zero);
    }



    public override void AddContextMenuItems(UnityEditor.GenericMenu menu)
    {
        Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);

        AddContextMenuItem(typeof(DialogNode), pos, ref menu);
        AddContextMenuItem(typeof(BranchingNode), pos, ref menu);
        AddContextMenuItem(typeof(EventNode), pos, ref menu);
        AddContextMenuItem(typeof(BarrierNode), pos, ref menu);

        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Preferences"), false, () => NodeEditorWindow.OpenPreferences());
        NodeEditorWindow.AddCustomContextMenuItems(menu, target);
    }

    public override void RemoveNode(Node node)
    {
        //BaseNodeType.NODE_TYPE node_type = ((BaseNodeType)node).GetNodeType();
        //if (node_type != BaseNodeType.NODE_TYPE.GLOBAL_PROPERTIES && node_type != BaseNodeType.NODE_TYPE.ROOT)
        //{
        //    base.RemoveNode(node);
        //}
         base.RemoveNode(node);
    }

    void AddContextMenuItem(Type type, Vector2 pos,  ref UnityEditor.GenericMenu menu)
    {
        //Get node context menu path
        string path = GetNodeMenuName(type);
        if (string.IsNullOrEmpty(path)) return ;

        menu.AddItem(new GUIContent(path), false, () => {
            CreateNode(type, pos);
        });
    }


}
