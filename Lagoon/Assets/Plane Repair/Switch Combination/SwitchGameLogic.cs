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

    [SerializeField] bool[] light_active = new bool[4];

    public override void GameInit(Transform segment_transform)
    {
        for (int i = 0; i < 4; i++)
        {
            //Instantiate the switches
            
            switchs[i] = Instantiate(switch_prefab, this.transform);
            switchs[i].transform.position = new Vector3(i + 1, 0, 0);

            switchs[i].activate = switch_info[i].activates;
            switchs[i].deactivate = switch_info[i].deactivates;

            //Instantiate the light

            lights[i] = Instantiate(light_prefab, this.transform);
            lights[i].SetMatOff();
            lights[i].transform.position = new Vector3(i + 1, 1, 0);
            
        }
    }

    // Update is called once per frame
    public override void GameUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            if (switchs[i].just_changed)
            {
                for (int l = 0; l < switchs[i].activate.Count; l++)
                {
                    light_active[switchs[i].activate[l]] = false;
                }
                switchs[i].just_changed = false;
                switchs[i].on = false;
            }

            if (switchs[i].IsActive())
            {
                for (int j = 0; j < switchs[i].activate.Count; j++)
                {
                    light_active[switchs[i].activate[j]] = true;
                }

                for (int k = 0; k < switchs[i].deactivate.Count; k++)
                {
                    light_active[switchs[i].deactivate[k]] = false;
                }
            }
        }

        for(int m = 0; m < 4; m++)
        {
            if(light_active[m])
            {
                lights[m].SetMatOn();
            }
            else if(!light_active[m])
            {
                lights[m].SetMatOff();
            }
        }
    }

    public override void GameCleanUp()
    {
        
        for(int i = 0; i < 4; i++)
        {
            Destroy(switchs[i].gameObject);
            switchs[i] = null;

            Destroy(lights[i].gameObject);
            lights[i] = null;

        }
    }
}
