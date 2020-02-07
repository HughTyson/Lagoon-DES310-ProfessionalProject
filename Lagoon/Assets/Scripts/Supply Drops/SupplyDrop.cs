using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{

    [SerializeField] SupplyBox box_;
    SupplyBox[] box = new SupplyBox[1]; 

    [SerializeField] List<GameObject> drop_points;
    


    private void OnEnable()
    {
        int random_point = Random.Range(0, drop_points.Count);      //get the random point it will be dropped at

        Vector3 drop_position = drop_points[random_point].transform.position;


        for(int i = 0; i < box.Length; i++)
        {
            box[i] = Instantiate(box_);
            box[i].transform.position = new Vector3(drop_position.x, drop_position.y + 20, drop_position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
