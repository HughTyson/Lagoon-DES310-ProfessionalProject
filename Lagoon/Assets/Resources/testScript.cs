using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Update is called once per frame

    [SerializeField] SelectableButton_TextButton button1;
    [SerializeField] SelectableButton_TextButton button2;
    [SerializeField] SelectableButton_TextButton button3;
    [SerializeField] Slider_Default slider;

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
        }
        else if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.RB))
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            slider.Hide();
        }

        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.Y))
        {
            button1.HoverOver();
        }
    }
}
