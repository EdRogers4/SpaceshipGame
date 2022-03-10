using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip clipButton;

    public void SetScreenToFade()
    {
        audioSource.PlayOneShot(clipButton, 1.0f);
        animator.SetBool("isFade", true);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
