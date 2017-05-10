using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FizzyoBreath : MonoBehaviour
{
    BreathRecogniser breathRecogniser;

    public float FizzyoPressure;
    public float breathVolume;

    public Image OuterBar;
    public float OuterBarFill = 0f;
    public Image InnerBar;
    public float InnerBarFill = 0f;

    public float maxPressure = 0.4f;
    public float maxBreathLength = 3f;

    // Use this for initialization
    void Start()
    {
        breathRecogniser = new BreathRecogniser(maxPressure, maxBreathLength);
        breathRecogniser.ExhalationComplete += BreathAnalyser_ExhalationComplete;
    }

    private void BreathAnalyser_ExhalationComplete(object sender, ExhalationCompleteEventArgs e)
    {
        if (e.IsBreathGood)
        {
            ScoreManager.Instance.GoodBreathAnimation();
        }
        else
        {
            ScoreManager.Instance.BadBreathAnimation();
        }
    }


    // Update is called once per frame
    void Update()
    {
        breathRecogniser.AddSample(Time.deltaTime, FizzyoDevice.Instance().Pressure());

        FizzyoPressure = FizzyoDevice.Instance().Pressure();

        breathVolume = breathRecogniser.ExhaledVolume;

        // Set Visuals
        OuterBarFill = breathRecogniser.Breathlength / breathRecogniser.MaxBreathLength;
        OuterBar.fillAmount = OuterBarFill;

        InnerBarFill = (float)ScoreManager.Instance.CurrentLevel.GoodBreathCount / (float)ScoreManager.Instance.CurrentLevel.GoodBreathMax;
        InnerBar.fillAmount = InnerBarFill;
    }
}
