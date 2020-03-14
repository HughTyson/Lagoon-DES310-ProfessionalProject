using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FishType : ScriptableObject
{

    public enum FISH_TEIR
    {
        T1,
        T2,
        T3,
        T4
    };

    [System.Serializable]

    public class Stats
    {
        public float sizeMin = 1;
        public float sizeMax = 1;
        public float maxForce = 1;
        public float defaultForce = 1;
        public float defaultTurnForce = 1;
        public float maxTurnForce = 1;
        public float maxVelocity = 3;

    }
    public string fishTypeName;


    [SerializeField]
    public GameObject fishPrefab;
    public float minRangeTimeUnactiveTillDespawn;
    public float maxRangeTimeUnactiveTillDespawned;



    [SerializeField]
    public Stats Teir1;
    [SerializeField]
    public Stats Teir2;
    [SerializeField]
    public Stats Teir3;
    [SerializeField]
    public Stats Teir4;


    [HideInInspector]
    public Dictionary<FISH_TEIR,Stats> StatsList = new Dictionary<FISH_TEIR, Stats>();
    public void OnEnable()
    {
        StatsList.Add(FISH_TEIR.T1, Teir1);
        StatsList.Add(FISH_TEIR.T2, Teir2);
        StatsList.Add(FISH_TEIR.T3, Teir3);
        StatsList.Add(FISH_TEIR.T4, Teir4);
       
    }

}
