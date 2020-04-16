using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FireChanger : MonoBehaviour
{


    [SerializeField] ParticleSystem particle_system_fireAlpha;
    [SerializeField] ParticleSystem particle_system_fireAdd;     //secodn layer which uses add m modifier... the bright yellow bit in the middle
    [SerializeField] ParticleSystem particle_system_glow;        
    [SerializeField] ParticleSystem particle_system_sparks;      //obvious

    [SerializeField] float fireAlpha_rateovertime_night;
    [SerializeField] float fireAdd_rateovertime_night;
    [SerializeField] float glow_rateovertime_night;
    [SerializeField] float sparks_rateovertime_night;

    [SerializeField] UnityEngine.Experimental.VFX.VisualEffect sparks;


    TypeRef<float> fireAlpha_rateovertime_day = new TypeRef<float>();
    TypeRef<float> fireAdd_rateovertime_day = new TypeRef<float>();
    TypeRef<float> glow_rateovertime_day = new TypeRef<float>();
    TypeRef<float> sparks_rateovertime_day = new TypeRef<float>();

    TimeMovement.Solar current;
    TimeMovement.Solar old;

    TweenManager.TweenPathBundle slowdown_tween;

    bool update_vars = true;

    ParticleSystem.EmissionModule em_fireAdd;
    ParticleSystem.EmissionModule em_fireAlpha;
    ParticleSystem.EmissionModule em_glow;
    ParticleSystem.EmissionModule em_sparks;

    // Start is called before the first frame update
    void Start()
    {

        slowdown_tween = new TweenManager.TweenPathBundle(
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 15.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 15.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 15.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 20.0f, TweenManager.CURVE_PRESET.LINEAR))
        );

        em_fireAdd= particle_system_fireAdd.emission;
        em_fireAlpha = particle_system_fireAlpha.emission;
        em_glow = particle_system_glow.emission;
        em_sparks = particle_system_sparks.emission;

        old = TimeMovement.Solar.DAY;

        em_fireAdd.rateOverTime = 0;
        em_fireAlpha.rateOverTime = 0;
        em_glow.rateOverTime = 0;
        em_sparks.rateOverTime = 0;

        sparks.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {


        current = GM_.Instance.DayNightCycle.GetSolar();



        if (old != current)
        {
            old = current;



            switch (current)
            {
                case TimeMovement.Solar.DAY:
                    {

                            GM_.Instance.tween_manager.StartTweenInstance(
                                slowdown_tween,
                                new TypeRef<float>[] { fireAdd_rateovertime_day, fireAlpha_rateovertime_day, glow_rateovertime_day, sparks_rateovertime_day },
                                tweenUpdatedDelegate_: UpdateFire,
                                tweenCompleteDelegate_: CompleteFire
                            );
                        

                    }
                    break;
                case TimeMovement.Solar.NIGHT:
                    {

                        Debug.Log("HELLO");

                        sparks.enabled = true;

                        em_fireAdd.rateOverTime = fireAdd_rateovertime_night;
                        em_fireAlpha.rateOverTime = fireAlpha_rateovertime_night;
                        em_glow.rateOverTime = glow_rateovertime_night;
                        em_sparks.rateOverTime = sparks_rateovertime_night;



                    }
                    break;
                default:
                    break;
            }
            //particle_system_fireAlpha.emission.rateOverTime.mode = ParticleSystemCurveMode.Constant;



        }

        if(update_vars)
        {

        }

    }

    void UpdateFire()
    {
            em_fireAdd.rateOverTime = fireAdd_rateovertime_day.value;
            em_fireAlpha.rateOverTime = fireAlpha_rateovertime_day.value;
            em_glow.rateOverTime = glow_rateovertime_day.value;
            em_sparks.rateOverTime = sparks_rateovertime_day.value;

            sparks.enabled = false;
    }

    void CompleteFire()
    {
        //enabled = false;
    }
}
