using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Audio Music")]

[System.Serializable]
public class AudioMusic : ScriptableObject
{

    [Header("Source Clip")]
    [SerializeField]
    AudioClip audioClip;


    [Header("Default Values")]
    [SerializeField] float volume;

    public float Volume => volume;
    public AudioClip Clip => audioClip; 
}
