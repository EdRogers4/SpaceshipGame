using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    //Public Spawn Variables
    public List<GameObject> listEnemy;
    public List<GameObject> listProton;
    public List<Transform> listSpawnpoint;
    public GameObject prefabFrigate;
    public GameObject prefabProton;

    //World Objects
    public GameObject particlesObject;

    //Private Spawn Variables
    private GameObject spawnedEnemy;
    private GameObject spawnedEnemyProjectile;
    private ParticleSystem spawnedParticle;

    //Enemy Targets
    public GameObject ship;
    public GameObject enemyDestroyed;

    //Enemy Stats
    public float thrustFrigate;
    public float handlingFrigate;
    public float velocityProton;
    public float targetingProton;

    //Audio
    private AudioSource audioSource;
    public AudioClip[] clipDestroyed;

    //Particles
    private ParticleSystem particleDestroyed;
    public ParticleSystem particleProtonDestroyed;

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
            spawnedEnemy = Instantiate(prefabFrigate, listSpawnpoint[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }

        yield return new WaitForSeconds(30.0f);
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

    public void ProtonDestroyed(GameObject destroyed)
    {
        spawnedParticle = Instantiate(particleProtonDestroyed, destroyed.transform.position, destroyed.transform.rotation);
        spawnedParticle.gameObject.transform.parent = particlesObject.transform;
        listProton.Remove(destroyed);
        Destroy(destroyed);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            distanceTargetMove = Vector3.Distance(listEnemy[i].transform.position, ship.transform.position);

            if (distanceTargetMove > minimumDistanceToTarget)
            {
                listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrustFrigate;
                Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                float singleStep = handlingFrigate * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        for (int i = 0; i < listProton.Count; i++)
        {
            distanceTargetMove = Vector3.Distance(listProton[i].transform.position, ship.transform.position);

            if (distanceTargetMove > minimumDistanceToTarget)
            {
                listProton[i].transform.position += listProton[i].transform.forward * Time.deltaTime * velocityProton;
                Vector3 targetDirection = ship.transform.position - listProton[i].transform.position;
                float singleStep = targetingProton * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(listProton[i].transform.forward, targetDirection, singleStep, 0.0f);
                listProton[i].transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}
