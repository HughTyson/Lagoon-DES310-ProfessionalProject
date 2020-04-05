using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio SFX")]

[System.Serializable]
public class AudioSFX : ScriptableObject
{
    [Header("Source Clip")]
    [SerializeField]
    AudioClip audioClip;


    [Header("Default Values")]
    [SerializeField]
    bool isLooping = false;
    [Range(-3, 3)]
    [SerializeField]
    float pitch = 1.0f;
    [Range(0,1)]
    [SerializeField]
    float spatialBlend2DTo3D;
    [Range(0, 1)]
    [SerializeField]
    float volume = 1;
    [Range(-1, 1)]
    [SerializeField]
    float panning = 1;

    [Range(0,255)]
    [SerializeField]
    int priority = 128;



    public bool IsLooping => isLooping;
    public float Pitch => pitch;
    public float SpatialBlend2DTo3D => spatialBlend2DTo3D;
    public float Volume => volume;
    public float Panning => panning;
    public int Priority => priority;

    public AudioClip Clip => audioClip;
    

}
