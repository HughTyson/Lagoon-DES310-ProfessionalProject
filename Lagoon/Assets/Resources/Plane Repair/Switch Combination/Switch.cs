using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public List<int> activate;
    public List<int> deactivate;

    public bool on;

    public bool just_changed;

    public Material mat_on = null;
    public Material mat_off = null;

    TweenManager.TweenPathBundle rotation_tween;

    Quaternion off_rot = new Quaternion(-0.3f,-0.1f, -0.7f, 0.6f);// = new Vector3(39.447f, -155.355f, -68.788f);
    Quaternion on_rot = new Quaternion(0.1f, 0.2f, -0.8f, 0.6f);// = new Vector3(-60-067, 1.516f, 10.23f);

    public enum SwitchState
    {
        OFF,
        ON,
        NEUTRAL
    }

    public SwitchState state;
    Quaternion rot;

    public void Start()
    {

        state = SwitchState.NEUTRAL;

        transform.rotation = off_rot;

    }

    public bool IsActive()
    {
        return on;
    }

    public void selectedSwitch()
    {
        Debug.Log(just_changed);

        just_changed = true;

        if (on)
        {
            on = false;
        }
        else if (!on)
        {
            on = true;
        }
    }

    public void SetMatOff()
    {
        GetComponent<MeshRenderer>().sharedMaterial = mat_off;
    }

    public void SetMatOn()
    {
        GetComponent<MeshRenderer>().sharedMaterial = mat_on;
    }

    public void Rotate(TweenManager.DIRECTION direction)
    {
        //GM_.Instance.tween_manager.StartTweenInstance(
        //    rotation_tween,
        //    new TypeRef<float>[] { x, y, z },
        //    tweenUpdatedDelegate_: UpdateRot,
        //    startingDirection_: direction
        //);
    }

    private void Update()
    {
        switch (state)
        {
            case SwitchState.OFF:
                {
                    rot = Quaternion.Lerp(transform.rotation, off_rot, 1.0f);
                    transform.rotation = rot;

                    if (transform.rotation == off_rot)
                    {
                        state = SwitchState.NEUTRAL;
                    }

                }
                break;
            case SwitchState.ON:
                {
                    rot = Quaternion.Lerp(transform.rotation, on_rot, 1.0f);
                    transform.rotation = rot;

                    if(transform.rotation == on_rot)
                    {
                        state = SwitchState.NEUTRAL;
                    }
                }
                break;
            case SwitchState.NEUTRAL:
                {

                }
                break;
            default:
                break;
        }

        //transform.rotation = rot;

    }


}
