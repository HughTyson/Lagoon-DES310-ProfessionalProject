using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_ : MonoBehaviour
{

    public static GM_ instance = null;

    public InputManager input = new InputManager();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        
    }


    private void FixedUpdate()
    {
        input.FixedUpdate();
    }

    void LateUpdate()
    {
        input.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
    }

    private void OnDestroy()
    {
        input.SetVibration(0, 0);
        input.FixedUpdate();
    }
}
