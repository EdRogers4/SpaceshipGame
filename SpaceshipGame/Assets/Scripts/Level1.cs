using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public Animator animator;
    public Enemies scriptEnemies;
    private AudioSource audioSource;
    public AudioClip clipWarning;
    public AudioClip clipSiren;
    public AudioClip clipColossalShipFlyIn;
    public AudioClip clipColossalShipFlyOut;
    public AudioClip clipColossalShipFly;
    public AudioClip clipPunisherAffirmative;
    public AudioClip clipPunisherEnter;
    public AudioClip clipPunisherHuh;
    public AudioClip clipPunisherLockedOn;
    public AudioClip clipPunisherNoTarget;
    public AudioClip clipPunisherSearching;
    public AudioClip clipPunisherTargetAcquired;
    public AudioClip clipPunisherWhatIsThat;

    void Start()
    {
        animator.SetBool("isStart", true);
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SpawnFrigateGroup0()
    {
        StartCoroutine(scriptEnemies.SpawnFrigateGroup0());
    }

    public void SpawnFrigateGroup1()
    {
        StartCoroutine(scriptEnemies.SpawnFrigateGroup1());
    }

    public void SpawnFrigateGroup2()
    {
        StartCoroutine(scriptEnemies.SpawnFrigateGroup2());
    }

    public void SpawnFrigateGroup3()
    {
        StartCoroutine(scriptEnemies.SpawnFrigateGroup3());
    }

    public void SpawnWingGroup0()
    {
        scriptEnemies.SpawnWingGroup0();
    }

    public void SpawnWingGroup1()
    {
        scriptEnemies.SpawnWingGroup1();
    }

    public void SpawnWingGroup2()
    {
        scriptEnemies.SpawnWingGroup2();
    }

    public void SpawnWingGroup3()
    {
        scriptEnemies.SpawnWingGroup3();
    }

    public void SpawnAsteroids()
    {
        StartCoroutine(scriptEnemies.SpawnAsteroid());
    }

    public void StopSpawnAsteroids()
    {
        StopCoroutine(scriptEnemies.SpawnAsteroid());
    }

    public void SpawnDroneGroup0()
    {
        StartCoroutine(scriptEnemies.SpawnDroneGroup0());
    }

    public void SpawnDroneGroup1()
    {
        StartCoroutine(scriptEnemies.SpawnDroneGroup1());
    }

    public void SpawnPunisher()
    {
        StartCoroutine(scriptEnemies.SpawnPunisher());
    }

    public void AudioClipPlayWarning()
    {
        audioSource.PlayOneShot(clipWarning, 0.5f);
    }

    public void AudioClipPlaySiren()
    {
        audioSource.PlayOneShot(clipSiren, 0.3f);
    }

    public void PlayColossalShipFlyIn()
    {
        audioSource.PlayOneShot(clipColossalShipFlyIn, 0.1f);
    }

    public void PlayColossalShipFlyOut()
    {
        audioSource.PlayOneShot(clipColossalShipFlyOut, 0.1f);
    }

    public void PlayColossalShipFly()
    {
        audioSource.PlayOneShot(clipColossalShipFly, 0.1f);
    }

    public void PlayPunisherAffirmative()
    {
        audioSource.PlayOneShot(clipPunisherAffirmative, 1.0f);
    }

    public void PlayPunisherEnter()
    {
        audioSource.PlayOneShot(clipPunisherEnter, 1.0f);
    }

    public void PlayPunisherHuh()
    {
        audioSource.PlayOneShot(clipPunisherHuh, 1.0f);
    }

    public void PlayPunisherLockedOn()
    {
        audioSource.PlayOneShot(clipPunisherLockedOn, 1.0f);
    }

    public void PlayPunisherNoTarget()
    {
        audioSource.PlayOneShot(clipPunisherNoTarget, 1.0f);
    }

    public void PlayPunisherSearching()
    {
        audioSource.PlayOneShot(clipPunisherSearching, 1.0f);
    }

    public void PlayPunisherTargetAcquired()
    {
        audioSource.PlayOneShot(clipPunisherTargetAcquired, 1.0f);
    }

    public void PlayPunisherWhatIsThat()
    {
        audioSource.PlayOneShot(clipPunisherWhatIsThat, 1.0f);
    }
}
