using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GM_ : MonoBehaviour
{
    [SerializeField] ConvoGraph convoGraph;

    [SerializeField] AudioManager audioManager;

    [SerializeField]
    TweenCurveLibrary TomasCurveLibrary;
    [SerializeField]
    TweenCurveLibrary HughCurveLibrary;

    [SerializeField] TimeMovement DayNightValues;
    [SerializeField] CustomSceneManager sceneManager;

    static GM_ instance_ = null;

    Members members = null;
    static public Members Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<GM_>();
                if (instance_ == null)
                {
                    GameObject test = Instantiate(Resources.Load<GameObject>("_GAME MANAGER/GAME_MANAGER"));
                    test.name = "GAME MANAGER";
                    instance_ = test.GetComponent<GM_>();                   
                }
            }
            return instance_.members; // returns an interface to the GM_ to hide MonoBehavour methods
        }
    }

    // Creates an interface to the GM_ to hide MonoBehavour methods
    public class Members
    {
        public InputManager input;
        public PauseManager pause;
        public StatsManager stats;
        public StoryManager story;
        public StoryObjectiveHandler story_objective; 
        public TweenManager tween_manager;
        public UpdateEventSystem update_events;
        public TweenCurveLibrary tween_curveLibrary_Tomas;
        public TweenCurveLibrary tween_curveLibrary_Hugh;
        public TimeMovement DayNightCycle;
        public Inventory inventory;
        public AudioManager audio;
        public PlayerSettings settings;
        public CustomSceneManager scene_manager;
    };

    private void Awake()
    {
        if (instance_ == null)
        {
            DontDestroyOnLoad(gameObject);

            if (instance_ != null && instance_ != this)
            {
                Debug.LogError("Error, multiple GAME MANAGERS!");
                Debug.Break();
            }

            instance_ = this;

            members = new Members();
            members.scene_manager = sceneManager;
            members.update_events = gameObject.AddComponent<UpdateEventSystem>();
            members.tween_manager = new TweenManager();
            members.input = new InputManager();
            members.pause = new PauseManager();
            members.stats = new StatsManager();
            members.story = new StoryManager((convoGraph.Root)); // should be barrier node.);
            members.story_objective = new StoryObjectiveHandler();
            members.settings = new PlayerSettings();
            members.inventory = new Inventory();
            members.audio = audioManager;

            members.tween_curveLibrary_Hugh = HughCurveLibrary;
            members.tween_curveLibrary_Tomas = TomasCurveLibrary;

            members.DayNightCycle = DayNightValues;
            members.DayNightCycle.SetBaseTime(1.0f);
        }
        else
        {
            Destroy(gameObject);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        members.story.Begin();
    }


    void Update() // the execution order of this is set to first so it will call before any other game objects
    {
        members.tween_manager.Update();
        members.input.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
        members.pause.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour

    }

    private void OnDestroy()
    {
        if (members != null)
        {
            members.input.SetVibrationBoth(0, 0); // prevents controller vibrating even if Unity game closes
            members.input.Update();
        }
        if (instance_ == this)
            instance_ = null;
    }
}



