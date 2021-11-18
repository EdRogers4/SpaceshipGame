using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public Image dimmer;
    public static int scoreValue = 0;
    public Text scoreText;
    private string scoreString;

    static float volume;

    public bool isPause;
    public bool isSettings;
    public bool isMenu;
    public int difficulty; //0 = normal. 1 = easy, 2 = hard

    private AudioSource audioSource;

    private void Awake()
    {
        dimmer.gameObject.SetActive(true);
    }

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        scoreString = scoreValue.ToString("0");
        scoreText.text = scoreString + "00";
        StartCoroutine(FadeTo(0f, 3.0f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = dimmer.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            dimmer.color = newColor;
            yield return null;
        }
    }

    public void UpdateScore()
    {
        scoreValue += 1;
        scoreString = scoreValue.ToString("0");

        if (scoreValue < 10)
        {
            scoreText.text = "00" + scoreString;
        }
        else if (scoreValue < 100)
        {
            scoreText.text = "0" + scoreString;
        }
        else
        {
            scoreText.text = scoreString;
        }
    }

    public void SetGameDifficulty(int level)
    {
        difficulty = level;
    }

    public void TogglePauseSettings()
    {
        isMenu = false;

        if (isPause && isSettings)
        {
            Time.timeScale = 1.0f;
            isSettings = false;
            isPause = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            isSettings = true;
            isPause = true;
        }
    }

    public void TogglePauseMenu()
    {
        isSettings = false;

        if (isPause && isMenu)
        {
            Time.timeScale = 1.0f;
            isMenu = false;
            isPause = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            isMenu = true;
            isPause = true;
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            isPause = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            isPause = false;
        }
    }

    public void ToggleMusic(bool music)
    {
        if (music)
        {
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
