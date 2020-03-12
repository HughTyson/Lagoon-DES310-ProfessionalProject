using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_ : MonoBehaviour
{
    [SerializeField] ConvoGraph convoGraph;
    [SerializeField] GameObject selfPrefab;
    public static GM_ instance_ = null;

    public static GM_ Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<GM_>();
                if (instance_ == null && Application.isPlaying)
                {
                    Object test = Resources.Load("GAME_MANAGER");
                    instance_ = Instantiate((GameObject)test).GetComponent<GM_>();
                }
            }
            return instance_;
        }
    }

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
        instance_ = this;
        input = new InputManager();
        pause = new PauseManager();
        stats = new StatsManager();
        story = new StoryManager(((RootNode)convoGraph.FindRootNode()).NextNode()); // should be barrier node.);
        story_objective = new StoryObjectiveHandler();
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
