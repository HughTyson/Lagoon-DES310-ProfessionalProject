using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEventSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public event System.Action UpdateEvent;
    public event System.Action FixedUpdateEvent;
    public event System.Action LateUpdateEvent;


    private void Update()
    {
        UpdateEvent?.Invoke();
    }
    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }
    private void LateUpdate()
    {
        LateUpdateEvent?.Invoke();
    }
}
