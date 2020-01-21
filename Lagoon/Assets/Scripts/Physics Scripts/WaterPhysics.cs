using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Apply this script to an object which acts as a liquid


[RequireComponent(typeof(Collider))]
public class WaterPhysics : MonoBehaviour
{
    // Start is called before the first frame update



    [Tooltip("The drag multiplier applied to a buoyant object")]
    [SerializeField] float dragMultiplier = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDragMultiplier()
    {
        return dragMultiplier;
    }
}
