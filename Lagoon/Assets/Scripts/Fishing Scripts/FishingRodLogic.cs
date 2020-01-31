using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] Transform FlexibleFishingTip;
    [SerializeField] Transform FixedFishingTip;
    [SerializeField] Transform FixedFishingBottom;

    [SerializeField] Transform[] FishingRodJoints;


    Vector3 flexibleTipPrevPos;
    Vector3 flexibleTipPos;
    Vector3 flexibleTipVelocity;
    Vector3 flexibleTipVelocityDampened;
    void Start()
    {
        flexibleTipPrevPos = FlexibleFishingTip.position;
        flexibleTipPos = FlexibleFishingTip.position;

    }


  
    public Vector3 GetFlexibleTipVelocity()
    {
        return flexibleTipVelocityDampened;
    }
    private void FixedUpdate()
    {


        float distance = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);



        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            float t = ((float)i / (float)(FishingRodJoints.Length - 1));
            Vector3 valueFixed = Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t);
            Vector3 valueFlexible = Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t);


            //float angle_between = Vector3.Angle(valueFixed - FixedFishingBottom.position, valueFlexible - FixedFishingBottom.position);
            //Vector3 correctly_rotated = Vector3.RotateTowards(valueFixed - FixedFishingBottom.position, valueFlexible - FixedFishingBottom.position, angle_between * t, float.MaxValue);
            FishingRodJoints[i].transform.position =  Vector3.Lerp(valueFixed, valueFlexible, t);
        }
        for (int i = 0; i < FishingRodJoints.Length - 1; i++)
        {
            
          //  FishingRodJoints[i].rotation = Quaternion.LookRotation(FishingRodJoints[i + 1].position - FishingRodJoints[i].position, FishingRodJoints[i].up);
        }

        flexibleTipPrevPos = flexibleTipPos;
        flexibleTipPos = FlexibleFishingTip.position;
        flexibleTipVelocity = (FixedFishingTip.position- FlexibleFishingTip.position) *5.0f;
        flexibleTipVelocityDampened = flexibleTipVelocity;
      //  Vector3.Lerp();

        // Vector3 test = new Vector3(flexibleTipVelocity.x, 0, flexibleTipVelocity.z);
        //flexibleTipVelocityDampened = Vector3.Lerp(Vector3.zero, test, Mathf.Pow(Mathf.Clamp01(test.magnitude / 30.0f),0.1f));


    }



}
