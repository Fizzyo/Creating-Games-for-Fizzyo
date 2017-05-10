﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartCalibrate : MonoBehaviour {
    //public 
    public Text startText;
    //public GameObject pressureBar;
    public Slider slider;

    //private
    private float maxPressureReading = 0;
    private float minPressureThreshold = 0.1f;

    private System.Diagnostics.Stopwatch blowingStopwatch;
    private int countdownToStart = 3;
    private float smoothing = 0.00001f;

    // Use this for initialization
    void Start () {
        // Create new stopwatch.
        blowingStopwatch = new System.Diagnostics.Stopwatch();
        startText.text = countdownToStart.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        float pressure = FizzyoDevice.Instance().Pressure();
        //float y = transform.position.y + ((pressure * 5) - pressureBar.transform.position.y) * smoothing;
        //pressureBar.transform.position = new Vector3(pressureBar.transform.position.x,y , pressureBar.transform.position.z);
        slider.value = (pressure * 5) * smoothing;

        if (pressure > minPressureThreshold )
        {
            maxPressureReading = pressure;
            blowingStopwatch.Start();
            int timeToStart = (int)(countdownToStart - (blowingStopwatch.ElapsedMilliseconds / 1000));

            if (timeToStart > 0)
            {
                startText.text = "" + timeToStart;
            }else{
                //Save the max recorded pressure to use to scale sensor input during gameplay.
                PlayerPrefs.SetFloat("Max Fizzyo Pressure", maxPressureReading);
                SceneManager.LoadScene("JetpackLevel");
            }
        }
        else
        {
            blowingStopwatch.Stop();
        }
       
    }
}
