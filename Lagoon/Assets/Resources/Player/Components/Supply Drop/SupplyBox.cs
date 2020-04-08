using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System.Linq;

public class SupplyBox : MonoBehaviour
{

    public enum STATE
    {
        DROPPING,
        IN_WATER,
        CAUGHT
    }

    // ==========================================
    //              Visible Variables
    //===========================================

    [SerializeField] public STATE box_state;




    // ==========================================
    //              Hidden Variables
    //===========================================


    [HideInInspector] public List<InventoryItem> stored_items = new List<InventoryItem>();

    private Rigidbody body;
    bool all_essentials_packed;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();


        //might need to put this in a function that's called if there is a need for a reference to objects needed


    }

    public void Fill(params System.Type[] required_items)
    {


        int amount = Random.Range(required_items.Length + 1, 6);

        if(required_items.Length > 0)
        {
            all_essentials_packed = false;
        } 
        else if(required_items.Length == 0)
        {
            all_essentials_packed = true;
        }

        for(int i = 0; i < amount; i++)
        {

            if(!all_essentials_packed)
            {
                Generate(required_items[i]);

                if(i == required_items.Length - 1)
                {
                    all_essentials_packed = true;
                }
            }
            else if(all_essentials_packed)
            {
                Generate(GM_.Instance.inventory.GetRandomType());
            }

            //if there is a need for the item because of the story then get a specific item and add it to the stored items

            //if there is no specific item needed then
        }

    }

    void Generate(System.Type type)
    {
           InventoryItem instance = (InventoryItem)System.Activator.CreateInstance(type);

            instance.Init();

            stored_items.Add(instance);
    }

    // Update is called once per frame
    void Update()
    {

        switch (box_state)
        {
            case STATE.DROPPING:
                {
                    //potentially use this state for animation of dropping
                }
                break;
            case STATE.IN_WATER:
                {
                    //if in the water then give off the emote of where the box is
                }
                break;
            case STATE.CAUGHT:
                {
                    //might need this state? might not
                }
                break;
            default:
                break;
        }


    }
    

    // adds all items in the box into the inventory
    public void GetBoxContents()
    {

        for (int i = 0; i < stored_items.Count; i++)
        {
            GM_.Instance.inventory.AddNewItem(stored_items[i]);
        }

    }
}
