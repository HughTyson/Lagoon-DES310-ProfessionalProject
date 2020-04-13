using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{

    [SerializeField] Transform camera;


    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] PlayerConversationState convo;

    Vector3 look_at = new Vector3(0, 5, 0);


    [SerializeField] Transform radio;

    bool distance = false;


    void Awake()
    {
        GM_.Instance.story.Event_ConvoExit += Story_Event_ConvoExit;
    }

    private void Story_Event_ConvoExit()
    {

        GM_.Instance.DayNightCycle.SetBaseTime(1);
        GM_.Instance.DayNightCycle.SetTime();


        GM_.Instance.scene_manager.ChangeScene(2);

        distance = true;

    }

    // Start is called before the first frame update
    void Start()
    {


        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;




    }

    // Update is called once per frame
    void Update()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

        if(distance)
        {

            camera.transform.rotation = Quaternion.LookRotation(look_at - transform.position);
            Debug.Log(GM_.Instance.DayNightCycle.GetTime());
        }

    }
}
