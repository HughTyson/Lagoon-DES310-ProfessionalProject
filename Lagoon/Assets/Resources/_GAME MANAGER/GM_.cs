﻿using System.Collections;
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

    [SerializeField] TutorialManager tutorialManager;

    static GM_ instance_ = null;
    static bool destroyed_ = false;
    Members members = null;

    static public Members Instance
    {
        get
        {
            if (destroyed_)
                return null;
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
        public TimeMovement day_night_cycle;
        public Inventory inventory;
        public AudioManager audio;
        public PlayerSettings settings;
        public CustomSceneManager scene_manager;
        public TutorialManager tutorial_manger;
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
            members.story = new StoryManager((convoGraph.Root),convoGraph); // should be barrier node.);
            members.story_objective = new StoryObjectiveHandler();
            members.settings = new PlayerSettings();
            members.inventory = new Inventory();
            members.audio = audioManager;
            members.tutorial_manger = new TutorialManager();


            members.tween_curveLibrary_Hugh = HughCurveLibrary;
            members.tween_curveLibrary_Tomas = TomasCurveLibrary;

            members.day_night_cycle = DayNightValues;
            members.day_night_cycle.SetBaseTime(1.0f);

            members.stats.ResetStats();
        }
        else
        {
            Destroy(gameObject);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        members.story.Init();
        
    }


    void Update() // the execution order of this is set to first so it will call before any other game objects
    {
        members.tween_manager.Update();
        members.input.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour
        members.pause.Update(); // called in late update so it isn't called inbetween objects, potentially causing weird behaviour

    }

    private void OnDestroy()
    {

        if (instance_ == this)
        {
            members.input.SetVibrationBoth(0, 0); // prevents controller vibrating even if Unity game closes
            members.input.Update();

            for (int i = instance_.gameObject.transform.childCount - 1 ; i >= 0; i--)
                Destroy(instance_.gameObject.transform.GetChild(i).gameObject);
            instance_ = null;
            destroyed_ = true;
        }
    }
}



