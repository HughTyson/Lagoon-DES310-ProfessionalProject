using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{

    [SerializeField] Vector3 start_rotation;
    [SerializeField] Vector3 end_rotation;

    TweenManager.TweenPathBundle stars_tween;

    [SerializeField] Material star_mat;
    [SerializeField] Transform stars_transform;
    [HideInInspector] float move_speed = 0.3f;
    TypeRef<float> stars_alpha = new TypeRef<float>(0);
    bool update_stars;

    // Start is called before the first frame update
    void Start()
    {



        stars_tween = new TweenManager.TweenPathBundle(
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(0, 1, 4, TweenManager.CURVE_PRESET.LINEAR)
                       )
                   );

        star_mat.SetFloat("_ShowStars", stars_alpha.value);

    }

    public void StarStart()
    {

        update_stars = true;

        GM_.Instance.tween_manager.StartTweenInstance(
            stars_tween,
            new TypeRef<float>[] { stars_alpha },
            tweenUpdatedDelegate_: StarsAlphaUpdate
        );
    }

    public void EndStars()
    {
        GM_.Instance.tween_manager.StartTweenInstance(
            stars_tween,
            new TypeRef<float>[] { stars_alpha },
            tweenUpdatedDelegate_: StarsAlphaUpdate,
            tweenCompleteDelegate_: StarsReset,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            
           
        );
    }

    void StarsAlphaUpdate()
    {
        star_mat.SetFloat("_ShowStars", stars_alpha.value);
    }

    void StarsReset()
    {
        update_stars = false;
        //transform.rotation.eulerAngles = new Vector3()
    }

    Vector3 move = new Vector3(1, 0, 1);

    // Update is called once per frame
    void Update()
    {
        stars_transform.Rotate(new Vector3(0.3f, 0, 1), (0.1f * GM_.Instance.DayNightCycle.GetTime()) * Time.deltaTime);
        Debug.Log(transform.rotation.eulerAngles);
    }


}
