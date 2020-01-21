using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{

    [SerializeField] GameObject prefabFish;
    [SerializeField] int maxFish = 5;
    [SerializeField] float spawnRadius;
    [SerializeField] Vector3 spawnCentre3d;

    List<GameObject> fishList = new List<GameObject>();

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
            Vector2 distanceVec = new Vector2(fishList[i].transform.position.x - spawnCentre3d.x, fishList[i].transform.position.z - spawnCentre3d.z);
            if (distanceVec.magnitude > spawnRadius)
            {
                Destroy(fishList[i]);
                fishList.RemoveAt(i);
            }
        }
    }

    void SpawnFish()
    {
        GameObject fish = Instantiate(prefabFish);
        Vector3 new_position = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized * spawnRadius;
        new_position.y = spawnCentre3d.y;
        fish.transform.position = new_position;
        fish.GetComponent<Rigidbody>().velocity = (spawnCentre3d - fish.transform.position).normalized; // make fish face the centre of the spawn circle
        fishList.Add(fish);
    }
}
