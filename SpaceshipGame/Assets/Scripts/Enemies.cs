using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Lists")]
    public List<GameObject> listEnemy;
    public List<GameObject> listProton;
    public List<GameObject> listAsteroid;
    public List<GameObject> listPlasma;
    public List<Transform> listSpawnpoint;
    public List<Transform> listAsteroidSpawn;
    public List<Transform> listWingSpawnGroup0;
    public List<Transform> listWingSpawnGroup1;
    public List<Transform> listWingSpawnGroup2;
    public List<Transform> listWingSpawnGroup3;
    public List<float> listAsteroidSpeed;

    [Header("Spawn")]
    public GameObject prefabFrigate;
    public GameObject prefabWing;
    public GameObject prefabPlasma;
    public GameObject prefabProton;
    public GameObject prefabAsteroid;

    [Header("Scripts")]
    public Ship scriptShip;
    public GameSettings scriptGameSettings;

    [Header("World")]
    public GameObject particlesObject;
    public GameObject ship;
    public GameObject enemyDestroyed;
    public Camera cameraMain;

    [Header("Spawn")]
    public float timeMinimumSpawnAsteroid;
    public float timeMaximumSpawnAsteroid;
    public float speedAsteroidMinimum;
    public float speedAsteroidMaximum;
    private int countSpawnAsteroid;
    private GameObject spawnedEnemy;
    private GameObject spawnedEnemyProjectile;
    private GameObject spawnedAsteroid;
    private ParticleSystem spawnedParticle;

    [Header("Stats")]
    public float thrustFrigate;
    public float handlingFrigate;
    public float velocityProton;
    public float targetingProton;
    public float thrustWing;
    public float handlingWing;
    public float velocityPlasma;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] clipDestroyed;
    public AudioClip[] clipProtonDestroyed;
    public AudioClip[] clipPlasmaDestroyed;
    public AudioClip[] clipAsteroidDestroyed;

    [Header("Particles")]
    private ParticleSystem particleDestroyed;
    public ParticleSystem particleProtonDestroyed;
    public ParticleSystem particlePlasmaDestroyed;

    [Header("Distance")]
    public float distanceTargetMove;
    private float minimumDistanceFrigate = 4.0f;
    private float minimumDistanceWing = 100.0f;
    private float minimumDistanceProton = 0.1f;


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(SpawnFrigate());
        StartCoroutine(SpawnAsteroid());
    }

    public IEnumerator SpawnAsteroid()
    {
        countSpawnAsteroid = Random.Range(0, listAsteroidSpawn.Count);
        yield return new WaitForSeconds(Random.Range(timeMinimumSpawnAsteroid, timeMaximumSpawnAsteroid));
        spawnedAsteroid = Instantiate(prefabAsteroid, listAsteroidSpawn[countSpawnAsteroid].transform.position, listAsteroidSpawn[countSpawnAsteroid].transform.rotation);
        listAsteroid.Add(spawnedAsteroid);
        listAsteroidSpeed.Add(Random.Range(speedAsteroidMinimum, speedAsteroidMaximum));
        countSpawnAsteroid += 1;

        if (countSpawnAsteroid >= listAsteroidSpawn.Count)
        {
            countSpawnAsteroid = 0;
        }

        spawnedAsteroid.GetComponent<Asteroid>().scriptEnemies = this;
        spawnedAsteroid.GetComponent<Asteroid>().destination = listAsteroidSpawn[countSpawnAsteroid];
        StartCoroutine(SpawnAsteroid());
    }

    public IEnumerator SpawnFrigate()
    {
        for (int i = 0; i < listSpawnpoint.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabFrigate, listSpawnpoint[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }

        yield return new WaitForSeconds(30.0f);
        StartCoroutine(SpawnFrigate());
    }

    public void SpawnWingGroup0()
    {
        for (int i = 0; i < listWingSpawnGroup0.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listWingSpawnGroup0[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void SpawnWingGroup1()
    {
        for (int i = 0; i < listWingSpawnGroup1.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listWingSpawnGroup1[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void SpawnWingGroup2()
    {
        for (int i = 0; i < listWingSpawnGroup2.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listWingSpawnGroup2[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void SpawnWingGroup3()
    {
        for (int i = 0; i < listWingSpawnGroup3.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listWingSpawnGroup3[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void EnemyDestroyed()
    {
        audioSource.PlayOneShot(clipDestroyed[Random.Range(0, clipDestroyed.Length)], 0.25f);
        Destroy(enemyDestroyed);
    }

    public void AsteroidDestroyed()
    {
        audioSource.PlayOneShot(clipAsteroidDestroyed[Random.Range(0, clipAsteroidDestroyed.Length)], 0.35f);
    }

    public void ProtonDestroyed(GameObject destroyed)
    {
        audioSource.PlayOneShot(clipProtonDestroyed[Random.Range(0, clipProtonDestroyed.Length)], 0.6f);
        spawnedParticle = Instantiate(particleProtonDestroyed, destroyed.transform.position, destroyed.transform.rotation);
        spawnedParticle.gameObject.transform.parent = particlesObject.transform;
        listProton.Remove(destroyed);
        Destroy(destroyed);
    }

    public void PlasmaDestroyed(GameObject destroyed)
    {
        audioSource.PlayOneShot(clipPlasmaDestroyed[Random.Range(0, clipPlasmaDestroyed.Length)], 0.6f);
        spawnedParticle = Instantiate(particlePlasmaDestroyed, destroyed.transform.position, destroyed.transform.rotation);
        spawnedParticle.gameObject.transform.parent = particlesObject.transform;
        listPlasma.Remove(destroyed);
        Destroy(destroyed);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < listAsteroid.Count; i++)
        {
            if (listAsteroid[i] != null)
            {
                float step = listAsteroidSpeed[i] * Time.deltaTime;
                listAsteroid[i].transform.position = Vector3.MoveTowards(listAsteroid[i].transform.position, listAsteroid[i].GetComponent<Asteroid>().destination.position, step);
            }
        }

        for (int i = 0; i < listPlasma.Count; i++)
        {
            listPlasma[i].transform.position += listPlasma[i].transform.forward * Time.deltaTime * velocityPlasma;
        }

        if ((scriptShip.shipName == "Fighter" && !scriptShip.isDeadFighter) || (scriptShip.shipName == "Bomber" && !scriptShip.isDeadBomber) 
            || (scriptShip.shipName == "Interceptor" && !scriptShip.isDeadInterceptor) || (scriptShip.shipName == "Breaker" && !scriptShip.isDeadBreaker))
        {
            for (int i = 0; i < listEnemy.Count; i++)
            {

                distanceTargetMove = Vector3.Distance(listEnemy[i].transform.position, ship.transform.position);

                if (listEnemy[i].GetComponent<Enemy>().enemyName == "Frigate")
                {
                    if (distanceTargetMove > minimumDistanceFrigate)
                    {
                        listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrustFrigate;
                        Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                        float singleStep = handlingFrigate * Time.deltaTime;
                        Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                        listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "Wing")
                {
                    if (distanceTargetMove > minimumDistanceWing)
                    {
                        listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrustWing;
                    }

                    Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                    float singleStep = handlingWing * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                    listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }

            for (int i = 0; i < listProton.Count; i++)
            {
                distanceTargetMove = Vector3.Distance(listProton[i].transform.position, ship.transform.position);

                if (distanceTargetMove > minimumDistanceProton)
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
}
