using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
struct Hatch
{

    public GameObject hatch;
    public List<GameObject> screws;

    public float y_pos;

    [HideInInspector] public List<TweenManager.TweenPathBundle> tweens;

    [HideInInspector] public List<TypeRef<float>> x_ref;
    [HideInInspector] public List<TypeRef<float>> y_ref;
    [HideInInspector] public List<TypeRef<float>> z_ref;

    public void Init()
    {

        Debug.Log(hatch.transform.position);

        tweens = new List<TweenManager.TweenPathBundle>();
        x_ref = new List<TypeRef<float>>();
        y_ref = new List<TypeRef<float>>();
        z_ref = new List<TypeRef<float>>();

        tweens.Add(
                new TweenManager.TweenPathBundle(
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(hatch.transform.position.x, hatch.transform.position.x, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(hatch.transform.position.y, y_pos, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(hatch.transform.position.z, hatch.transform.position.z - 2, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                       )
                   )
           );

        x_ref.Add(new TypeRef<float>(hatch.transform.position.x));
        y_ref.Add(new TypeRef<float>(hatch.transform.position.y));
        z_ref.Add(new TypeRef<float>(hatch.transform.position.z));

        for (int i = 0; i < 4; i++)
        {

            tweens.Add(
                new TweenManager.TweenPathBundle(
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(screws[i].transform.position.x, screws[i].transform.position.x, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(screws[i].transform.position.y, y_pos, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(screws[i].transform.position.z, screws[i].transform.position.z - 2, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                       )
                   )
            );

                x_ref.Add(new TypeRef<float>(screws[i].transform.position.x));
                y_ref.Add(new TypeRef<float>(screws[i].transform.position.y));
                z_ref.Add(new TypeRef<float>(screws[i].transform.position.z));
        }

    }

    public void MoveHatch(TweenManager.DIRECTION direction, System.Action Update, System.Action Complete)
    {
        for(int i = 0; i < 4; i++)
        {
            GM_.Instance.tween_manager.StartTweenInstance(
                tweens[i],
                new TypeRef<float>[] { x_ref[i], y_ref[i], z_ref[i] },
                startingDirection_: direction
            );
        }

        GM_.Instance.tween_manager.StartTweenInstance(
                tweens[0],
                new TypeRef<float>[] { x_ref[0], y_ref[0], z_ref[0] },
                tweenCompleteDelegate_: Complete,
                tweenUpdatedDelegate_: Update,
                startingDirection_: direction
            );
    }

    public void UpdatePos()
    {
        hatch.transform.position = new Vector3(x_ref[0].value, y_ref[0].value, z_ref[0].value);

        for(int i = 0; i < 4; i++)
        {

            screws[i].transform.position = new Vector3(x_ref[i].value, y_ref[i].value, z_ref[i].value);
        }

    }
}

[System.Serializable]
class OnSegment
{
    [SerializeField] public RepairGameBase game;
    [SerializeField] public Hatch hatch;
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
        TAIL,
        FUSELAGE_RIGHT_FRONT,
        FUSELAGE_RIGHT_MID,
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

        if(games.Count > 0)
        {
            segment_complete = false;
            
        }
        else
        {
            segment_complete = true;
        }

       for(int i = 0; i < games.Count; i++)
        {
            games[i].hatch.Init();
        }

       
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
                                    if(!games[selected_game].game.game_complete)
                                    {
                                        GM_.Instance.ui.helperButtons.DisableAll();
                                        if (!game_selected)
                                        {
                                            GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Open Segment");
                                        }
                                    }

                                }
                                break;
                            default:
                                break;
                        }

                        if (game_selected && needs_init) //initilise the game 
                        {
                            games[selected_game].game.GameInit(games[selected_game].game.transform.position);   //call the init function

                            games[selected_game].hatch.MoveHatch(TweenManager.DIRECTION.START_TO_END, games[selected_game].hatch.UpdatePos, null);

                            needs_init = false;
                            return;
                        }

                        if (game_selected) //if the game has been selected then update the game
                        {
                            games[selected_game].game.GameUpdate();

                            if(games[selected_game].game.game_complete)
                            {
                                segment_complete = true;
                                //games[selected_game].game.GameCleanUp();

                                CleanUp();
                            }
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
        games[selected_game].hatch.MoveHatch(TweenManager.DIRECTION.END_TO_START, games[selected_game].hatch.UpdatePos, DestoryGame);


    }

    void HandelInputGame()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A)) //check if the A button is down to check for an interaction
        {
            game_selected = true;
        }

        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B)) //when b button is selcted then leave the segment
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
            if (GM_.Instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
            {
                if (counter >= transition_time)
                {
                    counter = 0;
                    selected_game++;

                    if (selected_game > games.Count - 1)
                    {
                        selected_game = 0;
                    }

                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else if (GM_.Instance.input.GetAxis(InputManager.AXIS.LH) < -0.2)
            {
                if (counter >= transition_time)
                {
                    counter = 0;
                    selected_game--;

                    if (selected_game < 0)
                    {
                        selected_game = games.Count - 1;
                    }
                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else
            {
                counter = transition_time;
            }
        }
    }

    void DestoryGame()
    {
        games[selected_game].game.GameCleanUp(); //call the cleanup function

        //reset variables in the segment for selectign what mini game
        game_selected = false;
        needs_init = true;

        segment_state = State.GAME_SELECTION;
    }


}
