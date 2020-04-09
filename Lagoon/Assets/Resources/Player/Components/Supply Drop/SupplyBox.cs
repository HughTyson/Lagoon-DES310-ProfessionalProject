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

    [SerializeField] Transform look_at;


    // ==========================================
    //              Hidden Variables
    //===========================================


    [HideInInspector] public List<InventoryItem> stored_items = new List<InventoryItem>();

    [SerializeField] FishingUI ui_logic;

    private Rigidbody body;
    bool all_essentials_packed;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();


        ui_logic.gameObject.SetActive(true);

        //might need to put this in a function that's called if there is a need for a reference to objects needed

    }

    public void SetTransform()
    {
        ui_logic.AttachLookAtTransform(look_at);
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
                    
                    if(!ui_logic.fishBiteIndicatorAnimation.IsPlaying)
                    {
                        
                        ui_logic.SetPosition(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
                        ui_logic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_BITE);
                    }


                    
                    //if in the water then give off the emote of where the box is
                }
                break;
            case STATE.CAUGHT:
                {
                    //might need this state? might not

                    if (ui_logic != null)
                        ui_logic.gameObject.SetActive(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
        {
            box_state = STATE.IN_WATER;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.FISHING_BOB))
        {
            box_state = STATE.CAUGHT;
        }
    }
}
