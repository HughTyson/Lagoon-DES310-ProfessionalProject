using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFishing : MonoBehaviour
{

    enum FISHING_STATE
    { 
    NOT_FISHING,
    CASTING,
    CASTED,
    REELING
    };



    // Start is called before the first frame update
    [SerializeField] GameObject prefabFishingBob;
    [SerializeField] Text textFishingCastPower;
    [SerializeField] Text textFishingFishCaught;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float fishingCastPowerMin = 1;
    [SerializeField] float fishingCastPowerMax = 10;
    [SerializeField] float fishingCastTimeToMax = 1;
    
    float currentCastTimeToMax = 0;

    Transform playerTransform;


    private FISHING_STATE fishing_state;
    private GameObject istantanceFishingBob;

    

    void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
        fishing_state = FISHING_STATE.NOT_FISHING;
    }

    // Update is called once per frame
    void Update()
    {
        switch (fishing_state)
        {
            case FISHING_STATE.NOT_FISHING:
                {

                    if (Input.GetButtonDown("PlayerRB"))
                    {
                        BeginCast();
                    }
                    break;
                }
            case FISHING_STATE.CASTING:
                {
                    currentCastTimeToMax += Time.deltaTime;
                    if (!Input.GetButton("PlayerRB"))
                    {
                        EndCast();
                    }
                    break;
                }
            case FISHING_STATE.CASTED:
                {
                    if (Input.GetButtonDown("PlayerRB"))
                    {
                        ReelIn();
                    }


                    break;
                }
        }

        textFishingCastPower.text = "Fishing Cast Power: " + ((int)(Mathf.PingPong(currentCastTimeToMax / fishingCastTimeToMax, 1) * 100)) + "%";
    }


    void ReelIn()
    {
        if (istantanceFishingBob != null)
        {
            Destroy(istantanceFishingBob);
        }
        fishing_state = FISHING_STATE.NOT_FISHING;
        if (istantanceFishingBob.GetComponent<FishingBobLogic>().WasFishCaught())
        {
            textFishingFishCaught.text = "Fish Caught";
            textFishingFishCaught.color = Color.green;
        }
        else
        {
            textFishingFishCaught.text = "Fish Not Caught";
            textFishingFishCaught.color = Color.red;
        }

    }

    void BeginCast()
    {
        if (istantanceFishingBob != null)
        {
            Destroy(istantanceFishingBob);
        }
        currentCastTimeToMax = 0;
        fishing_state = FISHING_STATE.CASTING;

        textFishingFishCaught.text = "";
    }

    void EndCast()
    {
        Vector3 cast_direction = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        cast_direction.y = 0.75f;
        cast_direction = cast_direction.normalized;

        fishing_state = FISHING_STATE.CASTING;
        istantanceFishingBob = Instantiate(prefabFishingBob);
        Physics.IgnoreCollision(GetComponent<Collider>(), istantanceFishingBob.GetComponent<Collider>());
        istantanceFishingBob.transform.position = transform.position;
        istantanceFishingBob.GetComponent<Rigidbody>().AddForce(cast_direction * Mathf.Lerp(fishingCastPowerMin,fishingCastPowerMax, Mathf.PingPong(currentCastTimeToMax / fishingCastTimeToMax, 1)), ForceMode.Impulse);

        fishing_state = FISHING_STATE.CASTED;
    }
}
