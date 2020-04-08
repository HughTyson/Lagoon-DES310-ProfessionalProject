using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageContent : MonoBehaviour
{
    public Vector3 UnactivePosition => unactivePosition;

    Vector3 unactivePosition;

    public Quaternion UnactiveRotation => unactiveRotation;
    Quaternion unactiveRotation;

    private void Awake()
    {
        unactivePosition = transform.position;
        unactiveRotation = transform.rotation;
    }

    public void LateUpdate()
    {

        
    }
}
