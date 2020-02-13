using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;
public class ConvoManager : MonoBehaviour
{

    [SerializeField] ConvoGraph convoGraph;

    BaseNodeType current_node;
    ConversationCharacter leftCharacter = null;
    ConversationCharacter rightCharacter = null;


    void Start()
    {

       // current_node = convoGraph.Init();
    }

    // Update is called once per frame
    void Update()
    {

        //switch (current_node.GetNodeType())
        //{
        //    case BaseNodeType.NODE_TYPE.ROOT:
        //        {
        //            current_node = ((RootNode)current_node).NextNode();
        //            break;
        //        }
        //    case BaseNodeType.NODE_TYPE.DIALOG:
        //        {
        //                break;
        //        }
        //    case BaseNodeType.NODE_TYPE.BRANCH:
        //        {

        //            break;
        //        }
        //    case BaseNodeType.NODE_TYPE.EVENT:
        //        {

        //            break;
        //        }
        //    case BaseNodeType.NODE_TYPE.BARRIER:
        //        {

        //            break;
        //        }

        //}
                
    }

    public BaseNodeType.NODE_TYPE GetCurrentNodeType()
    {
        return current_node.GetNodeType();
    }
    public BaseNodeType GetCurrentNode()
    {
        return current_node;
    }


}



