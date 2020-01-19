using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisions : MonoBehaviour
{

    [SerializeField] float minDistance = 1.0f;
    [SerializeField] float maxDistance = 4.0f;
    [SerializeField] float smooth = 10.0f;

    Vector3 dollyDir;
    [SerializeField] Vector3 dollyDirAdjusted;
    [SerializeField] float distance;





    // Start is called before the first frame update
    void OnEnable()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 desiredCameraPOs = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position,desiredCameraPOs, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.1f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

    }
}
