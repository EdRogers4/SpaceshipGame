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
        scriptEnemies.SpawnFrigateGroup0();
    }

    public void SpawnFrigateGroup1()
    {
        scriptEnemies.SpawnFrigateGroup1();
    }

    public void SpawnFrigateGroup2()
    {
        scriptEnemies.SpawnFrigateGroup2();
    }

    public void SpawnFrigateGroup3()
    {
        scriptEnemies.SpawnFrigateGroup3();
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
}
