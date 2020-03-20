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

    

    [Range(0, 1)] [SerializeField] private float secondsInFullDay = 120f;
    private float current_time;
    private float time_multiplyer = 1f;

    TweenManager.TweenPathBundle atmosphere_tween;
    TweenManager.TweenPathBundle sun_intensity_tween;
    TweenManager.TweenPathBundle moon_intensity_tween;

    TypeRef<float> atmosphere_alter_value = new TypeRef<float>(0);
    TypeRef<float> sun_intesity = new TypeRef<float>(0);
    TypeRef<float> moon_intesity = new TypeRef<float>(0);

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

        sun_intesity.value = sun.intensity;
        moon_intesity.value = moon.intensity;

        sun_intensity_tween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(sun_intesity.value, 0, 1.5f, TweenManager.CURVE_PRESET.EASE_OUT)
            )
        );

        moon_intensity_tween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(moon_intesity.value, 0, 1.5f, TweenManager.CURVE_PRESET.EASE_OUT)
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
                    if (transform.rotation.eulerAngles.z < 1 || transform.rotation.eulerAngles.z > 181)
                    {
                        ChangeLight(ACTIVE.MOON);
                    }
                }
                break;
            case ACTIVE.MOON:
                {
                    if (transform.rotation.eulerAngles.z > 1 && transform.rotation.eulerAngles.z < 181)
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
                    //method one

                    
                    sun.enabled = true;
                    active = ACTIVE.SUN;
                    AtmosphereChange(false);

                    GM_.Instance.tween_manager.StartTweenInstance(
                        sun_intensity_tween,
                        new TypeRef<float>[] { sun_intesity },
                        tweenUpdatedDelegate_: SunIntensity,
                        startingDirection_: TweenManager.DIRECTION.END_TO_START
                    );

                    GM_.Instance.tween_manager.StartTweenInstance(
                        moon_intensity_tween,
                        new TypeRef<float>[] { moon_intesity },
                        tweenUpdatedDelegate_: MoonIntensity,
                        tweenCompleteDelegate_: MoonDisabled
                    );

                }
                break;
            case ACTIVE.MOON:
                {
                    
                    moon.enabled = true;
                    active = ACTIVE.MOON;
                    AtmosphereChange(true);

                    GM_.Instance.tween_manager.StartTweenInstance(
                        sun_intensity_tween,
                        new TypeRef<float>[] { sun_intesity },
                        tweenUpdatedDelegate_: SunIntensity
                        
                    );

                    GM_.Instance.tween_manager.StartTweenInstance(
                        moon_intensity_tween,
                        new TypeRef<float>[] { moon_intesity },
                        tweenUpdatedDelegate_: MoonIntensity,
                        tweenCompleteDelegate_: SunDisabled,
                        startingDirection_: TweenManager.DIRECTION.END_TO_START
                    );

                    

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
        override_atmosphere.value = atmosphere_alter_value.value;
    }

    void SunIntensity()
    {
        sun.intensity = sun_intesity.value;
    }

    void MoonIntensity()
    {
        moon.intensity = moon_intesity.value;
    }

    void SunDisabled()
    {
        sun.enabled = false;
    }

    void MoonDisabled()
    {
        moon.enabled = false;
    }


    

    
}
