using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_MainMenu : MonoBehaviour
{

    [SerializeField] MenuSelectableBase startButton;
    [SerializeField] MenuSelectableBase optionsButton;
    [SerializeField] MenuSelectableBase creditsButton;
    [SerializeField] MenuSelectableBase exitButton;



    // Start is called before the first frame update
    void Start()
    {
        startButton.Event_Selected += start_transitionToGame;
        optionsButton.Event_Selected += start_transitionToOptions;
        creditsButton.Event_Selected += start_transitionToCredits;
        exitButton.Event_Selected += start_transitionToExit;

        startButton.HoveredOver();
    }


    void start_transitionToGame()
    {

    }
    void start_transitionToCredits()
    {

    }
    void start_transitionToExit()
    {

    }
    void start_transitionToOptions()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
