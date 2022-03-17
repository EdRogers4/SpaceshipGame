using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip clipButton;
    public Text textLevel;
    public int level;

    public void SetScreenToFade()
    {
        audioSource.PlayOneShot(clipButton, 1.0f);
        animator.SetBool("isFade", true);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void ChangeLevel()
    {
        if (level <= 1)
        {
            level = 2;
            textLevel.text = "Level 2";
        }
        else if (level > 1)
        {
            level = 1;
            textLevel.text = "Level 1";
        }
    }
}
