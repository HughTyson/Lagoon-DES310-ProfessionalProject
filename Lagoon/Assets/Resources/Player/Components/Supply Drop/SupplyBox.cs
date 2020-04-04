using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox : MonoBehaviour
{


    public enum STATE
    {
        DROPPING,
        IN_WATER,
        CAUGHT
    }

    // ==========================================
    //              Visible Variables
    //===========================================

    [SerializeField] public STATE box_state;


    // ==========================================
    //              Hidden Variables
    //===========================================

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (box_state)
        {
            case STATE.DROPPING:
                {
                    //potentially use this state for animation of dropping, also use to check if the box has collided with the water
                }
                break;
            case STATE.IN_WATER:
                {
                    //if in the water then give off the emote of where the box is
                }
                break;
            case STATE.CAUGHT:
                {
                    //might need this state? might not
                }
                break;
            default:
                break;
        }


    }
    

    // returns what the box contains. Will not be a void once a format for the box content is created.
    public void GetBoxContents()
    {

    }
}
