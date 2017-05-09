using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartCalibrate : MonoBehaviour
{
    //public 
    public Text StartText;
    public GameObject PressureBar;
    public ParticleSystem ParticleSystem;

    //private
    private float maxPressureReading = 0;
    private float minPressureThreshold = 0.1f;

    private System.Diagnostics.Stopwatch blowingStopwatch;
    private int countdownToStart = 3;
    private float smoothing = 0.05f;

    // Use this for initialization
    void Start()
    {
        // Create new stopwatch.
        blowingStopwatch = new System.Diagnostics.Stopwatch();
        StartText.text = "" + countdownToStart;
    }

    // Update is called once per frame
    void Update()
    {
        float pressure = Fizzyo.FizzyoDevice.Instance().Pressure();

        //animate breath particles
        ParticleSystem.startSpeed = pressure * 500;
        ParticleSystem.startLifetime = pressure * 1;

        //set pressure bar height
        float destHeight = -20 * pressure;
        float y = PressureBar.transform.localPosition.y + ((destHeight - PressureBar.transform.localPosition.y) * smoothing);
        PressureBar.transform.localPosition = new Vector3(PressureBar.transform.localPosition.x, y, PressureBar.transform.localPosition.z);


        if (pressure > minPressureThreshold)
        {
            maxPressureReading = pressure;
            blowingStopwatch.Start();
            int timeToStart = (int)(countdownToStart - (blowingStopwatch.ElapsedMilliseconds / 1000));

            if (timeToStart > 0)
            {
                StartText.text = "" + timeToStart;
            }
            else
            {
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
