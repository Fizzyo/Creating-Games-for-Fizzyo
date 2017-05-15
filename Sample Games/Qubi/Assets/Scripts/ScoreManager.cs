using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public enum GameStage { SessionSetup, LevelPlaying, LevelEnd, GameEnd }
    public GameStage currentStage = GameStage.SessionSetup;

    // player prefs to load
    public int SessionBreathCount = 8;
    public int SessionSetCount = 3;
    public int CoinHighScore = 0;

    // Levels
    public int CurrentLevelIndex;
    public PlatformLevel CurrentLevel;
    public List<PlatformLevel> Levels;

    // UI
    public GameObject HUD;
    public GameObject LevelSetupUI;
    public GameObject LevelEndUI;
    public GameObject GameEndUI;

    // In-Game objects
    public GameObject Player;
    public GameObject LevelEndPrefab;
    private GameObject levelEnd;

    // Particle Effects
    public GameObject GoodBreathParticles;
    public GameObject BadBreathParticles;

    // Audio
    public AudioSource GoodBreathSound;
    public AudioSource BadBreathSound;
    public AudioSource LevelEndSound;
    public AudioSource GameEndSound;
    public AudioSource CoinEffect;

    public AudioSource BackgroundMenuMusic;

    // Save keys
    private string sessionBreathCountKey = "BreathsCount";
    private string sessionSetCountKey = "SetsCount";
    private string coinHighScoreKey = "coinHighScore";

    // Events
    public delegate void LevelResetEventHandler();
    public event LevelResetEventHandler LevelStartEvent;
    public event LevelResetEventHandler LevelEndEvent;
    public event LevelResetEventHandler GameEndEvent;

    //In Game background music manager
    public BackgroundMusicManager backgroundMusicManager;

    // First thing to be called
    private void Awake()
    {
        Instance = this;
    }

    // Called at the start
    private void Start()
    {
        LoadPlayerPrefs();

        LevelSetupUI.SetActive(true);
        HUD.SetActive(false);
        LevelEndUI.SetActive(false);
        GameEndUI.SetActive(false);
        StartCoroutine(AudioFader.FadeIn(BackgroundMenuMusic, 5f));
    }

    #region SaveLoad
    // Loads the player prefs
    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(sessionBreathCountKey))
            SessionBreathCount = PlayerPrefs.GetInt(sessionBreathCountKey);

        if (PlayerPrefs.HasKey(sessionSetCountKey))
            SessionSetCount = PlayerPrefs.GetInt(sessionSetCountKey);

        if (PlayerPrefs.HasKey(coinHighScoreKey))
            CoinHighScore = PlayerPrefs.GetInt(coinHighScoreKey);
    }

    // Saves the player prefs
    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(sessionBreathCountKey, SessionBreathCount);
        PlayerPrefs.SetInt(sessionSetCountKey, SessionSetCount);
        PlayerPrefs.Save();
    }

    // If it's a new high score, save it
    public void CheckHighScore()
    {
        if (TotalCoins() > CoinHighScore)
        {
            CoinHighScore = TotalCoins();
            PlayerPrefs.SetInt(coinHighScoreKey, CoinHighScore);
            PlayerPrefs.Save();
        }
    }
    #endregion

    // Called once per frame
    private void Update()
    {
        if (currentStage == GameStage.LevelPlaying)
        {
            CurrentLevel.LevelTime += Time.deltaTime;

            if (CurrentLevel.GoodBreathCount >= CurrentLevel.GoodBreathMax && levelEnd == null)
            {
                CreateLevelEnd();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonPressed();
        }
    }

    #region Breaths

    public void GoodBreathAnimation()
    {
        GoodBreathSound.Stop();
        GoodBreathSound.Play();

        CurrentLevel.GoodBreathCount++;

        if (CurrentLevel.GoodBreathCount % 2 == 0)
        {
            backgroundMusicManager.PlayNextLevel(CurrentLevel.GoodBreathCount / 2);
        }

        GameObject particles = Instantiate(GoodBreathParticles);
        particles.transform.position = Player.transform.GetChild(0).position;
        particles.transform.parent = Player.transform;
        Destroy(particles, 2f);
    }

    public void BadBreathAnimation()
    {
        BadBreathSound.Stop();
        BadBreathSound.Play();

        CurrentLevel.BadBreathCount++;

        GameObject particles = Instantiate(BadBreathParticles);
        particles.transform.position = Player.transform.GetChild(0).position;
        particles.transform.parent = Player.transform;
        Destroy(particles, 2f);
    }

    #endregion

    #region LevelFunctions

    // handle button presses depending on current game stage
    public void ButtonPressed()
    {
        switch (currentStage)
        {
            case GameStage.SessionSetup:
                SavePlayerPrefs();
                CreateLevels();
                StartNewLevel();
                break;

            case GameStage.LevelPlaying:

                break;

            case GameStage.LevelEnd:
                IncrementLevel();
                StartNewLevel();
                break;

            case GameStage.GameEnd:
                NewSession();
                break;
        }
    }

    // Called when a coin is touched
    public void GetCoin()
    {
        CurrentLevel.CoinCount++;
        CoinEffect.Stop();
        CoinEffect.Play();
    }

    // shows the new session ui
    public void NewSession()
    {
        currentStage = GameStage.SessionSetup;

        LevelSetupUI.SetActive(true);
        HUD.SetActive(false);
        LevelEndUI.SetActive(false);
        GameEndUI.SetActive(false);
    }

    // Creates the levels at the start of the game, after setting up number of breaths and sets/levels
    public void CreateLevels()
    {
        Levels = new List<PlatformLevel>();
        Levels.Clear();

        for (int i = 0; i < SessionSetCount; i++)
        {
            PlatformLevel newLevel = new PlatformLevel();
            newLevel.GoodBreathMax = SessionBreathCount;
            Levels.Add(newLevel);
        }

        CurrentLevelIndex = 0;
        CurrentLevel = Levels[CurrentLevelIndex];
    }

    // Increments the level
    public void IncrementLevel()
    {
        CurrentLevelIndex++;
        CurrentLevel = Levels[CurrentLevelIndex];
    }

    // Begin playing the current Level
    public void StartNewLevel()
    {
        currentStage = GameStage.LevelPlaying;

        if (BackgroundMenuMusic.isPlaying)
        {
            StartCoroutine(AudioFader.FadeOut(BackgroundMenuMusic, 0.5f));
        }
        backgroundMusicManager.StartBackgroundMusic();

        if (LevelStartEvent != null)
            LevelStartEvent();

        LevelSetupUI.SetActive(false);
        HUD.SetActive(true);
        LevelEndUI.SetActive(false);

        if (levelEnd != null)
        {
            Destroy(levelEnd);
        }

        Player.transform.position = Vector3.zero;
        GameEndSound.Stop();
    }

    // Shows the end of level scores and stops the game
    public void EndLevel()
    {
        if (CurrentLevelIndex == Levels.Count - 1)
        {
            EndGame();
        }
        else
        {
            currentStage = GameStage.LevelEnd;

            if (LevelEndEvent != null)
                LevelEndEvent();

            LevelSetupUI.SetActive(false);
            HUD.SetActive(false);
            LevelEndUI.SetActive(true);
            GameEndUI.SetActive(false);

            LevelEndSound.Stop();
            LevelEndSound.Play();
        }
    }

    public void EndGame()
    {
        currentStage = GameStage.GameEnd;

        CheckHighScore();

        if (GameEndEvent != null)
            GameEndEvent();

        LevelSetupUI.SetActive(false);
        HUD.SetActive(false);
        LevelEndUI.SetActive(false);
        GameEndUI.SetActive(true);

        GameEndSound.Stop();
        GameEndSound.Play();

        backgroundMusicManager.StopBackgroundMusic();
    }

    public void CreateLevelEnd()
    {
        levelEnd = Instantiate(LevelEndPrefab);
        levelEnd.transform.position = Player.transform.position + Vector3.right * 30f;
    }
    #endregion

    public int TotalCoins()
    {
        int newCount = 0;

        foreach (PlatformLevel level in ScoreManager.Instance.Levels)
        {
            newCount += level.CoinCount;
        }

        return newCount;
    }

    public float LevelTimeTotal()
    {
        float newTimeTotal = 0f;

        foreach (PlatformLevel level in ScoreManager.Instance.Levels)
        {
            newTimeTotal += level.LevelTime;
        }

        return newTimeTotal;
    }

    public int TotalGoodBreathCount()
    {
        int newCount = 0;

        foreach (PlatformLevel level in ScoreManager.Instance.Levels)
        {
            newCount += level.GoodBreathCount;
        }

        return newCount;
    }

    public int TotalBadBreathCount()
    {
        int newCount = 0;

        foreach (PlatformLevel level in ScoreManager.Instance.Levels)
        {
            newCount += level.BadBreathCount;
        }

        return newCount;
    }
}

[System.Serializable]
public class PlatformLevel
{
    public int GoodBreathMax = 8;
    public int GoodBreathCount = 0;
    public int BadBreathCount = 0;

    public int CoinCount = 0;
    public float LevelTime = 0f;

    public float difficulty = 1f;

    public float MinPlayerSpeed = 8f;
    public float MaxPlayerSpeed = 16f;

    public PlatformLevel()
    {

    }
}
