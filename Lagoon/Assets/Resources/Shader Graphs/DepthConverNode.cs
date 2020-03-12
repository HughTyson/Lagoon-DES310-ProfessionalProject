using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.ShaderGraph
{
    //public class DepthConverNode : CodeFunctionNode
    //{

    //}
    // Z buffer to linear depth
    //inline float LinearEyeDepth(float z)
    //{
    //  
    //}

    // z = input depth
    // near_p
    // far_p

    // _ZBufferParams.z  =  ((1.0 - (m_FarClip / m_NearClip)) / 2.0) / m_FarClip;
    // _ZBufferParams.w = (1.0 + (m_FarClip / m_NearClip)) / 2.0 / m_FarClip;
     // return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);

}