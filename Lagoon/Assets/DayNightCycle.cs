using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;

public class DayNightCycle : MonoBehaviour
{
    public enum ACTIVE { SUN, MOON };
    public ACTIVE active;



    [SerializeField] Light sun;
    [SerializeField] Light moon;

    [SerializeField] GameObject scene;

    [SerializeField] private ClampedFloatParameter override_atmosphere;
    
    ProceduralSky procedural_sky;

    TypeRef<float> atmosphere_alter_value = new TypeRef<float>(0);

    [Range(0, 1)] [SerializeField] private float secondsInFullDay = 120f;
    private float current_time;
    private float time_multiplyer = 1f;

    TweenManager.TweenPathBundle atmosphere_tween;

    // Start is called before the first frame update
    void Start()
    {

        Volume volume = scene.GetComponent<Volume>();
        ProceduralSky sky;

        if(volume.profile.TryGet<ProceduralSky>(out sky))
        {
            procedural_sky = sky;
        }

        override_atmosphere = procedural_sky.atmosphereThickness;
        atmosphere_alter_value.value = override_atmosphere.value;

        sun.enabled = true;
        moon.enabled = false;

        active = ACTIVE.SUN;

        atmosphere_tween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0.3f, 6.0f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
    }

    // Update is called once per frame
    void Update()
    {

        //rotate the object by a value - predetermined by the designers - this value can also be changed if time is being pushed forward (use tween manager)

        UpdateLight();

        current_time += (Time.deltaTime / secondsInFullDay) * time_multiplyer;

        if (current_time >= 1)
        {
            current_time = 0;
        }

        CorrectTime();

        ResetRotations();

        Debug.Log(transform.rotation.eulerAngles.z);

    }

    void ResetRotations()
    {
        if (transform.rotation.eulerAngles.z > 360.01)
        {
            transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 360);
        }

        if (transform.rotation.eulerAngles.z < 0.01)
        {
            transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 360);
        }
    }

    void CorrectTime()
    {
        switch (active)
        {
            case ACTIVE.SUN:
                {
                    if (transform.rotation.eulerAngles.z < 5 || transform.rotation.eulerAngles.z > 185)
                    {
                        ChangeLight(ACTIVE.MOON);
                    }
                }
                break;
            case ACTIVE.MOON:
                {
                    if (transform.rotation.eulerAngles.z > 5 && transform.rotation.eulerAngles.z < 185)
                    {
                        ChangeLight(ACTIVE.SUN);
                    }
                }
                break;
            default:
                break;
        }
    }

    void ChangeLight(ACTIVE new_light)
    {

        switch (new_light)
        {
            case ACTIVE.SUN:
                {
                    moon.enabled = false;
                    sun.enabled = true;
                    active = ACTIVE.SUN;
                    AtmosphereChange(false);
                }
                break;
            case ACTIVE.MOON:
                {
                    sun.enabled = false;
                    moon.enabled = true;
                    active = ACTIVE.MOON;
                    AtmosphereChange(true);
                }
                break;
            default:
                break;
        }

    }

    void UpdateLight()
    {
        transform.localRotation = Quaternion.Euler(0, 0, (current_time * 360f) - 90);
    }

    void AtmosphereChange(bool change_to_night)
    {

        if(!change_to_night)
        {
            GM_.Instance.tween_manager.StartTweenInstance(
                atmosphere_tween,
                new TypeRef<float>[] { atmosphere_alter_value },
                tweenUpdatedDelegate_: AtmosphereUpdate,
                startingDirection_: TweenManager.DIRECTION.END_TO_START
                );
        }
        else if(change_to_night)
        {
            GM_.Instance.tween_manager.StartTweenInstance(
                atmosphere_tween,
                new TypeRef<float>[] { atmosphere_alter_value },
                tweenUpdatedDelegate_: AtmosphereUpdate
            );
        }

    }

    void AtmosphereUpdate()
    {
        //procedural_sky.atmosphereThickness.value = procedural_sky.atmosphereThickness.value;
        override_atmosphere.value = atmosphere_alter_value.value;
    }
}
