using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{

    TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }


    [SerializeField] float timeOfSample = 1.0f;
    float timer = 0;
    // Update is called once per frame
    int sampled_fps = 0;

    float samples_total;
    int sampled_frames_count = 0;
    float highest_frame_time = 0;
    float lowest_frame_time = float.MaxValue;
    void Update()
    {
        timer -= Time.unscaledDeltaTime;
        samples_total += Time.unscaledDeltaTime;
        sampled_frames_count++;

        lowest_frame_time = Mathf.Min(lowest_frame_time, Time.unscaledDeltaTime);
        highest_frame_time = Mathf.Max(highest_frame_time, Time.unscaledDeltaTime);
        if (timer <= 0)
        {
            timer = timeOfSample;
            sampled_fps = (int)(1.0f / (samples_total / ((float)sampled_frames_count)));
            text.text = "Average FPS: " + sampled_fps.ToString() + System.Environment.NewLine +
                "Highest FPS:" + (1.0f / lowest_frame_time).ToString("#") + System.Environment.NewLine + 
                "Lowest FPS:" + (1.0f / highest_frame_time).ToString("#");

             highest_frame_time = 0;
             lowest_frame_time = float.MaxValue;
            samples_total = 0;
            sampled_frames_count = 0 ;
        }
    }
}
