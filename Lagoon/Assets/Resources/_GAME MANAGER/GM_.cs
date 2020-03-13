using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GM_ : MonoBehaviour
{
    [SerializeField] ConvoGraph convoGraph;
    [SerializeField] UIManager uiManager;

    static GM_ instance_ = null;

    static Members members = null;
    public static Members Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<GM_>();
                if (instance_ == null && Application.isPlaying)
                {
                    GameObject test = Instantiate(Resources.Load<GameObject>("_GAME MANAGER/GAME_MANAGER"));
                    test.name = "GAME MANAGER";
                    instance_ = test.GetComponent<GM_>();                   
                }
            }
            return members; // returns an interface to the GM_ to hide MonoBehavour methods
        }
    }

    // Creates an interface to the GM_ to hide MonoBehavour methods
    public class Members
    {
        public InputManager input;
        public PauseManager pause;
        public StatsManager stats;
        public StoryManager story;
        public StoryObjectiveHandler story_objective; // might not need to be here
        public StoryEventHandler story_events;
        public UIManager ui;
        public TweenManager tween_manager;
    };

    private void Awake()
    {
        if (instance_ != null && instance_ != this)
        {
            Debug.LogError("Error, multiple GAME MANAGERS!");
            Debug.Break();
        }

        instance_ = this;
        members = new Members();
        members.tween_manager = new TweenManager();
        members.input = new InputManager();
        members.pause = new PauseManager();
        members.stats = new StatsManager();
        members.story = new StoryManager(((RootNode)convoGraph.FindRootNode()).NextNode()); // should be barrier node.);
        members.story_objective = new StoryObjectiveHandler();
        members.story_events = new StoryEventHandler();
        members.ui = uiManager;

    }


    // Start is called before the first frame update
    void Start()
    {
        members.story.Begin();
    }


    private void FixedUpdate()
    {
        members.input.FixedUpdate();
    }

    void Update() // the execution order of this is set to first so it will call before any other game objects
    {
        members.tween_manager.Update();
        members.input.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
        members.ui.ManagerUpdate();
        members.story.Update();
        members.pause.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
    }

    private void OnDestroy()
    {
        members.input.SetVibrationBoth(0, 0); // prevents controller vibrating even if Unity game closes
        members.input.FixedUpdate();
        Destroy(instance_);
    }
}



