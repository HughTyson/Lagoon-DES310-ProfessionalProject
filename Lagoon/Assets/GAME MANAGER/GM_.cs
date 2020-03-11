using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_ : MonoBehaviour
{

    public static GM_ instance = null;

    public InputManager input = new InputManager();
    public PauseManager pause = new PauseManager();
    public StatsManager stats = new StatsManager();




    // [Header("Self Pointers")]
    //  [SerializeField] UIManager ui;
    public UIManager ui;
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


    private void FixedUpdate()
    {
        input.FixedUpdate();
    }

    void Update() // the execution order of this is set to first so it will call before any other game objects
    {
        input.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
        pause.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
        ui.ManagerUpdate();
    }

    private void OnDestroy()
    {
        input.SetVibrationBoth(0, 0); // prevents controller vibrating even if Unity game closes
        input.FixedUpdate();
    }
}
