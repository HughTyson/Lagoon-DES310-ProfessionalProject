using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_ : MonoBehaviour
{
    [SerializeField] ConvoGraph convoGraph;
    public static GM_ instance = null;

    public InputManager input;
    public PauseManager pause;
    public StatsManager stats;
    public StoryManager story;
    public StoryObjectiveHandler story_objective;


    // [Header("Self Pointers")]
    //  [SerializeField] UIManager ui;
    public UIManager ui;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            input = new InputManager();
            pause = new PauseManager();
            stats = new StatsManager();
            story = new StoryManager(((RootNode)convoGraph.FindRootNode()).NextNode()); // should be barrier node.);
            story_objective = new StoryObjectiveHandler();
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        story.Begin();
    }


    private void FixedUpdate()
    {
        input.FixedUpdate();
    }

    void Update() // the execution order of this is set to first so it will call before any other game objects
    {
        input.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
        ui.ManagerUpdate();
        story.Update();
        pause.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
    }

    private void OnDestroy()
    {
        input.SetVibrationBoth(0, 0); // prevents controller vibrating even if Unity game closes
        input.FixedUpdate();
    }
}
