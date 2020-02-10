using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{

    [SerializeField] GameObject prefabFish;
    [SerializeField] int maxFish = 5;
    [SerializeField] float fishSpawnDepth;

    [SerializeField] List<Collider> NoSpawnColliders;



    [System.Serializable]
    public class FishType
    {
        public GameObject fishPrefab;
        public float minRangeTimeUnactiveTillDespawn;
        public float maxRangeTimeUnactiveTillDespawned;

        // add other stuff
    
    }

    [SerializeField] List<FishType> AllowableFishTypes;


    public class Fish
    {
        public float initializedUnactiveDespawnTime;
        public float currentUnactiveDespawnTime;
        public GameObject fishObject;
    }
    List<Fish> fishList = new List<Fish>();



    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (fishList.Count != maxFish)
        {
            SpawnFish();
        }
        
        for (int i = 0; i < fishList.Count; i++)
        {
            if (fishList[i].fishObject != null)
            {
                if (fishList[i].fishObject.GetComponentInChildren<FishLogic>().IsInDespawnableState())
                {
                    fishList[i].currentUnactiveDespawnTime -= Time.deltaTime;

                    if (fishList[i].currentUnactiveDespawnTime <= 0)
                    {
                        Destroy(fishList[i].fishObject);
                        fishList.Remove(fishList[i]);
                    }
                }
                else
                {
                    fishList[i].currentUnactiveDespawnTime = fishList[i].initializedUnactiveDespawnTime;
                }
            }
            else
            {
                fishList.Remove(fishList[i]);
            }
        }

    }

    void SpawnFish()
    {

        Vector3 new_position = new Vector3(0,-fishSpawnDepth,0);

        Bounds bounds = GetComponentInChildren<Collider>().bounds;
        new_position.x = Random.Range(bounds.min.x, bounds.max.x);
        new_position.z = Random.Range(bounds.min.z, bounds.max.z);

        for (int i = 0; i < NoSpawnColliders.Count; i++) // do only one attempt every frame to prevent multiple failed attempts which could cause lag
        {
            if (NoSpawnColliders[i].bounds.Contains(new_position))
            {
                return;
            }
        }

            Fish new_fish = new Fish();

            FishType chosen_fish_type = AllowableFishTypes[Random.Range(0, AllowableFishTypes.Count)]; // give rarity later and bell curve distribution

            new_fish.fishObject = Instantiate(chosen_fish_type.fishPrefab);
            new_fish.initializedUnactiveDespawnTime = Random.Range(chosen_fish_type.minRangeTimeUnactiveTillDespawn, chosen_fish_type.maxRangeTimeUnactiveTillDespawned);
            new_fish.currentUnactiveDespawnTime = new_fish.initializedUnactiveDespawnTime;

            new_fish.fishObject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            new_fish.fishObject.transform.position = new_position;


        fishList.Add(new_fish);


    }
}
