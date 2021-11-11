using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    static float volume;

    public bool isPause;
    public bool isSettings;
    public bool isMenu;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        
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
