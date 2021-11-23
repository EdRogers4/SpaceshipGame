using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public Animator animator;
    public Enemies scriptEnemies;

    void Start()
    {
        animator.SetBool("isStart", true);
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
}
