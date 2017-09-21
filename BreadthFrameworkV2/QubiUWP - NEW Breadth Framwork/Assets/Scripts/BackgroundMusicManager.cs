using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour {

    private AudioSource BackgroundAudioSource;
    public BackgroundMusicCollection[] collection;
    private int selectedCollection;
    private int currentLevel = 0;


    	// Use this for initialization
	void Awake () {
        if (collection == null || collection.Length < 1)
        {
            Debug.Log("No audio added");
        }

        BackgroundAudioSource = GetComponent<AudioSource>();
	}

    public void StartBackgroundMusic()
    {
        selectedCollection = Random.Range(0, collection.Length);
        if (BackgroundAudioSource.isPlaying)
        {
            BackgroundAudioSource.Stop();
        }
        currentLevel = 0;
        BackgroundAudioSource.clip = collection[selectedCollection].BackgroundMusic[currentLevel];
        StartCoroutine(AudioFader.FadeIn(BackgroundAudioSource, 0.5f));
    }

    public void PlayNextLevel(int nextLevel)
    {
        if (nextLevel < collection[selectedCollection].BackgroundMusic.Length)
        {
            StartCoroutine(CrossFadeAudio(nextLevel));
        }
    }

    private IEnumerator CrossFadeAudio(int nextLevel)
    {
        yield return StartCoroutine(AudioFader.FadeOut(BackgroundAudioSource, 0.5f));
        currentLevel = nextLevel;
        BackgroundAudioSource.clip = collection[selectedCollection].BackgroundMusic[currentLevel];
        yield return StartCoroutine(AudioFader.FadeIn(BackgroundAudioSource, 0.5f));
    }

    public void StopBackgroundMusic()
    {
        StartCoroutine(AudioFader.FadeOut(BackgroundAudioSource, 0.5f));
    }

}
