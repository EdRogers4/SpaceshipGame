using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    public Animator animator;
    public Enemies scriptEnemies;
    private AudioSource audioSource;

    void Start()
    {
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

    public void SpawnSquidFrigateGroup0()
    {
        StartCoroutine(scriptEnemies.SpawnSquidFrigateGroup0());
    }

    public void SpawnSquidFrigateGroup1()
    {
        StartCoroutine(scriptEnemies.SpawnSquidFrigateGroup1());
    }

    public void SpawnSquidFrigateGroup2()
    {
        StartCoroutine(scriptEnemies.SpawnSquidFrigateGroup2());
    }

    public void SpawnSquidFrigateGroup3()
    {
        StartCoroutine(scriptEnemies.SpawnSquidFrigateGroup3());
    }

    public void SpawnSquidProbe0()
    {
        StartCoroutine(scriptEnemies.SpawnSquidProbe0());
    }

    public void SpawnSquidProbe1()
    {
        StartCoroutine(scriptEnemies.SpawnSquidProbe1());
    }

    public void SpawnSquidDestroyer0()
    {
        StartCoroutine(scriptEnemies.SpawnSquidDestroyer0());
    }

    public void SpawnSquidDestroyer1()
    {
        StartCoroutine(scriptEnemies.SpawnSquidDestroyer1());
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
}
