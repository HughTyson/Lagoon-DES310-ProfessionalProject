using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerScript : MonoBehaviour
{


    [SerializeField] GameObject prefabObjectToPool;
    [SerializeField] int listSize;
    [SerializeField] bool canExpand;

    List<GameObject> unactiveObjects = new List<GameObject>();
    List<GameObject> activeObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < listSize; i++)
        {
            unactiveObjects.Add(Instantiate(prefabObjectToPool));
        }
    }

    public GameObject ActivateObject()
    {
        if (unactiveObjects.Count == 0)
        {
            if (canExpand)
            {
                GameObject objectPointer = Instantiate(prefabObjectToPool);
                activeObjects.Add(objectPointer);
                return objectPointer;
            }
            else
            {
                return null;
            }
        }
        else
        {
            GameObject objectPointer = unactiveObjects[0];
            objectPointer.SetActive(true);

            activeObjects.Add(objectPointer);
            unactiveObjects.Remove(objectPointer);

            return objectPointer;
        }
    }

    public bool DeactivateObject(GameObject ObjectToDeactivate)
    {
        ObjectToDeactivate.SetActive(false);
        unactiveObjects.Add(ObjectToDeactivate);
        return activeObjects.Remove(ObjectToDeactivate);
    }

    

}
