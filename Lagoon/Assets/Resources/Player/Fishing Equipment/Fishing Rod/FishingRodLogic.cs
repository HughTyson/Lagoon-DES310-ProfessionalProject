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
    

    Quaternion[] initialLocalRotation;
    Vector3[] initalLocalPositions;
    void Start()
    {
        FishingRodJointTs = new float[FishingRodJoints.Length];
        initialLocalRotation = new Quaternion[FishingRodJoints.Length];
        initalLocalPositions = new Vector3[FishingRodJoints.Length];

        float TotalMagnitude = Vector3.Distance(FixedFishingBottom.position, FishingRodJoints[FishingRodJoints.Length - 1].position);
        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            FishingRodJointTs[i] = Vector3.Distance(FixedFishingBottom.position, FishingRodJoints[i].position) / TotalMagnitude;
            initialLocalRotation[i] = FishingRodJoints[i].transform.localRotation;
            initalLocalPositions[i] = FishingRodJoints[i].localPosition;
        }

    }

    private void OnEnable()
    {
        transform.position = FixedFishingRodTransform.position;
        transform.rotation = FixedFishingRodTransform.rotation;

        GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    private void FixedUpdate()
    {
        if (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED)
            return;

       GetComponent<Rigidbody>().AddForceAtPosition(-fishingLineLogic.EndOfLineForce()*1000.0f, FixedFishingTip.position);
    }

    private void LateUpdate()
    {


        Vector3 FixedDir = (FixedFishingTip.position - FixedFishingBottom.position).normalized;
        Vector3 FlexDir = (FlexibleFishingTip.position - FixedFishingBottom.position).normalized;


        float FixedMag = Vector3.Distance(FixedFishingTip.position, FixedFishingBottom.position);


        // Set position of the joints
        for (int i = 0; i < FishingRodJoints.Length; i++)
        {
            // float t = FishingRodJoints[i].position;

            Vector3 CorrectDir = Vector3.Slerp(FixedDir, FlexDir, FishingRodJointTs[i]);
            FishingRodJoints[i].position = (CorrectDir * (FixedMag * FishingRodJointTs[i])) + FixedFishingBottom.position;
        }


        // Set rotaiton of the joints
        for (int i = 0; i < FishingRodJoints.Length - 1; i++)
        {
            Quaternion rot = Quaternion.FromToRotation((initalLocalPositions[i + 1] - initalLocalPositions[i]).normalized, (FishingRodJoints[i + 1].localPosition - FishingRodJoints[i].localPosition).normalized);
            FishingRodJoints[i].localRotation = rot * initialLocalRotation[i];
        }

        Quaternion finalRot = Quaternion.FromToRotation((initalLocalPositions[initalLocalPositions.Length - 1] - initalLocalPositions[initalLocalPositions.Length - 2]).normalized, (FishingRodJoints[initalLocalPositions.Length - 1].localPosition - FishingRodJoints[initalLocalPositions.Length - 2].localPosition).normalized);
        FishingRodJoints[initalLocalPositions.Length - 1].localRotation = finalRot * initialLocalRotation[initalLocalPositions.Length - 1];
    }



}
