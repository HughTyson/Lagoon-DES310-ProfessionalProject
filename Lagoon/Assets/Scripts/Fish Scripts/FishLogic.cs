using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FishLogic : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float maxVelocity;
    [SerializeField] float maxForce;
    [SerializeField] Vector2 targetLocation;
    [SerializeField] float targetRadius;

    [SerializeField] float wanderingTargetRadius;
    [SerializeField] float wanderingTargetDistance;
    enum STATE
    {
        WANDERING,
        AVOIDING,
        ATTRACTED
    }




    float prev_angle;
    float angle_change_tick = 0;
    void Start()
    {
         prev_angle = Random.Range(0, Mathf.PI * 2.0f);

    }
    /*
        PVector location;
        PVector velocity;
        PVector acceleration;
        
    */
    void FixedUpdate()
    {
        wander();

        Vector2 velocityUnitXZ = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(velocityUnitXZ.x, 0, velocityUnitXZ.y), Vector3.up);
    }



    void wander()
    {
        Vector2 velocityXZ = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z);

        angle_change_tick -= Time.deltaTime;
        if (angle_change_tick < 0)
        {
            prev_angle = Random.Range(0, Mathf.PI * 2.0f);
            angle_change_tick = Random.Range(0.1f,1);
        }

        Vector2 target = new Vector2(Mathf.Cos(prev_angle) * wanderingTargetRadius, Mathf.Sin(prev_angle) * wanderingTargetRadius);

        if (velocityXZ.magnitude < 0.000001) // no direction vector
        {
            target += new Vector2(transform.position.x, transform.position.z);
            seek(target);
        }
        else
        {
            target += new Vector2(transform.position.x, transform.position.z);
            target += velocityXZ.normalized * wanderingTargetDistance;
            seek(target);
        }
    }

    void seek(Vector2 target)
    {
        Vector2 location = new Vector2(transform.position.x, transform.position.z);
        Vector2 desired = target - location;
        desired = desired.normalized;   
        desired *= maxVelocity;

        Vector2 velocityXZ = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(desired - velocityXZ,maxForce);
        

        GetComponent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));

        //transform.rotation = Quaternion.LookRotation(new Vector3(steer.x, 0, steer.y), Vector3.up);
    }

    void arrive(Vector2 target)
    {
        Vector2 location = new Vector2(transform.position.x, transform.position.z);
        Vector2 desired = target - location;
        float desired_dist = desired.magnitude;

        desired = desired.normalized;

        desired *= Mathf.Lerp(0, maxVelocity, desired_dist / targetRadius);


        Vector2 velocityXZ = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(desired - velocityXZ, maxForce);

        GetComponent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));



    }
    void flee(Vector2 fleeingTarget)
    {
        Vector2 location = new Vector2(transform.position.x, transform.position.z);
        Vector2 desired =  location - fleeingTarget;
        desired = desired.normalized;
        desired *= maxVelocity;

        Vector2 velocityXZ = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(desired - velocityXZ, maxForce);

        GetComponent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));

        //transform.rotation = Quaternion.LookRotation(new Vector3(steer.x, 0, steer.y), Vector3.up);
    }
}
