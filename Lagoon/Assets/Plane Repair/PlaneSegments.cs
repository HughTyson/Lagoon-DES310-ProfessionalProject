using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct OnSegment
{
    [SerializeField] public  RepairGameBase game;
    [SerializeField] public Vector3 position;
}


public class PlaneSegments : MonoBehaviour
{

    // ==========================================
    //              Visible Variables
    //===========================================

    public enum SegmentType
    {
        PROPELLER,
        ENGINE_FRONT,
        ENGINE_MID,
        COCKPIT,
        LEFTWING,
        RIGHTWING,
        FUSELAGE_LEFT_FRONT,
        FUSELAGE_LEFT_MID,
        FUSELAGE_LEFT_BACK,
        FUSELAGE_RIGHT_FRONT,
        FUSELAGE_RIGHT_MID,
        FUSELAGE_RIGHT_BACK,
    }

    [SerializeField] public SegmentType type;


    // ==========================================
    //              Hidden Variables
    //===========================================


    List<OnSegment> games;

    int selected_game;

    float counter;
    [SerializeField] float transition_time;
    bool selected = false;
    bool needs_init = true;


    public void SegmentUpdate()
    {

        HandelInput();

        switch (games[selected_game].game.type)
        {
            case RepairGameBase.GameType.SwitchGame:
                {
                    //
                }


                break;
            default:
                break;
        }

        if(selected && needs_init)
        {
            games[selected_game].game.GameInit();
            needs_init = false;
            return;
        }

        if(selected)
        {
            games[selected_game].game.GameUpdate();
        }

    }

    public void CleanUp()
    {
        games[selected_game].game.GameCleanUp();
    }

    void HandelInput()
    {

        if (Input.GetButtonDown("PlayerA"))
        {
            selected = true;
        }

        if (!selected)
        {
            if (Input.GetAxisRaw("PlayerLH") > 0.2)
            {
                if (counter > transition_time)
                {
                    counter = 0;
                    selected_game++;

                    if (selected_game > games.Count)
                    {
                        selected_game = 0;
                    }

                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else
            {
                counter = 0;
            }

            if (Input.GetAxisRaw("PlayerLH") < 0.2)
            {
                if (counter > transition_time)
                {
                    counter = 0;
                    selected_game--;

                    if (selected_game < 0)
                    {
                        selected_game = games.Count;
                    }
                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else
            {
                counter = 0;
            }
        }
    }

    
}
