using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Level")]
    public int currentLevel;
    public bool isCompleteDemo;

    [Header("UI")]
    public Image dimmer;
    public Animator animatorMessage;
    public GameObject gameOver;
    public Text textGameOver;
    public Text textResolution;
    public GameObject completeText;

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
    private bool isVoiceMute;
    public bool isMusicPaused;
    public AudioSource audioSourceMusic;
    private AudioSource audioSource;
    public AudioClip[] clipBullseye;
    public AudioClip[] clipCombo;
    public AudioClip[] clipGameOver;
    public AudioClip[] clipGamePaused;
    public AudioClip[] clipHealthLow;
    public AudioClip clipGetReady;
    public AudioClip clipMissionComplete;
    public AudioClip clipUntouchable;
    public AudioClip clipUltimate;
    public AudioClip clipGameMissionCompleteSFX;
    public AudioClip clipGameOverSFX;
    public AudioClip clipGamePausedSFX;
    public AudioClip clipGameRestartSFX;
    public AudioClip clipGameResumedSFX;
    public AudioClip clipMenuClose;
    public AudioClip clipMenuOpen;
    public AudioClip clipMenuSwitchOn;
    public AudioClip clipMenuSwitchOff;
    public AudioClip clipMenuSelect;

    [Header("Screen")]
    private float screenWidth;
    private float screenHeight;
    public float screenRatio;
    public GameObject[] boarderPhone;
    public GameObject[] boarder10point5;
    public GameObject[] boarder12point9;

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

        screenWidth = Screen.currentResolution.width;
        screenHeight = Screen.currentResolution.height;
        screenRatio = screenWidth / screenHeight;
        //textResolution.text = "Ratio: " + screenRatio;

        if (screenRatio >= 2.2f)
        {
            boarderPhone[0].SetActive(true);
            boarderPhone[1].SetActive(true);
        }
        else if (screenRatio < 1.6f)
        {
            boarder12point9[0].SetActive(true);
            boarder12point9[1].SetActive(true);
        }
        else
        {
            boarder10point5[0].SetActive(true);
            boarder10point5[1].SetActive(true);
        }
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

        if (!isVoiceMute)
        {
            audioSource.PlayOneShot(clipGetReady, 1.0f);
        }

        yield return new WaitForSeconds(1.0f);
        audioSourceMusic.enabled = true;
    }
    public void FadeOut()
    {
        dimmer.gameObject.SetActive(true);
        Color newColor = new Color(0, 0, 0, 255);
        dimmer.color = newColor;
    }

    public IEnumerator AudioClipPlayBullseye(int number)
    {
        if (!isVoiceMute)
        {
            yield return new WaitForSeconds(1.0f);
            audioSource.PlayOneShot(clipBullseye[number], 1.0f);
        }
    }

    public IEnumerator AudioClipPlayCombo()
    {
        if (!isVoiceMute)
        {
            yield return new WaitForSeconds(0.5f);
            if (countCombo == 3)
            {
                audioSource.PlayOneShot(clipCombo[0], 1.0f);
            }
            else if (countCombo == 4)
            {
                audioSource.PlayOneShot(clipCombo[1], 1.0f);
            }
            else if (countCombo == 5)
            {
                audioSource.PlayOneShot(clipCombo[2], 1.0f);
            }
            else if (countCombo >= 6)
            {
                audioSource.PlayOneShot(clipCombo[3], 1.0f);
            }
        }

        countCombo = 0;
    }

    public IEnumerator AudioClipPlayGameOver()
    {
        if (!isVoiceMute)
        {
            yield return new WaitForSeconds(2.0f);
            audioSource.PlayOneShot(clipGameOver[0], 1.0f);
            yield return new WaitForSeconds(2.0f);
            audioSource.PlayOneShot(clipGameOver[1], 1.0f);
        }
    }

    public void AudioClipPlayGamePaused(int number)
    {
        if (!isVoiceMute)
        {
            //audioSource.PlayOneShot(clipGamePaused[number], 1.0f);
        }
    }

    public IEnumerator AudioClipPlayHealthLow(int number)
    {
        if (!isVoiceMute)
        {
            yield return new WaitForSeconds(1.5f);
            audioSource.PlayOneShot(clipHealthLow[number], 1.0f);
        }
    }

    public void AudioClipPlayGetReady()
    {
        if (!isVoiceMute)
        {
            audioSource.PlayOneShot(clipGetReady, 1.0f);
        }
    }

    public void PlayMissionComplete()
    {
        if (!isVoiceMute)
        {
            StartCoroutine(AudioClipPlayMissionComplete());
        }
    }

    public IEnumerator AudioClipPlayMissionComplete()
    {
        yield return new WaitForSeconds(2.0f);
        animatorMessage.SetBool("isMissionComplete", true);
        audioSource.PlayOneShot(clipGameMissionCompleteSFX, 1.0f);
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(clipMissionComplete, 1.0f);
        animatorMessage.SetBool("isMissionComplete", false);
        yield return new WaitForSeconds(5.5f);

        if (currentLevel == 2)
        {
            gameOver.SetActive(true);
            textGameOver.text = "Complete";
            FadeOut();
            completeText.SetActive(true);
            isCompleteDemo = true;
            Time.timeScale = 0.0f; 
        }
        else if (currentLevel == 1)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

    }

    public void AudioClipPlayUntouchable()
    {
        if (!isVoiceMute)
        {
            audioSource.PlayOneShot(clipUntouchable, 1.0f);
        }
    }

    public void AudioClipPlayUltimate()
    {
        if (!isVoiceMute)
        {
            StartCoroutine(DelayAudioClipPlayUltimate());  
        }
    }

    public IEnumerator DelayAudioClipPlayUltimate()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(clipUltimate, 1.0f);
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
            audioSourceMusic.Play();
            AudioClipPlayGamePaused(1);
            audioSource.PlayOneShot(clipGameResumedSFX, 1.0f);
            isSettings = false;
            isPause = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            audioSourceMusic.Pause();
            AudioClipPlayGamePaused(0);
            audioSource.PlayOneShot(clipGamePausedSFX, 1.0f);
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
            audioSourceMusic.Play();
            AudioClipPlayGamePaused(1);
            audioSource.PlayOneShot(clipGameResumedSFX, 1.0f);
            isMenu = false;
            isPause = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            audioSourceMusic.Pause();
            AudioClipPlayGamePaused(0);
            audioSource.PlayOneShot(clipGamePausedSFX, 1.0f);
            isMenu = true;
            isPause = true;
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            audioSourceMusic.Pause();
            AudioClipPlayGamePaused(0);
            audioSource.PlayOneShot(clipGamePausedSFX, 1.0f);
            isPause = true;
        }
        else
        {
            if (audioSourceMusic.isActiveAndEnabled)
            {
                Time.timeScale = 1.0f;
                audioSourceMusic.Play();
                AudioClipPlayGamePaused(1);
                isPause = false;
            }
        }
    }

    public void ToggleMusic(bool music)
    {
        if (music)
        {
            audioSourceMusic.enabled = true;
            audioSource.PlayOneShot(clipMenuSwitchOn, 1.0f);
        }
        else
        {
            audioSourceMusic.enabled = false;
            audioSource.PlayOneShot(clipMenuSwitchOff, 1.0f);
        }
    }

    public void ToggleVoice(bool mute)
    {
        if (mute)
        {
            isVoiceMute = true;
            audioSource.PlayOneShot(clipMenuSwitchOn, 1.0f);
        }
        else
        {
            isVoiceMute = false;
            audioSource.PlayOneShot(clipMenuSwitchOff, 1.0f);
        }
    }

    public void AudioClipPlayMenuOpen()
    {
        audioSource.PlayOneShot(clipMenuOpen, 1.0f);
    }

    public void AudioClipPlayMenuSelect()
    {
        audioSource.PlayOneShot(clipMenuSelect, 1.0f);
    }

    public void AudioClipPlayMenuClose()
    {
        audioSource.PlayOneShot(clipMenuClose, 1.0f);
        audioSource.PlayOneShot(clipGameResumedSFX, 1.0f);
    }

    public void Restart()
    {
        if (!isCompleteDemo)
        {
            SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        audioSource.PlayOneShot(clipGameRestartSFX, 1.0f);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
