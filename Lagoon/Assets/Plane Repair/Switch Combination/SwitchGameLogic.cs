using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct SwitchInformation
{
    [SerializeField] public List<int> activates;
    [SerializeField] public List<int> deactivates;
}

public class SwitchGameLogic : RepairGameBase
{

    public Switch switch_prefab;
    private Switch[] switchs = new Switch[4];

    public Light light_prefab;
    private Light[] lights = new Light[4];

    [SerializeField] SwitchInformation[] switch_info = new SwitchInformation[4];

    bool[] l_active = new bool[4];

    [SerializeField] bool[] light_active = new bool[4];

    int selected_switch = 0;
    float counter;
    float transition_time = 0.3f;
    bool selected = false;

    public override void GameInit(Vector3 position)
    {
        transform.position = position;

        for (int i = 0; i < 4; i++)
        {
            //Instantiate the switches

            switchs[i] = Instantiate(switch_prefab, this.transform);
            switchs[i].transform.position = new Vector3(transform.position.x + (i * 0.1f), transform.position.y, transform.position.z);

            switchs[i].activate = switch_info[i].activates;
            switchs[i].deactivate = switch_info[i].deactivates;

            //Instantiate the light

            lights[i] = Instantiate(light_prefab, this.transform);
            lights[i].SetMatOff();
            lights[i].transform.position = new Vector3(transform.position.x + (i * 0.1f), transform.position.y + 0.1f, transform.position.z);


            l_active[i] = light_active[i];

        }
    }

    // Update is called once per frame
    public override void GameUpdate()
    {

        HandelInput();
        
        for (int i = 0; i < 4; i++)
        {

            if(i == selected_switch)
            {
                switchs[selected_switch].SetMatOn();

            }
            else
            {
                switchs[i].SetMatOff();
            }

            if (switchs[i].just_changed)
            {

                for (int l = 0; l < switchs[i].activate.Count; l++)
                {
                    if (switchs[i].on)
                    {
                        if (l_active[switchs[i].activate[l]] == true)
                        {
                            l_active[switchs[i].activate[l]] = false;
                        }
                        else
                        {
                            l_active[switchs[i].activate[l]] = true;
                        }
                    }
                    else if (!switchs[i].on)
                    {
                        if (l_active[switchs[i].activate[l]] == false)
                        {
                            l_active[switchs[i].activate[l]] = true;
                        }
                        else
                        {
                            l_active[switchs[i].activate[l]] = false;
                        }
                    }


                }

                switchs[i].just_changed = false;
            }
        }

        for (int m = 0; m < 4; m++)
        {
            if (l_active[m])
            {
                lights[m].SetMatOn();
            }
            else if (!l_active[m])
            {
                lights[m].SetMatOff();
            }
        }

        if(l_active[0] && l_active[1] && l_active[2] && l_active[3])
        {
            Debug.Log("Hello");
            game_complete = true;
        }
    }

    public override void GameCleanUp()
    {

        for (int i = 0; i < 4; i++)
        {
            Destroy(switchs[i].gameObject);
            switchs[i] = null;

            Destroy(lights[i].gameObject);
            lights[i] = null;

            l_active[i] = false;
        }
    }

    void HandelInput()
    {
        if(GM_.instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            switchs[selected_switch].selectedSwitch();
        }

        JoyStickUpdate(selected_switch);
    }

    void JoyStickUpdate(int selection_switch)
    {
        if (GM_.instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
        {
            if (counter >= transition_time)
            {
                counter = 0;
                selected_switch++;

                if (selected_switch > switchs.Length - 1)
                {
                    selected_switch = 0;
                }

            }
            else
            {
                counter += Time.deltaTime;
            }
        }
        else if (GM_.instance.input.GetAxis(InputManager.AXIS.LH) < -0.2)
        {
            if (counter >= transition_time)
            {
                counter = 0;
                selected_switch--;

                if (selected_switch < 0)
                {
                    selected_switch = switchs.Length - 1;
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

