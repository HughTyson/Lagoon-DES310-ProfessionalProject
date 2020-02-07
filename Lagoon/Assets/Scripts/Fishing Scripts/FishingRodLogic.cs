using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] FishingLineLogic fishingLineLogic;

    [SerializeField] Transform FlexibleFishingTip;
    [SerializeField] Transform FixedFishingTip;
    [SerializeField] Transform FixedFishingBottom;

    [SerializeField] Transform[] FishingRodJoints;



    void Start()
    {
        //float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);
        //SoftJointLimit limit = GetComponent<ConfigurableJoint>().linearLimit;
        //limit.limit = distanceForFixed;
        //limit.contactDistance = 5;
        //GetComponent<ConfigurableJoint>().linearLimit = limit;
    }


   public void SafetyUpdate()
    {
        float distanceForFlex = Vector3.Distance(FixedFishingBottom.position, FlexibleFishingTip.position);
        float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);
        if (distanceForFlex > distanceForFixed)
        {
            FlexibleFishingTip.position = FixedFishingBottom.position + ((FlexibleFishingTip.position - FixedFishingBottom.position).normalized * distanceForFixed);
        }
    }
    private void Update()
    {
        float distanceForFlex = Vector3.Distance(FixedFishingBottom.position, FlexibleFishingTip.position);
        float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);
        if (distanceForFlex > distanceForFixed)
        {
            FlexibleFishingTip.position = FixedFishingBottom.position + ((FlexibleFishingTip.position - FixedFishingBottom.position).normalized * distanceForFixed);
        }
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForceAtPosition(-fishingLineLogic.EndOfLineForce()*GetComponent<Rigidbody>().mass*100.0f, FixedFishingTip.position);
    }

    private void LateUpdate()
    {


        float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);


        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            float t = ((float)i / (float)(FishingRodJoints.Length - 1));
            Vector3 valueFixed = Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t);
            Vector3 valueFlexible = Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t);

            Vector3 middle = Vector3.Lerp(valueFixed, valueFlexible, 0.5f);


            Vector3 perpendicular = Vector3.Cross((valueFlexible - valueFixed).normalized, Vector3.up).normalized;


            //if (Vector3.Dot(perpendicular,(FixedFishingBottom.position - middle)) < 0)
            //{
            //    perpendicular = Vector3.Cross((valueFixed - valueFlexible).normalized, Vector3.up).normalized;
            //}

            Vector3 sphere_centre = middle + perpendicular*distanceForFixed;

            //float angle_between = Vector3.Angle(valueFixed - sphere_centre, valueFlexible - sphere_centre);

            //Vector3 correctly_rotated = Vector3.RotateTowards(valueFixed - sphere_centre, valueFlexible - sphere_centre, 0.1f, float.MaxValue);

            Vector3 correct_direction = Vector3.Slerp((valueFixed - sphere_centre).normalized, (valueFlexible - sphere_centre).normalized, t);
            FishingRodJoints[i].transform.position = correct_direction*Vector3.Distance(sphere_centre, valueFixed) + sphere_centre;
        }
        for (int i = 0; i < FishingRodJoints.Length - 1; i++)
        {
            
          //  FishingRodJoints[i].rotation = Quaternion.LookRotation(FishingRodJoints[i + 1].position - FishingRodJoints[i].position, FishingRodJoints[i].up);
        }
    }



}
