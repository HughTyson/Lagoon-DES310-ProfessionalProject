using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    [SerializeField] float aspectRatio;
    // Use this for initialization
    void Start()
    {
        GetComponent<Camera>().aspect = aspectRatio;
    }
}
