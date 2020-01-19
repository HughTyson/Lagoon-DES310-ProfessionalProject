using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] Transform cameraTransform;

    [SerializeField] float fishingCastPowerMin;
    [SerializeField] float fishingCastPowerMax;

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
        if (Input.GetButtonDown("PlayerRB"))
        {
            Cast();
        }
    }


    void Cast()
    {
        if (istantanceFishingBob != null)
        {
            Destroy(istantanceFishingBob);
        }
        Vector3 cast_direction = new Vector3(cameraTransform.forward.x, -cameraTransform.forward.y, cameraTransform.forward.z);
        fishing_state = FISHING_STATE.CASTING;
        istantanceFishingBob = Instantiate(prefabFishingBob);
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), istantanceFishingBob.GetComponent<SphereCollider>());
        istantanceFishingBob.transform.position = transform.position;
        istantanceFishingBob.GetComponent<Rigidbody>().AddForce(cast_direction * fishingCastPowerMax,ForceMode.Impulse);
    }
    //void BeginCast()
    //{

    //}

    //void EndCast()
    //{

    //}
}
