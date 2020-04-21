using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Update is called once per frame

    [SerializeField] SelectableButton_ button1;
    [SerializeField] SelectableButton_ button2;
    [SerializeField] SelectableButton_ button3;
    [SerializeField] Slider_ slider;
    [SerializeField] Checkbox_ checkbox;
    [SerializeField] SelectableButton_ button4;
    [SerializeField] MenuItem_ test;
    [SerializeField] SelectableAndUnhoverableButton unhoverableButton;
    private void Start()
    {
    }
    void Update()
    {

        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.LB))
        {
            button1.Show();
            button2.Show();
            button3.Show();
            slider.Show();
            checkbox.Show();
            button4.Show();
            test.Show();
            unhoverableButton.Show();
        }
        else if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.RB))
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            slider.Hide();
            checkbox.Hide();
            button4.Hide();
            test.Hide();
            unhoverableButton.Hide();
        }

        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.Y))
        {
            button4.HoverOver();
        }


        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.X))
        {
            unhoverableButton.ListenForSelection();
        }
    }
}
