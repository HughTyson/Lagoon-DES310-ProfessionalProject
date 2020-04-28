using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{

    [SerializeField] GameObject prefabFish;

    [SerializeField] float fishSpawnDepth;

    public AnimationCurve plot = new AnimationCurve();


    [SerializeField] Vector2 fishHabitatRingsOriginXZ;
    [System.Serializable]
    public class FishGenerationStats
    {
        public FishType.FISH_TEIR fishTeirEnum;
        public List<GameObject> spawnArea;
        public float habitatRingMin;
        public float habitatRingMax;
        public int maxFish = 5;

        [HideInInspector]
        public List<Fish> fishList = new List<Fish>();

        [HideInInspector]
        public List<float> spawnAreaWeighting = new List<float>();
    }

    [SerializeField] List<FishGenerationStats> fishGenerationStats;

    [SerializeField] List<FishType> AllowableFishTypes;

    [Header("DEBUG")]
    [SerializeField] Material DebugMat;

    public class Fish
    {
        public float initializedUnactiveDespawnTime;
        public float currentUnactiveDespawnTime;
        public GameObject fishObject;

    }



    //public AnimationCurve testing = new AnimationCurve();
    //public AnimationCurve testing2 = new AnimationCurve();
    //public AnimationCurve testing3 = new AnimationCurve();


    // Start is called before the first frame update


    [SerializeField] bool allowBigFishToSpawn = false;
    void AllowBigFishSpawning()
    {
        if (startedBigFishBarrier)
        {
            allowBigFishToSpawn = true;
        }
    }

    bool startedBigFishBarrier = false;
    void bigFishBarrierstart(StoryManager.BarrierStartArgs args)
    {
        for (int i = 0; i < args.Barriers.Count; i++)
        {
            if (args.Barriers[i] == RootNode.BARRIER_STATE.NEXT_DAY)
            {
                startedBigFishBarrier = true;
            }

        }
    }


    void Start()
        {

        GM_.Instance.story.Event_BarrierStart += bigFishBarrierstart;
        GM_.Instance.story.Event_BarrierOpened += AllowBigFishSpawning;

        // create spawner chance weightings based on size of colliders.
        // This makes sure that if colliders have different sizes, they won't all have the same weighting (e.i collider A is 2x bigger than collider B so A should be 2x more likely to be the chosen spawn collider)
        for (int i = 0; i < fishGenerationStats.Count; i++)
        {
            float total_area = 0.0f;

            for (int k = 0; k < fishGenerationStats[i].spawnArea.Count; k++)
            {
                Vector3 size = fishGenerationStats[i].spawnArea[k].transform.lossyScale;

                fishGenerationStats[i].spawnAreaWeighting.Add((size.x * size.z));
                total_area += fishGenerationStats[i].spawnAreaWeighting[k];
            }

            for (int k = 0; k < fishGenerationStats[i].spawnArea.Count; k++)
            {
                fishGenerationStats[i].spawnAreaWeighting[k] /= total_area;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int k = 0; k < fishGenerationStats.Count; k++)
        {
            FishGenerationStats fishRing = fishGenerationStats[k];

            if (fishGenerationStats[k].fishTeirEnum == FishType.FISH_TEIR.T4)
            {
                if (!allowBigFishToSpawn)
                    continue;
            }

            if (fishRing.fishList.Count != fishRing.maxFish)
            {                
                SpawnFish(fishRing);
            }

            for (int i = 0; i < fishRing.fishList.Count; i++)
            {
                Fish fish = fishRing.fishList[i];
                if (fish.fishObject != null)
                {
                    if (fish.fishObject.GetComponentInChildren<FishLogic>().IsInDespawnableState())
                    {
                        fish.currentUnactiveDespawnTime -= Time.deltaTime;

                        if (fish.currentUnactiveDespawnTime <= 0)
                        {
                            //  fish.fishObject.GetComponentInChildren<FishLogic>().Despawn();
                            Destroy(fish.fishObject);
                            fishRing.fishList.Remove(fish);
                        }
                    }
                    else
                    {
                        fish.currentUnactiveDespawnTime = fish.initializedUnactiveDespawnTime;
                    }
                }
                else
                {
                    fishRing.fishList.Remove(fish);
                }
            }

        }

    }

    void SpawnFish(FishGenerationStats fishRing)
    {
        float spawnerDecider = Random.value;
        float currentWeighting = 0.0f;
        int decidedSpawner = 0;

        // decide on which collider to spawn in based on their weighting
        for (int i = 0; i < fishRing.spawnAreaWeighting.Count; i++)
        {
            currentWeighting += fishRing.spawnAreaWeighting[i];
            if (spawnerDecider <= currentWeighting)
            {
                decidedSpawner = i;
                break;
            }
        }
        Vector3 new_position = Vector3.zero;

        // get random point in AABB bounds
        Transform trans = fishRing.spawnArea[decidedSpawner].transform;
        new_position.x = (Random.value - 0.5f) * trans.lossyScale.x;
        new_position.z = (Random.value - 0.5f) * trans.lossyScale.z;



        float spawnedInDepth = -2.0f; 
        // convert point to be OOB
        new_position = trans.rotation * new_position;
        new_position += trans.position;
        new_position.y = spawnedInDepth;

        Fish new_fish = new Fish();



        List<FishType> thisRingsFishTypes = new List<FishType>();
        for (int i = 0; i < AllowableFishTypes.Count; i++)
        {
            if ( AllowableFishTypes[i].StatsList[fishRing.fishTeirEnum].spawnsInThisTeir)
            {
                thisRingsFishTypes.Add(AllowableFishTypes[i]);
            }

        }

        FishType chosen_fish_type = thisRingsFishTypes[Random.Range(0, thisRingsFishTypes.Count)]; // give rarity later and bell curve distribution

        new_fish.fishObject = Instantiate(chosen_fish_type.fishPrefab);
        new_fish.initializedUnactiveDespawnTime = Random.Range(chosen_fish_type.minRangeTimeUnactiveTillDespawn, chosen_fish_type.maxRangeTimeUnactiveTillDespawned);
        new_fish.currentUnactiveDespawnTime = new_fish.initializedUnactiveDespawnTime;

        new_fish.fishObject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        new_fish.fishObject.transform.position = new_position;
        new_fish.fishObject.transform.localScale = Vector3.one;

        FishLogic.VarsFromFishGenerator fish_vars = new FishLogic.VarsFromFishGenerator();
        fish_vars.habitatRingOrigin = fishHabitatRingsOriginXZ;
        fish_vars.habitatRingMin = fishRing.habitatRingMin;
        fish_vars.habitatRingMax = fishRing.habitatRingMax;
        fish_vars.defaultForce = chosen_fish_type.StatsList[fishRing.fishTeirEnum].defaultForce;
        fish_vars.maxForce = chosen_fish_type.StatsList[fishRing.fishTeirEnum].maxForce ;
        fish_vars.defaultTurnForce = chosen_fish_type.StatsList[fishRing.fishTeirEnum].defaultTurnForce;
        fish_vars.maxTurnForce = chosen_fish_type.StatsList[fishRing.fishTeirEnum].maxTurnForce;
        fish_vars.maxVelocity = chosen_fish_type.StatsList[fishRing.fishTeirEnum].maxVelocity;
        fish_vars.size = RandomnessExtension.RandomRangeWithNormalDistribution(chosen_fish_type.StatsList[fishRing.fishTeirEnum].sizeMin, chosen_fish_type.StatsList[fishRing.fishTeirEnum].sizeMax);
        fish_vars.fishTypeName = chosen_fish_type.fishTypeName;
        fish_vars.teir = fishRing.fishTeirEnum;
        fish_vars.depth = -fishSpawnDepth;
        fish_vars.spawnInDepth = spawnedInDepth;
        fish_vars.spawnedInTransitionTime = 2.0f;

        new_fish.fishObject.GetComponentInChildren<FishLogic>().Init(fish_vars);

        //if (fishRing.fishTeirEnum == FishType.FISH_TEIR.T2)
        //{
        //    new_fish.fishObject.GetComponentInChildren<MeshRenderer>().material = DebugMat;

        //}

        fishRing.fishList.Add(new_fish);


    }


    void OnDrawGizmos()
    {
        //Vector3 origin = transform.position;
        //origin.x = fishHabitatRingsOriginXZ.x;
        //origin.z = fishHabitatRingsOriginXZ.y;

        //List<Color> colours = new List<Color>();
        //colours.Add(new Color(0.0f, 0.0f, 1.0f));
        ////colours.Ad



        //colours.Add(new Color(0.0f, 1.0f, 1.0f));

        //colours.Add(new Color(1.0f, 1.0f, 0));
        //colours.Add(new Color(0.0f, 1.0f, 0));

        ////for (int i = 0; i < fishGenerationStats.Count; i++)
        ////{
        ////    UnityEditor.Handles.color = colours[i];
        ////    UnityEditor.Handles.DrawWireDisc(origin, Vector3.down, fishGenerationStats[i].habitatRingMin);
        ////    UnityEditor.Handles.DrawWireDisc(origin, Vector3.down, fishGenerationStats[i].habitatRingMax);
        ////}


        //Gizmos.color = Color.white;

        //for (int i = 0; i < fishGenerationStats.Count; i++)
        //{
        //    for (int k = 0; k < fishGenerationStats[i].spawnArea.Count; k++)
        //    {

        //        Transform trans = fishGenerationStats[i].spawnArea[k].transform;
        //        Matrix4x4 rotationMatrix = Matrix4x4.TRS(trans.position, trans.rotation, trans.lossyScale);
        //        Gizmos.matrix = rotationMatrix;

        //        Gizmos.DrawWireCube(transform.position, Vector3.one);
        //    }

        //}




    }
}
