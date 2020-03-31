using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct PosRot
{
    
    [SerializeField] public float time;

    [SerializeField] public Vector3 position;
    [SerializeField] public Vector3 rotation;

}

public class PlaneAnimation : MonoBehaviour
{

    //[Tooltip("X: Time, Y: Value")]

    //[SerializeField] List<PosRot> key_frame = new List<PosRot>();

    [SerializeField] AnimationClip clip;
    Animation animation;

    private void Start()
    {

        animation = GetComponent<Animation>();

        animation.AddClip(clip, clip.name);
        animation.Play(clip.name);

    }

    // AnimationCurve[] position_curves;
    // AnimationCurve[] rotation_curves;

    //// create a new AnimationClip
    //AnimationClip clip;

    //Keyframe[] x_position_keys;
    //Keyframe[] y_position_keys;
    //Keyframe[] z_position_keys;

    //Keyframe[] x_rotation_keys;
    //Keyframe[] y_rotation_keys;
    //Keyframe[] z_rotation_keys;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Animation animation = GetComponent<Animation>();

    //    position_curves = new AnimationCurve[3];
    //    rotation_curves = new AnimationCurve[3];

    //    //set up all curves
    //    for (int i = 0; i < 3; i++)
    //    {
    //        position_curves[i] = new AnimationCurve();
    //        rotation_curves[i] = new AnimationCurve();
    //    }

    //    clip = new AnimationClip();
    //    clip.legacy = true;

    //    //load the array with data taken from the inspector
    //    x_position_keys = new Keyframe[key_frame.Count];
    //    y_position_keys = new Keyframe[key_frame.Count];
    //    z_position_keys = new Keyframe[key_frame.Count];

    //    x_rotation_keys = new Keyframe[key_frame.Count];
    //    y_rotation_keys = new Keyframe[key_frame.Count];
    //    z_rotation_keys = new Keyframe[key_frame.Count];

    //    for (int i = 0; i < key_frame.Count; i++)
    //    {

    //        x_position_keys[i].inTangent = -3f;
    //        x_position_keys[i].outTangent = -5f;
    //        //x_position_keys[i].inWeight = 1;
    //        //x_position_keys[i].outTangent = 1;

    //        //y_position_keys[i].inWeight = 1;
    //        //y_position_keys[i].outTangent = 1;

    //        //z_position_keys[i].inWeight = 1;
    //        //z_position_keys[i].outTangent = 1;

    //        x_position_keys[i] = new Keyframe(key_frame[i].time, key_frame[i].position.x);
    //        y_position_keys[i] = new Keyframe(key_frame[i].time, key_frame[i].position.y);
    //        z_position_keys[i] = new Keyframe(key_frame[i].time, key_frame[i].position.z);

    //        x_rotation_keys[i] = new Keyframe(key_frame[i].time, key_frame[i].rotation.x);
    //        y_rotation_keys[i] = new Keyframe(key_frame[i].time, key_frame[i].rotation.y);
    //        z_rotation_keys[i] = new Keyframe(key_frame[i].time, key_frame[i].rotation.z);
    //    }


    //    position_curves[0] = new AnimationCurve(x_position_keys);
    //    clip.SetCurve("", typeof(Transform), "localPosition.x", position_curves[0]);

    //    position_curves[1] = new AnimationCurve(y_position_keys);
    //    clip.SetCurve("", typeof(Transform), "localPosition.y", position_curves[1]);

    //    position_curves[2] = new AnimationCurve(z_position_keys);
    //    clip.SetCurve("", typeof(Transform), "localPosition.z", position_curves[2]);

    //    //rotations

    //    rotation_curves[0] = new AnimationCurve(x_rotation_keys);
    //    clip.SetCurve("", typeof(Transform), "localEulerAngles.x", rotation_curves[0]);

    //    rotation_curves[1] = new AnimationCurve(y_rotation_keys);
    //    clip.SetCurve("", typeof(Transform), "localEulerAngles.y", rotation_curves[1]);

    //    rotation_curves[2] = new AnimationCurve(z_rotation_keys);
    //    clip.SetCurve("", typeof(Transform), "localEulerAngles.z", rotation_curves[2]);

    //    clip.EnsureQuaternionContinuity();

    //    // now animate the GameObject
    //    animation.AddClip(clip, clip.name);
    //    animation.Play(clip.name);
    //}

    //public void MovePlane()
    //{
    //    animation.Play(clip.name);
    //}


}
