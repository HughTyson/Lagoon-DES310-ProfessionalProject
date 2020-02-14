using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{

    [SerializeField] SupplyBox box_prefab;
    

    [SerializeField] public List<SupplyBox> box_list = new List<SupplyBox>();

    [SerializeField] List<GameObject> drop_points;

    public void Spawn()
    {
        StartCoroutine(SpawnBoxes());
        Debug.Log("HELLO");
    }
    
    //public void SpawnBoxes()
    //{

    //   // int random_point = Random.Range(0, drop_points.Count);      //get the random point it will be dropped at

    //   // SupplyBox new_box = new SupplyBox();

    //   // new_box = Instantiate(box_prefab);
    //   // new_box.box_state = SupplyBox.STATE.DROPPING;

    //   // Spawn(new_box);

    //}

    public void DestroyBox()
    {
        for(int i = 0; i < box_list.Count; i++)
        {
            Destroy(box_list[i].gameObject);
            box_list.Remove(box_list[i]);
        }
    }

    public IEnumerator SpawnBoxes()
    {

        Debug.Log("HELLO2");

        for (int i = 0; i < drop_points.Count; i++)
        {
            Debug.Log("HELLO3");

            SupplyBox new_box = new SupplyBox();

            new_box = Instantiate(box_prefab);
            new_box.box_state = SupplyBox.STATE.DROPPING;

            Vector3 spawn_pos = drop_points[i].transform.position;
            new_box.transform.position = spawn_pos;

            yield return new WaitForSeconds(0.7f);

        }
    }

}
