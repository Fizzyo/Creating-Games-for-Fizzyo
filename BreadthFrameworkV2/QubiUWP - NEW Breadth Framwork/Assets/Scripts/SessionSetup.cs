using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionSetup : MonoBehaviour
{
    public Text BreathDisplay;
    public Text SetsDisplay;

    private void Start()
    {
        BreathDisplay.text = ScoreManager.Instance.SessionBreathCount.ToString();
        SetsDisplay.text = ScoreManager.Instance.SessionSetCount.ToString();
    }

    private void Update()
    {
        BreathDisplay.text = ScoreManager.Instance.SessionBreathCount.ToString();
        SetsDisplay.text = ScoreManager.Instance.SessionSetCount.ToString();
    }

    public void IncrementBreathCount(int increment)
    {
        ScoreManager.Instance.SessionBreathCount += increment;
        ScoreManager.Instance.SessionBreathCount = Mathf.Clamp(ScoreManager.Instance.SessionBreathCount, 1, 90);
        BreathDisplay.text = ScoreManager.Instance.SessionBreathCount.ToString();
    }

    public void IncrementSetCount(int increment)
    {
        ScoreManager.Instance.SessionSetCount += increment;
        ScoreManager.Instance.SessionSetCount = Mathf.Clamp(ScoreManager.Instance.SessionSetCount, 1, 90);
        SetsDisplay.text = ScoreManager.Instance.SessionSetCount.ToString();
    }
}
