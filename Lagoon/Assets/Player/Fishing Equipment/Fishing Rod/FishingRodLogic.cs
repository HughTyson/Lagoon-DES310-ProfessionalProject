using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] FishingLineLogic fishingLineLogic;

    [SerializeField] Transform FlexibleFishingTip;
    [SerializeField] Transform FixedFishingTip;
    [SerializeField] Transform FixedFishingBottom;

    [SerializeField] Transform FixedFishingRodTransform;

    [SerializeField] Transform[] FishingRodJoints;

    float[] FishingRodJointTs;
    

    Quaternion[] initalAxis;
    void Start()
    {
        FishingRodJointTs = new float[FishingRodJoints.Length];
        initalAxis = new Quaternion[FishingRodJoints.Length];

        float TotalMagnitude = Vector3.Distance(FixedFishingBottom.position, FishingRodJoints[FishingRodJoints.Length - 1].position);
        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            FishingRodJointTs[i] = Vector3.Distance(FixedFishingBottom.position, FishingRodJoints[i].position) / TotalMagnitude;
            initalAxis[i] = FishingRodJoints[i].transform.localRotation;
        }
        //float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);
        //SoftJointLimit limit = GetComponent<ConfigurableJoint>().linearLimit;
        //limit.limit = distanceForFixed;
        //limit.contactDistance = 5;
        //GetComponent<ConfigurableJoint>().linearLimit = limit;

    }


    //public void SafetyUpdate()
    // {
    //     float distanceForFlex = Vector3.Distance(FixedFishingBottom.position, FlexibleFishingTip.position);
    //     float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);
    //     if (distanceForFlex > distanceForFixed)
    //     {
    //         FlexibleFishingTip.position = FixedFishingBottom.position + ((FlexibleFishingTip.position - FixedFishingBottom.position).normalized * distanceForFixed);
    //     }
    // }

    private void OnEnable()
    {
        transform.position = FixedFishingRodTransform.position;
        transform.rotation = FixedFishingRodTransform.rotation;
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        
       GetComponent<Rigidbody>().AddForceAtPosition(-fishingLineLogic.EndOfLineForce()*1000.0f, FixedFishingTip.position);
    }

    private void LateUpdate()
    {
        Test();

        //float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);
        // SafetyUpdate();


        //for (int i = 0; i < FishingRodJoints.Length; i++)
        //{
        //    float t = 1.0f - ((float)i / (float)(FishingRodJoints.Length - 1));
        //    //Vector3 valueFixed = Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t);
        //    //Vector3 valueFlexible = Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t);

        //    //Vector3 middle = Vector3.Lerp(valueFixed, valueFlexible, 0.5f);


        //    //// Vector3 perpendicular = Vector3.Cross((valueFlexible - valueFixed).normalized, Vector3.up).normalized;
        //    //float delta = 0.01f;
        //    //Vector3 direction = Vector3.Slerp((Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t - delta) - middle).normalized, (Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t - delta) - middle).normalized, t);
        //    //float mag = Vector3.Distance(middle, valueFixed);


        //    ////  Vector3 sphere_centre = middle + perpendicular*distanceForFixed;
        //    //FishingRodJoints[i].transform.position = direction * mag + middle;

        //    // Vector3 correct_direction = Vector3.Slerp((valueFixed - sphere_centre).normalized, (valueFlexible - sphere_centre).normalized, t);
        //    //FishingRodJoints[i].transform.position = correct_direction*Vector3.Distance(sphere_centre, valueFixed) + sphere_centre;

        //    Vector3 v2 = FixedFishingBottom.position;
        //    Vector3 v3 = FixedFishingBottom.position;
        //    Vector3 v0 = FlexibleFishingTip.position;

        //    Vector3 v1 = FixedFishingTip.position;


        //    Vector3 newPos = new Vector3(
        //        (1.0f - t) * ((1.0f - t) * ((1.0f - t) * v0.x + t * v1.x) + t * ((1.0f - t) * v1.x + t * v2.x)) + t * ((1.0f - t) * ((1.0f - t) * v1.x + t * v2.x) + t * ((1.0f - t) * v2.x + t * v3.x)),
        //        (1.0f - t) * ((1.0f - t) * ((1.0f - t) * v0.y + t * v1.y) + t * ((1.0f - t) * v1.y + t * v2.y)) + t * ((1.0f - t) * ((1.0f - t) * v1.y + t * v2.y) + t * ((1.0f - t) * v2.y + t * v3.y)),
        //        (1.0f - t) * ((1.0f - t) * ((1.0f - t) * v0.z + t * v1.z) + t * ((1.0f - t) * v1.z + t * v2.z)) + t * ((1.0f - t) * ((1.0f - t) * v1.z + t * v2.z) + t * ((1.0f - t) * v2.z + t * v3.z)));

        //  FishingRodJoints[i].transform.position = newPos;
        //}
    }


    void Test()
    {

        Vector3 FixedDir = (FixedFishingTip.position - FixedFishingBottom.position).normalized;
        Vector3 FlexDir = (FlexibleFishingTip.position - FixedFishingBottom.position).normalized;


        float FixedMag = Vector3.Distance(FixedFishingTip.position, FixedFishingBottom.position);

        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
           // float t = FishingRodJoints[i].position;

            Vector3 CorrectDir = Vector3.Slerp(FixedDir, FlexDir, FishingRodJointTs[i]);
            FishingRodJoints[i].transform.position = (CorrectDir * (FixedMag * FishingRodJointTs[i])) + FixedFishingBottom.position;
        }
        for (int i = 0; i < FishingRodJoints.Length - 1; i++)
        {
             // FishingRodJoints[i].localRotation = Quaternion.Euler(initalAxis[i].eulerAngles - FishingRodJoints[i].transform.worldToLocalMatrix.MultiplyVector((Quaternion.LookRotation(FishingRodJoints[i + 1].position - FishingRodJoints[i].position)).eulerAngles));
              
        }
        
    }
    void Old()
    {

        float distanceForFixed = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);


        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            float t = ((float)i / (float)(FishingRodJoints.Length - 1));
            Vector3 valueFixed = Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t);
            Vector3 valueFlexible = Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t);

            Vector3 middle = Vector3.Lerp(valueFixed, valueFlexible, 0.5f);


             Vector3 perpendicular = Vector3.Cross((valueFlexible - valueFixed).normalized, Vector3.up).normalized;
            float delta = 0.01f;
            Vector3 direction = Vector3.Slerp((Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t - delta) - middle).normalized, (Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t - delta) - middle).normalized, t);
            float mag = Vector3.Distance(middle, valueFixed);


              Vector3 sphere_centre = middle + perpendicular*distanceForFixed;
           //  FishingRodJoints[i].transform.position = direction * mag + middle;

             Vector3 correct_direction = Vector3.Slerp((valueFixed - sphere_centre).normalized, (valueFlexible - sphere_centre).normalized, t);
            FishingRodJoints[i].transform.position = correct_direction*Vector3.Distance(sphere_centre, valueFixed) + sphere_centre;
        }
        for (int i = 0; i < FishingRodJoints.Length - 1; i++)
        {

            //  FishingRodJoints[i].rotation = Quaternion.LookRotation(FishingRodJoints[i + 1].position - FishingRodJoints[i].position, FishingRodJoints[i].up);
        }
    }


}
