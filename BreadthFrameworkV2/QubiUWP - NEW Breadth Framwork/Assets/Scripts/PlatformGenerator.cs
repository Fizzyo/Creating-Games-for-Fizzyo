using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public float LevelLength = 100f;
    public float PlatformWidth = 2f;
    public float GapWidth = 2f;
    public float HorizontalPosition = 0f;
    public float PerlinFrequency = 1f;
    public float MaxHeight;
    public float MinHeight;
    public int CoinsPerPlatform = 3;

    public GameObject PlatformPrefab;
    public GameObject CoinPrefab;

    public List<GameObject> Platforms;

    private GameObject player;

    private float PlatformAheadDistance = 30f;
    private float PlatformBehindDistance = 20f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ScoreManager.Instance.LevelEndEvent += ResetPlatforms;
        ScoreManager.Instance.GameEndEvent += ResetPlatforms;
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.Instance.currentStage == ScoreManager.GameStage.LevelPlaying)
        {
            // create platforms ahead of the player
            float distanceToNextPlatform = HorizontalPosition - player.transform.position.x;
            if (distanceToNextPlatform <= PlatformAheadDistance)
            {
                CreatePlatform(HorizontalPosition);
                HorizontalPosition += PlatformWidth + GapWidth;
            }

            // Destory platforms behind the player
            DestroyOldPlatforms();
        }
    }

    // creates a platform at a given horizontal position
    void CreatePlatform(float newHorizontalPosition)
    {
        GameObject newPlatform = Instantiate(PlatformPrefab);
        float perlinAmplitude = Mathf.PerlinNoise(0f, newHorizontalPosition * PerlinFrequency);

        float platformHeight = perlinAmplitude * (MaxHeight - MinHeight) + MinHeight;

        newPlatform.transform.position = new Vector3(newHorizontalPosition, platformHeight, 0f);
        newPlatform.transform.localScale = new Vector3(PlatformWidth, 1f, 1f);
        newPlatform.transform.parent = this.transform;

        // Add coins to the platform
        for (int i = 0; i < CoinsPerPlatform; i++)
        {
            GameObject newCoin = Instantiate(CoinPrefab);
            float coinOffset = (i + 0.5f) * (PlatformWidth / CoinsPerPlatform);
            newCoin.transform.position = newPlatform.transform.position + Vector3.right * coinOffset + Vector3.up * .5f;
            newCoin.transform.parent = newPlatform.transform;
        }

        Platforms.Add(newPlatform);
    }

    // destorys platforms that are behind the player
    void DestroyOldPlatforms()
    {
        for (int i = 0; i < Platforms.Count; i++)
        {
            float distanceBehindPlayer = player.transform.position.x - Platforms[i].transform.position.x;
            if (distanceBehindPlayer > PlatformBehindDistance)
            {
                GameObject platformToDestroy = Platforms[i];
                Platforms.Remove(platformToDestroy);
                Destroy(platformToDestroy);
            }
        }
    }

    public void DestoryAllPlatforms()
    {
        while (Platforms.Count > 0)
        {
            GameObject platformToDestroy = Platforms[0];
            Platforms.Remove(platformToDestroy);
            Destroy(platformToDestroy);
        }
    }

    public void ResetPlatforms()
    {
        HorizontalPosition = 0f;
        DestoryAllPlatforms();
    }
}
