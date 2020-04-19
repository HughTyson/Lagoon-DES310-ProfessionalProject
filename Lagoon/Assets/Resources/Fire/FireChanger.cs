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

    [SerializeField] List<UnityEngine.Experimental.VFX.VisualEffect> fireflys;

    [SerializeField] Material log_mat;

    TypeRef<float> fireAlpha_rateovertime_day = new TypeRef<float>();
    TypeRef<float> fireAdd_rateovertime_day = new TypeRef<float>();
    TypeRef<float> glow_rateovertime_day = new TypeRef<float>();
    TypeRef<float> sparks_rateovertime_day = new TypeRef<float>();
    TypeRef<float> fire_volume = new TypeRef<float>();

    TimeMovement.Solar current;
    TimeMovement.Solar old;

    TweenManager.TweenPathBundle slowdown_tween;
    

    bool update_vars = true;

    ParticleSystem.EmissionModule em_fireAdd;
    ParticleSystem.EmissionModule em_fireAlpha;
    ParticleSystem.EmissionModule em_glow;
    ParticleSystem.EmissionModule em_sparks;

    AudioSFX sfx_fire;
    AudioManager.SFXInstanceInterface fire_crackling;

    bool fade_in = false;
    float delay = 0;


    // Start is called before the first frame update
    void Start()
    {

        sfx_fire = GM_.Instance.audio.GetSFX("Fire_Crackling");

        slowdown_tween = new TweenManager.TweenPathBundle(
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 15.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 15.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 15.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(10, 0, 20.0f, TweenManager.CURVE_PRESET.LINEAR)),
                new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(sfx_fire.Volume, 0, 13.0f, TweenManager.CURVE_PRESET.LINEAR))
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

        for (int i = 0; i < fireflys.Count; i++)
        {
            fireflys[i].enabled = false;
        }

        log_mat.SetColor("_EmissiveColor", Color.black);

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
                            new TypeRef<float>[] { fireAdd_rateovertime_day, fireAlpha_rateovertime_day, glow_rateovertime_day, sparks_rateovertime_day, fire_volume },
                            tweenUpdatedDelegate_: UpdateFire,
                            tweenCompleteDelegate_: CompleteFire
                        );

                        for (int i = 0; i < fireflys.Count; i++)
                        {
                            fireflys[i].enabled = false;
                        }

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

                        for (int i = 0; i < fireflys.Count; i++)
                        {
                            fireflys[i].enabled = true;
                        }

                        fire_crackling = GM_.Instance.audio.PlaySFX(sfx_fire, transform);

                        fade_in = true;

                    }
                    break;
                default:
                    break;
            }


            
        }
        if (fade_in)
        {
            if (delay > 1.3)
            {
                Color current = log_mat.GetColor("_EmissiveColor");

                Color final = new Color(1, 1, 1);

                Color new_colour = Color.Lerp(current, final, Time.deltaTime / 5);

                log_mat.SetColor("_EmissiveColor", new_colour);

                if (new_colour.r + 0.3 >= 1.0f)
                {
                    fade_in = false;
                    log_mat.SetColor("_EmissiveColor", Color.white);
                    delay = 0;
                }
            }
            else
            {
                delay += Time.deltaTime;
            }
        }
    }

    void UpdateFire()
    {
        em_fireAdd.rateOverTime = fireAdd_rateovertime_day.value;
        em_fireAlpha.rateOverTime = fireAlpha_rateovertime_day.value;
        em_glow.rateOverTime = glow_rateovertime_day.value;
        em_sparks.rateOverTime = sparks_rateovertime_day.value;
        fire_crackling.Volume = fire_volume.value;

        //log_mat.SetColor("_EmissiveColor", Color.black);

        sparks.enabled = false;

        Color current = log_mat.GetColor("_EmissiveColor");

        Color final = new Color(0, 0, 0);

        Color mat = Color.Lerp(current, final, Time.deltaTime /4 );

        log_mat.SetColor("_EmissiveColor", mat);

        Debug.Log(mat.ToString());
    }

    void CompleteFire()
    {

        fire_crackling.Stop();
        fire_crackling = null;

        log_mat.SetColor("_EmissiveColor", Color.black);
        //enabled = false;
    }
}
