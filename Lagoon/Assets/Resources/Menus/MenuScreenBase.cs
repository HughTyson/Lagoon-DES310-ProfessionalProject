using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MenuScreenBase : MonoBehaviour
{
    [SerializeField] protected Camera camera_;
    public abstract void EnteredMenu();


    protected TypeRef<float> cameraPositionRef_X = new TypeRef<float>();
    protected TypeRef<float> cameraPositionRef_Y = new TypeRef<float>();
    protected TypeRef<float> cameraPositionRef_Z = new TypeRef<float>();

    protected TypeRef<float> cameraRotationRef_X = new TypeRef<float>();
    protected TypeRef<float> cameraRotationRef_Y = new TypeRef<float>();
    protected TypeRef<float> cameraRotationRef_Z = new TypeRef<float>();

    protected TypeRef<float>[] transitionOutputs;



    protected Vector3 default_cameraPosition;
    protected Vector3 default_cameraRotation;

    protected Vector3 current_cameraRotation;
    protected Vector3 current_cameraPosition;

    protected void SetupDefaults()
    {
       transitionOutputs = new TypeRef<float>[] {cameraPositionRef_X, cameraPositionRef_Y, cameraPositionRef_Z, cameraRotationRef_X, cameraRotationRef_Y, cameraRotationRef_Z };
       default_cameraPosition = camera_.transform.position;
        default_cameraRotation = camera_.transform.rotation.eulerAngles;
    }
}
