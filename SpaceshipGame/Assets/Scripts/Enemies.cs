using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    //Public Spawn Variables
    public List<GameObject> listEnemy;
    public List<Transform> listSpawnpoint;
    public GameObject prefabEnemy;

    //World Objects
    public GameObject particlesObject;

    //Private Spawn Variables
    private GameObject spawnedEnemy;

    //Enemy Targets
    public GameObject ship;
    public GameObject enemyDestroyed;

    //Enemy Stats
    public float thrust;
    public float handling;

    //Audio
    private AudioSource audioSource;
    public AudioClip[] clipDestroyed;

    //Particles
    private ParticleSystem particleDestroyed;

    //Private
    public float distanceTargetMove;
    private float minimumDistanceToTarget = 2.0f;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < listSpawnpoint.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabEnemy, listSpawnpoint[i].position, Quaternion.identity);
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }

        yield return new WaitForSeconds(15.0f);
        StartCoroutine(SpawnEnemy());
    }

    public IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(particle);
    }

        public void EnemyDestroyed()
    {
        audioSource.PlayOneShot(clipDestroyed[Random.Range(0, clipDestroyed.Length)], 0.25f);
        Destroy(enemyDestroyed);
    }

    void Update()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            distanceTargetMove = Vector3.Distance(listEnemy[i].transform.position, ship.transform.position);

            if (distanceTargetMove > minimumDistanceToTarget)
            {
                listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrust;
                Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                float singleStep = handling * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}
