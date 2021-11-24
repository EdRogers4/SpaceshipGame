using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("UI")]
    public Image dimmer;
    public Animator animatorMessage;
    public GameObject gameOver;
    public Text textGameOver;

    [Header("Score")]
    public int scoreValue = 0;
    public int enemyKOValue = 0;
    public int countAnnouncer = 0;
    public int countBullseye = 0;
    public int countCombo;
    public bool isTimerCombo;
    public float timerComboAmount;
    public float timerComboMax;
    public Text scoreText;
    private string scoreString;

    [Header("Menu")]
    public bool isPause;
    public bool isSettings;
    public bool isMenu;
    public int difficulty; //0 = normal. 1 = easy, 2 = hard

    [Header("Audio")]
    static float volume;
    private AudioSource audioSource;
    public AudioClip[] clipBullseye;
    public AudioClip[] clipCombo;
    public AudioClip[] clipGameOver;
    public AudioClip[] clipGamePaused;
    public AudioClip[] clipHealthLow;
    public AudioClip clipGetReady;
    public AudioClip clipMissionComplete;
    public AudioClip clipUntouchable;

    private void Awake()
    {
        dimmer.gameObject.SetActive(true);
    }

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        scoreString = scoreValue.ToString("0");
        scoreText.text = scoreString + "00";
        StartCoroutine(FadeTo(0f, 2.0f));
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
        dimmer.gameObject.SetActive(false);
        audioSource.PlayOneShot(clipGetReady, 1.0f);
    }

    public IEnumerator AudioClipPlayBullseye(int number)
    {
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(clipBullseye[number], 1.0f);
    }

    public IEnumerator AudioClipPlayCombo()
    {
        yield return new WaitForSeconds(1.0f);
        if (countCombo == 2)
        {
            //audioSource.PlayOneShot(clipCombo[0], 1.0f);
        }
        else if (countCombo == 3)
        {
            audioSource.PlayOneShot(clipCombo[1], 1.0f);
        }
        else if (countCombo == 4)
        {
            audioSource.PlayOneShot(clipCombo[2], 1.0f);
        }
        else if (countCombo >= 5)
        {
            audioSource.PlayOneShot(clipCombo[3], 1.0f);
        }

        countCombo = 0;
    }

    public IEnumerator AudioClipPlayGameOver()
    {
        yield return new WaitForSeconds(2.0f);
        audioSource.PlayOneShot(clipGameOver[0], 1.0f);
        yield return new WaitForSeconds(2.0f);
        audioSource.PlayOneShot(clipGameOver[1], 1.0f);
    }

    public void AudioClipPlayGamePaused(int number)
    {
        if (clipGamePaused[number] != null)
        {
            //audioSource.PlayOneShot(clipGamePaused[number], 1.0f);
        }
    }

    public IEnumerator AudioClipPlayHealthLow(int number)
    {
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(clipHealthLow[number], 1.0f);
    }

    public void AudioClipPlayGetReady()
    {
        audioSource.PlayOneShot(clipGetReady, 1.0f);
    }

    public void PlayMissionComplete()
    {
        StartCoroutine(AudioClipPlayMissionComplete());
    }

    public IEnumerator AudioClipPlayMissionComplete()
    {
        yield return new WaitForSeconds(2.0f);
        animatorMessage.SetBool("isMissionComplete", true);
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(clipMissionComplete, 1.0f);
        animatorMessage.SetBool("isMissionComplete", false);
        yield return new WaitForSeconds(5.5f);
        gameOver.SetActive(true);
        textGameOver.text = "Complete";
        Time.timeScale = 0.0f;
    }

    public void AudioClipPlayUntouchable()
    {
        audioSource.PlayOneShot(clipUntouchable, 1.0f);
    }

    public void UpdateEnemyKOValue()
    {
        enemyKOValue += 1;
        countAnnouncer += 1;
        countCombo += 1;

        if (countCombo >= 2)
        {
            StartCoroutine(AudioClipPlayCombo());
        }
        else if (!isTimerCombo)
        {
            if (enemyKOValue == 1)
            {
                StartCoroutine(AudioClipPlayBullseye(0));
            }

            if (countAnnouncer >= 10)
            {
                countAnnouncer = 0;
                countBullseye += 1;

                if (countBullseye <= 6)
                {
                    StartCoroutine(AudioClipPlayBullseye(countBullseye));
                }
                else
                {
                    StartCoroutine(AudioClipPlayBullseye(Random.Range(0, 6)));
                }
            }
        }

        if (countCombo >= 2)
        {
            isTimerCombo = true;
        }
    }

    private void Update()
    {
        if (isTimerCombo)
        {
            timerComboAmount -= 0.01f;

            if (timerComboAmount <= 0f)
            {
                timerComboAmount = timerComboMax;
                countCombo = 0;
                isTimerCombo = false;
            }
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
            AudioClipPlayGamePaused(1);
            isSettings = false;
            isPause = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            AudioClipPlayGamePaused(0);
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
            AudioClipPlayGamePaused(1);
            isMenu = false;
            isPause = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            AudioClipPlayGamePaused(0);
            isMenu = true;
            isPause = true;
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            AudioClipPlayGamePaused(0);
            isPause = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            AudioClipPlayGamePaused(1);
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
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
