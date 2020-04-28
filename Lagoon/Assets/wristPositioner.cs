﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wristPositioner : MonoBehaviour
{
    Animator characterAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        characterAnimator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform leftHandTrans = characterAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
        transform.position = leftHandTrans.position;
        transform.rotation = leftHandTrans.rotation;

    }
}
