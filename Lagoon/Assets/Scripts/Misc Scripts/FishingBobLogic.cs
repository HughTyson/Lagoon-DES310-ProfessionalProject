using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobLogic : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float bobBoiyancy;
    [SerializeField] float bobAirDrag;
    [SerializeField] float bobWaterDrag;
    void Start()
    {
        
    }

    // Update is called once per frame

    // Reminder:: Occurs at a FixedTimeStep

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Water")
        {
            GetComponentInParent<Rigidbody>().drag = bobWaterDrag;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Water")
        {
            if (other.bounds.Contains(transform.position))
            {
                Vector3 force = transform.up * (-Physics.gravity.y + bobBoiyancy);
                GetComponentInParent<Rigidbody>().AddRelativeForce(force, ForceMode.Acceleration);
            }

        }

      
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Water")
        {
           GetComponentInParent<Rigidbody>().drag = bobAirDrag;
        }

    }
}
