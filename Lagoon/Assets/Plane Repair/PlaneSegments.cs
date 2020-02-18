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
        NONE,
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
        FUSELAGE_RIGHT_BACK
    }

    [SerializeField] public SegmentType type;
    [SerializeField] float transition_time;

    [SerializeField] ButtonUIManager buttonUIManager;


    [SerializeField] List<OnSegment> games;

    // ==========================================
    //              Hidden Variables
    //===========================================


    

    int selected_game;

    float counter;
    
    bool selected = false;
    bool needs_init = true;


    public void SegmentUpdate()
    {

        HandelInput();

        switch (games[selected_game].game.type)
        {
            case RepairGameBase.GameType.SwitchGame:
                {

                    buttonUIManager.DisableAllButtons();
                    Debug.Log("HELLO");
                    if (!selected)
                    {
                        Debug.Log("HELLO");
                        buttonUIManager.EnableButton(ButtonUIManager.BUTTON_TYPE.A, "Switch Game");
                    }

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

        selected = false;
        needs_init = true;


    }

    void HandelInput()
    {

        if (GM_.instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            selected = true;
        }

        if(GM_.instance.input.GetButtonDown(InputManager.BUTTON.B))
        {
            if(selected)
            {
                CleanUp();
            }
        }

        if (!selected)
        {
            if (GM_.instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
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

            if (GM_.instance.input.GetAxis(InputManager.AXIS.LH) < 0.2)
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
