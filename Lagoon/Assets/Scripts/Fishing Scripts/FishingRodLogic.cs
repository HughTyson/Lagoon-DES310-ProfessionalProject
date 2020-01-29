using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] Transform FlexibleFishingTip;
    [SerializeField] Transform FixedFishingTip;
    [SerializeField] Transform FixedFishingBottom;

    [SerializeField] Transform[] FishingRodJoints;


 


    void Start()
    {
       

    }


  

    private void FixedUpdate()
    {

     

        float distance = Vector3.Distance(FixedFishingBottom.position, FixedFishingTip.position);



        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            float t = ((float)i / (float)(FishingRodJoints.Length - 1));
            Vector3 valueFixed = Vector3.Lerp(FixedFishingBottom.position, FixedFishingTip.position, t);
            Vector3 valueFlexible = Vector3.Lerp(FixedFishingBottom.position, FlexibleFishingTip.position, t);
            FishingRodJoints[i].transform.position = Vector3.Lerp(valueFixed, valueFlexible, t);
        }
        for (int i = 0; i < FishingRodJoints.Length - 1; i++)
        {
            
          //  FishingRodJoints[i].rotation = Quaternion.LookRotation(FishingRodJoints[i + 1].position - FishingRodJoints[i].position, FishingRodJoints[i].up);
        }

        }



}
