using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct OnSegment
{
    [SerializeField] public RepairGameBase game;
    [SerializeField] public Vector3 position;
}

[System.Serializable]
struct Panel
{

}


public class PlaneSegments : MonoBehaviour
{
    // ==========================================
    //                    Enums
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

    enum State
    {
        PANEL,
        GAME_SELECTION
    }

    // ==========================================
    //              Visible Variables
    //===========================================



    [SerializeField] public SegmentType type;
    [SerializeField] float transition_time;

    [SerializeField] ButtonUIManager buttonUIManager;


    [SerializeField] List<OnSegment> games;

    // ==========================================
    //              Hidden Variables
    //===========================================


    State segment_state;
    int selected_game;
    float counter;
    bool game_selected;
    bool needs_init;
    public bool segment_complete;

    private void Start()
    {
        selected_game = 0;
        segment_state = State.PANEL;
        segment_complete = false;
        game_selected = false;
        needs_init = true;
    }

    //update the segment
    public void SegmentUpdate()
    {

        switch (segment_state)
        {
            case State.PANEL:
                { segment_state = State.GAME_SELECTION; Debug.Log("Panel"); }
                break;
            case State.GAME_SELECTION:
                {

                    HandelInputGame();

                    if (games.Count > 0)    //check that there are games available in this segment
                    {
                        switch (games[selected_game].game.type) //Get the type of game that could be selected
                        {
                            case RepairGameBase.GameType.SwitchGame:    //if the game is of type switch
                                {
                                    //change the button ui
                                    buttonUIManager.DisableAllButtons();
                                    if (!game_selected)
                                    {
                                        buttonUIManager.EnableButton(ButtonUIManager.BUTTON_TYPE.A, "Switch Game");
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        if (game_selected && needs_init) //initilise the game 
                        {
                            games[selected_game].game.GameInit();   //call the init function
                            needs_init = false;
                            return;
                        }

                        if (game_selected) //if the game has been selected then update the game
                        {
                            games[selected_game].game.GameUpdate();
                        }
                    }
                    else
                    {
                        segment_complete = true;
                    }

                }
                break;
            default:
                break;
        }
    }

    public void CleanUp() //whne the game has been closed then clean up
    {
        games[selected_game].game.GameCleanUp(); //call the cleanup function

        //reset variables in the segment for selectign what mini game
        game_selected = false;
        needs_init = true;

        segment_state = State.GAME_SELECTION;
    }

    void HandelInputGame()
    {
        if (GM_.instance.input.GetButtonDown(InputManager.BUTTON.A)) //check if the A button is down to check for an interaction
        {
            game_selected = true;
        }

        if (GM_.instance.input.GetButtonDown(InputManager.BUTTON.B)) //when b button is selcted then leave the segment
        {
            if (game_selected)
            {
                CleanUp(); //call the cleanup function for the plane segment
            }
        }

        JoyStickUpdate(selected_game);
    }

    void HandelInputPanel()
    {

    }

    void JoyStickUpdate(int selection_change)
    {
        if (!game_selected)
        {
            if (GM_.instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
            {
                if (counter > transition_time)
                {
                    counter = 0;
                    selection_change++;

                    if (selection_change > games.Count)
                    {
                        selection_change = 0;
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
                    selection_change--;

                    if (selected_game < 0)
                    {
                        selection_change = games.Count;
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
