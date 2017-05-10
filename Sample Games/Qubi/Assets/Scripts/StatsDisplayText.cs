using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This behaviour is used to display stats form the game on a Text component
public class StatsDisplayText : MonoBehaviour
{
    private Text textDisplay;
    string stringToDisplay;

    public enum GameStats
    {
        CoinCountCurrentRound,
        CoinCountTotal,
        CoinCountHighScore,
        LevelTime,
        LevelTimeTotal,
        LevelCurrent,
        LevelCount,
        SessionBreathCount,
        SessionSetCount,
        GoodBreathCountCurrentLevel,
        BadBreathCountCurrentLevel,
        GoodBreathCountTotal,
        BadBreathCountTotal
    }

    public GameStats StatsToDisplay = (GameStats)0;

    public string Suffix = "";

    private void Start()
    {
        textDisplay = this.GetComponent<Text>();
        stringToDisplay = " ";
    }

    // Update is called once per frame
    void Update()
    {
        switch (StatsToDisplay)
        {
            case GameStats.CoinCountCurrentRound:
                stringToDisplay = ScoreManager.Instance.CurrentLevel.CoinCount.ToString() + Suffix;
                break;

            case GameStats.CoinCountTotal:
                stringToDisplay = ScoreManager.Instance.TotalCoins().ToString() + Suffix;
                break;

            case GameStats.CoinCountHighScore:
                stringToDisplay = ScoreManager.Instance.CoinHighScore.ToString() + Suffix;
                break;

            case GameStats.LevelTime:
                stringToDisplay = ScoreManager.Instance.CurrentLevel.LevelTime.ToString("0.0") + Suffix;
                break;

            case GameStats.LevelTimeTotal:
                stringToDisplay = ScoreManager.Instance.LevelTimeTotal().ToString("0.0") + Suffix;
                break;

            case GameStats.LevelCurrent:
                stringToDisplay = (ScoreManager.Instance.CurrentLevelIndex + 1).ToString() + Suffix;
                break;

            case GameStats.LevelCount:
                stringToDisplay = ScoreManager.Instance.Levels.Count.ToString() + Suffix;
                break;

            case GameStats.SessionBreathCount:
                stringToDisplay = ScoreManager.Instance.SessionBreathCount.ToString() + Suffix;
                break;

            case GameStats.SessionSetCount:
                stringToDisplay = ScoreManager.Instance.SessionSetCount.ToString() + Suffix;
                break;

            case GameStats.GoodBreathCountCurrentLevel:
                stringToDisplay = ScoreManager.Instance.CurrentLevel.GoodBreathCount.ToString() + Suffix;
                break;

            case GameStats.BadBreathCountCurrentLevel:
                stringToDisplay = ScoreManager.Instance.CurrentLevel.BadBreathCount.ToString() + Suffix;
                break;

            case GameStats.GoodBreathCountTotal:
                stringToDisplay = ScoreManager.Instance.TotalGoodBreathCount().ToString() + Suffix;
                break;

            case GameStats.BadBreathCountTotal:
                stringToDisplay = ScoreManager.Instance.TotalBadBreathCount().ToString() + Suffix;
                break;
        }

        if (stringToDisplay != null)
            textDisplay.text = stringToDisplay;
    }
}
