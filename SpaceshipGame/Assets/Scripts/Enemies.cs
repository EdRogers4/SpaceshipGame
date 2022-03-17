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
    public List<GameObject> listRocket;
    public List<Transform> listSpawnFrigate0;
    public List<Transform> listSpawnFrigate1;
    public List<Transform> listSpawnFrigate2;
    public List<Transform> listSpawnFrigate3;
    public List<Transform> listSpawnAsteroid;
    public List<Transform> listSpawnWing0;
    public List<Transform> listSpawnWing1;
    public List<Transform> listSpawnWing2;
    public List<Transform> listSpawnWing3;
    public List<Transform> listSpawnDestroyer;
    public Transform spawnDrone0;
    public Transform spawnDrone1;
    public Transform spawnPunisher;
    public List<Transform> listFlyPointDrone0;
    public List<Transform> listFlyPointDrone1;
    public List<Transform> listFlyPointPunisher0;
    public List<Transform> listFlyPointPunisher1;
    public List<Transform> listFlyPointSquidShark;
    public Transform flyPointPunisherEntrance;
    public List<float> listAsteroidSpeed;

    [Header("Spawn")]
    public GameObject prefabFrigate;
    public GameObject prefabSquidFrigate;
    public GameObject prefabSquidProbe;
    public GameObject prefabSquidDestroyer;
    public GameObject prefabWing;
    public GameObject prefabPlasma;
    public GameObject prefabProton;
    public GameObject prefabAsteroid;
    public GameObject prefabDrone;
    public GameObject prefabRocket;
    public GameObject prefabPunisher;
    public GameObject prefabSquidShark;

    [Header("Scripts")]
    public Ship scriptShip;
    public GameSettings scriptGameSettings;

    [Header("World")]
    public GameObject particlesObject;
    public GameObject instancesObject;
    public GameObject ship;
    public GameObject enemyDestroyed;
    public Camera cameraMain;
    private Vector3 newTargetPosition;
    public int countSquidDestroyerDestroyed;
    public bool isEnemyDestroyed;

    [Header("Spawn")]
    public float timeMinimumSpawnAsteroid;
    public float timeMaximumSpawnAsteroid;
    public float speedAsteroidMinimum;
    public float speedAsteroidMaximum;
    private int countSpawnAsteroid;
    private bool isSpawnFirstAsteroid;
    public bool isNoAsteroidsOnThisLevel;
    private GameObject spawnedEnemy;
    private GameObject spawnedEnemyProjectile;
    private GameObject spawnedAsteroid;
    private ParticleSystem spawnedParticle;
    public ParticleSystem particleSpawnFrigate;
    public ParticleSystem particleSpawnMothership;

    [Header("Stats")]
    public float thrustFrigate;
    public float handlingFrigate;
    public float thrustSquidFrigate;
    public float handlingSquidFrigate;
    public float thrustSquidProbe;
    public float handlingSquidProbe;
    public float thrustSquidDestroyer;
    public float handlingSquidDestroyer;
    public float velocityProton;
    public float targetingProton;
    public float thrustWing;
    public float handlingWing;
    public float velocityPlasma;
    public float thrustDrone;
    public float handlingDrone;
    public float velocityRocket;
    public float targetingRocket;
    public float thrustPunisher;
    public float handlingPunisher;
    public float thrustSquidShark;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] clipDestroyed;
    public AudioClip[] clipProtonDestroyed;
    public AudioClip[] clipPlasmaDestroyed;
    public AudioClip[] clipRocketDestroyed;
    public AudioClip[] clipAsteroidDestroyed;
    public AudioClip[] clipTeleport;
    public AudioClip[] clipMS_Start;
    public AudioClip[] clipMS_Blast;
    public AudioClip[] clipMS_Echo;
    public AudioClip[] clipMS_Charge;
    public AudioClip[] clipMS_Device;
    public AudioClip[] clipMS_Energy;
    public AudioClip[] clipMS_Feedback;
    public AudioClip[] clipMS_Gears;
    public AudioClip[] clipMS_Light;
    public AudioClip[] clipMS_Target;
    public AudioClip[] clipMS_Voice;
    public AudioClip[] clipLightningExplosion;
    public AudioClip clipDomeExplosion;
    public AudioClip clipNovaExplosion;

    [Header("Animations")]
    public Animator animatorCamera;

    [Header("Particles")]
    private ParticleSystem particleDestroyed;
    public ParticleSystem particleProtonDestroyed;
    public ParticleSystem particlePlasmaDestroyed;
    public ParticleSystem particleRocketDestroyed;

    [Header("Fly Points")]
    private Transform thisFlyPoint;

    [Header("Distance")]
    public float distanceTargetMove;
    public float distanceToFlyPoint;
    private float minimumDistanceFrigate = 4.0f;
    private float minimumDistanceSquidFrigate = 4.0f;
    private float minimumDistanceSquidProbe = 36.0f;
    private float minimumDistanceSquidDestroyer = 10f;
    private float minimumDistanceWing = 100.0f;
    private float minimumDistanceProton = 0.1f;


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (!isNoAsteroidsOnThisLevel)
        {
            StartCoroutine(SpawnAsteroid());
        }
    }

    public void StartCameraShake()
    {
        StartCoroutine(CameraShakeDestroyed());
    }

    public void PlayClipDomeExplosion()
    {
        audioSource.PlayOneShot(clipDomeExplosion, 1.0f);
    }

    public IEnumerator CameraShakeDestroyed()
    {
        animatorCamera.SetInteger("Shake", Random.Range(1, 3));
        yield return new WaitForSeconds(0.25f);
        animatorCamera.SetInteger("Shake", 0);
    }

    public IEnumerator SpawnAsteroid()
    {
        if (isSpawnFirstAsteroid)
        {

            yield return new WaitForSeconds(Random.Range(timeMinimumSpawnAsteroid, timeMaximumSpawnAsteroid));
        }
        else
        {
            isSpawnFirstAsteroid = true;
        }

        countSpawnAsteroid = Random.Range(0, listSpawnAsteroid.Count);
        spawnedAsteroid = Instantiate(prefabAsteroid, listSpawnAsteroid[countSpawnAsteroid].transform.position, listSpawnAsteroid[countSpawnAsteroid].transform.rotation);
        spawnedAsteroid.transform.parent = instancesObject.transform;
        listAsteroid.Add(spawnedAsteroid);
        listAsteroidSpeed.Add(Random.Range(speedAsteroidMinimum, speedAsteroidMaximum));
        countSpawnAsteroid += 1;

        if (countSpawnAsteroid >= listSpawnAsteroid.Count)
        {
            countSpawnAsteroid = 0;
        }

        spawnedAsteroid.GetComponent<Asteroid>().scriptEnemies = this;
        spawnedAsteroid.GetComponent<Asteroid>().destination = listSpawnAsteroid[countSpawnAsteroid];
        StartCoroutine(SpawnAsteroid());
    }

    public IEnumerator SpawnFrigateGroup0()
    {
        for (int i = 0; i < listSpawnFrigate0.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate0[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabFrigate, listSpawnFrigate0[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnFrigateGroup1()
    {
        for (int i = 0; i < listSpawnFrigate1.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate1[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabFrigate, listSpawnFrigate1[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnFrigateGroup2()
    {
        for (int i = 0; i < listSpawnFrigate2.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate2[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabFrigate, listSpawnFrigate2[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnFrigateGroup3()
    {
        for (int i = 0; i < listSpawnFrigate3.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate3[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabFrigate, listSpawnFrigate3[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidFrigateGroup0()
    {
        for (int i = 0; i < listSpawnFrigate0.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate0[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabSquidFrigate, listSpawnFrigate0[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidFrigateGroup1()
    {
        for (int i = 0; i < listSpawnFrigate1.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate1[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabSquidFrigate, listSpawnFrigate1[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidFrigateGroup2()
    {
        for (int i = 0; i < listSpawnFrigate2.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate2[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabSquidFrigate, listSpawnFrigate2[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidFrigateGroup3()
    {
        for (int i = 0; i < listSpawnFrigate3.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listSpawnFrigate3[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabSquidFrigate, listSpawnFrigate3[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidProbe0()
    {
        for (int i = 0; i < listFlyPointDrone0.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listFlyPointDrone0[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabSquidProbe, listFlyPointDrone0[i].position, listFlyPointDrone0[i].rotation);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidProbe1()
    {
        for (int i = 0; i < listFlyPointDrone1.Count; i++)
        {
            spawnedParticle = Instantiate(particleSpawnFrigate, listFlyPointDrone1[i].position, Quaternion.identity);
            spawnedParticle.transform.parent = particlesObject.transform;
            audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
            yield return new WaitForSeconds(0.75f);
            spawnedEnemy = Instantiate(prefabSquidProbe, listFlyPointDrone1[i].position, listFlyPointDrone1[i].rotation);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnSquidDestroyer0()
    {
        spawnedParticle = Instantiate(particleSpawnMothership, listSpawnDestroyer[0].position, particleSpawnMothership.gameObject.transform.rotation);
        spawnedParticle.transform.parent = particlesObject.transform;
        audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
        yield return new WaitForSeconds(0.75f);
        spawnedEnemy = Instantiate(prefabSquidDestroyer, listSpawnDestroyer[0].position, listSpawnDestroyer[0].rotation);
        spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
        spawnedEnemy.transform.parent = this.gameObject.transform;
        listEnemy.Add(spawnedEnemy);
        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator SpawnSquidDestroyer1()
    {
        spawnedParticle = Instantiate(particleSpawnMothership, listSpawnDestroyer[1].position, particleSpawnMothership.gameObject.transform.rotation);
        spawnedParticle.transform.parent = particlesObject.transform;
        audioSource.PlayOneShot(clipMS_Light[0], 1.0f);
        audioSource.PlayOneShot(clipMS_Light[1], 1.0f);
        yield return new WaitForSeconds(0.25f);
        audioSource.PlayOneShot(clipMS_Energy[0], 1.0f);
        audioSource.PlayOneShot(clipMS_Energy[1], 1.0f);
        audioSource.PlayOneShot(clipMS_Energy[2], 1.0f);
        yield return new WaitForSeconds(0.25f);
        audioSource.PlayOneShot(clipMS_Start[0], 1.0f);
        audioSource.PlayOneShot(clipMS_Start[1], 1.0f);
        audioSource.PlayOneShot(clipMS_Start[2], 1.0f);
        yield return new WaitForSeconds(1.25f);
        audioSource.PlayOneShot(clipMS_Blast[0], 1.0f);
        audioSource.PlayOneShot(clipMS_Blast[1], 1.0f);
        audioSource.PlayOneShot(clipMS_Blast[2], 1.0f);
        yield return new WaitForSeconds(1.00f);
        spawnedEnemy = Instantiate(prefabSquidDestroyer, listSpawnDestroyer[1].position, listSpawnDestroyer[1].rotation);
        spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
        spawnedEnemy.transform.parent = this.gameObject.transform;
        listEnemy.Add(spawnedEnemy);
        audioSource.PlayOneShot(clipMS_Echo[0], 1.0f);
        audioSource.PlayOneShot(clipMS_Echo[1], 1.0f);
        audioSource.PlayOneShot(clipMS_Echo[2], 1.0f);
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(clipMS_Feedback[0], 1.0f);
        audioSource.PlayOneShot(clipMS_Feedback[1], 1.0f);
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(clipMS_Gears[0], 1.0f);
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(clipMS_Gears[0], 1.0f);
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(clipMS_Gears[1], 1.0f);
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(clipMS_Device[0], 1.0f);
        yield return new WaitForSeconds(0.25f);
        audioSource.PlayOneShot(clipMS_Device[1], 1.0f);
        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator SpawnSquidDestroyer2()
    {
        spawnedParticle = Instantiate(particleSpawnMothership, listSpawnDestroyer[2].position, particleSpawnMothership.gameObject.transform.rotation);
        spawnedParticle.transform.parent = particlesObject.transform;
        audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
        yield return new WaitForSeconds(0.75f);
        spawnedEnemy = Instantiate(prefabSquidDestroyer, listSpawnDestroyer[2].position, listSpawnDestroyer[2].rotation);
        spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
        spawnedEnemy.transform.parent = this.gameObject.transform;
        listEnemy.Add(spawnedEnemy);
        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator SpawnSquidDestroyer3()
    {
        spawnedParticle = Instantiate(particleSpawnMothership, listSpawnDestroyer[3].position, particleSpawnMothership.gameObject.transform.rotation);
        spawnedParticle.transform.parent = particlesObject.transform;
        audioSource.PlayOneShot(clipTeleport[Random.Range(0, clipTeleport.Length)], 0.3f);
        yield return new WaitForSeconds(0.75f);
        spawnedEnemy = Instantiate(prefabSquidDestroyer, listSpawnDestroyer[3].position, listSpawnDestroyer[3].rotation);
        spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
        spawnedEnemy.transform.parent = this.gameObject.transform;
        listEnemy.Add(spawnedEnemy);
        yield return new WaitForSeconds(1.0f);
    }

    public void SpawnWingGroup0()
    {
        for (int i = 0; i < listSpawnWing0.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listSpawnWing0[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void SpawnWingGroup1()
    {
        for (int i = 0; i < listSpawnWing1.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listSpawnWing1[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void SpawnWingGroup2()
    {
        for (int i = 0; i < listSpawnWing2.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listSpawnWing2[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public void SpawnWingGroup3()
    {
        for (int i = 0; i < listSpawnWing3.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabWing, listSpawnWing3[i].position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
        }
    }

    public IEnumerator SpawnDroneGroup0()
    {
        for (int i = 0; i < listFlyPointDrone0.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabDrone, spawnDrone0.position, spawnDrone0.rotation);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
            spawnedEnemy.GetComponent<Enemy>().flyGroupNumber = 0;
            spawnedEnemy.GetComponent<Enemy>().flyPositionNumber = i;
            yield return new WaitForSeconds(1.0f);
        }
    }

    public IEnumerator SpawnDroneGroup1()
    {
        for (int i = 0; i < listFlyPointDrone1.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabDrone, spawnDrone1.position, spawnDrone0.rotation);
            spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
            spawnedEnemy.transform.parent = this.gameObject.transform;
            listEnemy.Add(spawnedEnemy);
            spawnedEnemy.GetComponent<Enemy>().flyGroupNumber = 1;
            spawnedEnemy.GetComponent<Enemy>().flyPositionNumber = i;
            yield return new WaitForSeconds(1.0f);
        }
    }

    public IEnumerator SpawnPunisher()
    {
        yield return new WaitForSeconds(1.0f);
        spawnedEnemy = Instantiate(prefabPunisher, spawnPunisher.position, spawnPunisher.rotation);
        spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
        spawnedEnemy.transform.parent = this.gameObject.transform;
        listEnemy.Add(spawnedEnemy);
    }

    public IEnumerator SpawnSquidShark()
    {
        yield return new WaitForSeconds(1.0f);
        spawnedEnemy = Instantiate(prefabSquidShark, spawnPunisher.position, spawnPunisher.rotation);
        spawnedEnemy.GetComponent<Enemy>().scriptEnemies = this;
        spawnedEnemy.transform.parent = this.gameObject.transform;
        listEnemy.Add(spawnedEnemy);
    }

    public void EnemyDestroyed()
    {
        audioSource.PlayOneShot(clipDestroyed[Random.Range(0, clipDestroyed.Length)], 0.15f);
        Destroy(enemyDestroyed);
    }

    public void AsteroidDestroyed()
    {
        audioSource.PlayOneShot(clipAsteroidDestroyed[Random.Range(0, clipAsteroidDestroyed.Length)], 0.15f);
    }

    public void ProtonDestroyed(GameObject destroyed)
    {
        audioSource.PlayOneShot(clipProtonDestroyed[Random.Range(0, clipProtonDestroyed.Length)], 0.3f);
        spawnedParticle = Instantiate(particleProtonDestroyed, destroyed.transform.position, destroyed.transform.rotation);
        spawnedParticle.gameObject.transform.parent = particlesObject.transform;
        listProton.Remove(destroyed);
        Destroy(destroyed);
    }

    public void PlasmaDestroyed(GameObject destroyed)
    {
        audioSource.PlayOneShot(clipPlasmaDestroyed[Random.Range(0, clipPlasmaDestroyed.Length)], 0.3f);
        spawnedParticle = Instantiate(particlePlasmaDestroyed, destroyed.transform.position, destroyed.transform.rotation);
        spawnedParticle.gameObject.transform.parent = particlesObject.transform;
        listPlasma.Remove(destroyed);
        Destroy(destroyed);
    }

    public void RocketDestroyed(GameObject destroyed)
    {
        audioSource.PlayOneShot(clipRocketDestroyed[Random.Range(0, clipRocketDestroyed.Length)], 0.6f);
        spawnedParticle = Instantiate(particleRocketDestroyed, destroyed.transform.position, destroyed.transform.rotation);
        spawnedParticle.gameObject.transform.parent = particlesObject.transform;
        listRocket.Remove(destroyed);
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
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "SquidFrigate")
                {
                    if (distanceTargetMove > minimumDistanceSquidFrigate)
                    {
                        listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrustSquidFrigate;
                        Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                        float singleStep = handlingSquidFrigate * Time.deltaTime;
                        Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                        listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "SquidProbe")
                {
                    if (distanceTargetMove > minimumDistanceSquidProbe)
                    {
                        Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                        float singleStep = handlingSquidProbe * Time.deltaTime;
                        Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                        listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "SquidDestroyer")
                {
                    if (distanceTargetMove > minimumDistanceSquidDestroyer)
                    {
                        listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrustSquidDestroyer;
                    }

                    Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                    float singleStep = handlingSquidDestroyer * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                    listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
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
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "Drone")
                {
                    if (listEnemy[i].GetComponent<Enemy>().flyGroupNumber == 0)
                    {
                        thisFlyPoint = listFlyPointDrone0[listEnemy[i].GetComponent<Enemy>().flyPositionNumber];
                        float step = thrustDrone * Time.deltaTime;
                        listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);

                        if (!listEnemy[i].GetComponent<Enemy>().isInFlyPosition)
                        {
                            distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                            if (distanceToFlyPoint <= 0.1f)
                            {
                                listEnemy[i].GetComponent<Enemy>().isInFlyPosition = true;
                            }
                        }
                        else
                        {
                            Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                            float singleStep = handlingDrone * Time.deltaTime;
                            Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                            listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
                        }
                    }
                    else if (listEnemy[i].GetComponent<Enemy>().flyGroupNumber == 1)
                    {
                        thisFlyPoint = listFlyPointDrone1[listEnemy[i].GetComponent<Enemy>().flyPositionNumber];
                        float step = thrustDrone * Time.deltaTime;
                        listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);

                        if (!listEnemy[i].GetComponent<Enemy>().isInFlyPosition)
                        {
                            distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                            if (distanceToFlyPoint <= 0.1f)
                            {
                                listEnemy[i].GetComponent<Enemy>().isInFlyPosition = true;
                            }
                        }
                        else
                        {
                            Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;
                            float singleStep = handlingDrone * Time.deltaTime;
                            Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                            listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
                        }
                    }
                }
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "Punisher")
                {
                    newTargetPosition = new Vector3(ship.transform.position.x, listEnemy[i].transform.position.y, ship.transform.position.z);
                    Vector3 targetDirection = newTargetPosition - listEnemy[i].transform.position;
                    float singleStep = handlingPunisher * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);
                    listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);

                    if (!listEnemy[i].GetComponent<Enemy>().isFlyPointEntrance)
                    {
                        if (!listEnemy[i].GetComponent<Enemy>().animatorPunisher.GetBool("Charge"))
                        {
                            listEnemy[i].GetComponent<Enemy>().animatorPunisher.SetBool("Charge", true);
                            thrustPunisher = 150.0f;
                        }

                        thisFlyPoint = flyPointPunisherEntrance;
                        float step = thrustPunisher * Time.deltaTime;
                        listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);
                        distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                        if (distanceToFlyPoint <= 0.1f)
                        {
                            listEnemy[i].GetComponent<Enemy>().animatorPunisher.SetBool("Charge", false);
                            listEnemy[i].GetComponent<Enemy>().isFlyPointEntrance = true;
                            thrustPunisher = 25.0f;
                            listEnemy[i].GetComponent<Enemy>().reloadTime = 5.0f;
                        }
                    }
                    else if (listEnemy[i].GetComponent<Enemy>().flyGroupNumber == 0)
                    {
                        if (listEnemy[i].GetComponent<Enemy>().flyPositionNumber == 0)
                        {
                            thisFlyPoint = listFlyPointPunisher0[listEnemy[i].GetComponent<Enemy>().flyPositionNumber];
                            float step = thrustPunisher * Time.deltaTime;
                            listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);
                            distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                            if (distanceToFlyPoint <= 0.1f)
                            {
                                listEnemy[i].GetComponent<Enemy>().flyPositionNumber = 1;
                            }
                        }
                        else if (listEnemy[i].GetComponent<Enemy>().flyPositionNumber == 1)
                        {
                            thisFlyPoint = listFlyPointPunisher0[listEnemy[i].GetComponent<Enemy>().flyPositionNumber];
                            float step = thrustPunisher * Time.deltaTime;
                            listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);
                            distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                            if (distanceToFlyPoint <= 0.1f)
                            {
                                listEnemy[i].GetComponent<Enemy>().flyPositionNumber = 0;
                            }
                        }
                    }
                }
                else if (listEnemy[i].GetComponent<Enemy>().enemyName == "SquidShark")
                {
                    if (!listEnemy[i].GetComponent<Enemy>().isFlyPointEntrance)
                    {
                        thisFlyPoint = listFlyPointSquidShark[0];
                        float step = thrustPunisher * Time.deltaTime;
                        listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);
                        distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                        if (distanceToFlyPoint <= 0.1f)
                        {
                            listEnemy[i].GetComponent<Enemy>().isFlyPointEntrance = true;
                            thrustSquidShark = 25.0f;
                            listEnemy[i].GetComponent<Enemy>().reloadTime = 5.0f;
                        }
                    }
                    else if (listEnemy[i].GetComponent<Enemy>().flyGroupNumber == 0)
                    {
                        if (listEnemy[i].GetComponent<Enemy>().flyPositionNumber == 0)
                        {
                            thisFlyPoint = listFlyPointSquidShark[1];
                            float step = thrustSquidShark * Time.deltaTime;
                            listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);
                            distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                            if (distanceToFlyPoint <= 0.1f)
                            {
                                listEnemy[i].GetComponent<Enemy>().flyPositionNumber = 1;
                            }
                        }
                        else if (listEnemy[i].GetComponent<Enemy>().flyPositionNumber == 1)
                        {
                            thisFlyPoint = listFlyPointSquidShark[2];
                            float step = thrustPunisher * Time.deltaTime;
                            listEnemy[i].transform.position = Vector3.MoveTowards(listEnemy[i].transform.position, thisFlyPoint.position, step);
                            distanceToFlyPoint = Vector3.Distance(listEnemy[i].transform.position, thisFlyPoint.position);

                            if (distanceToFlyPoint <= 0.1f)
                            {
                                listEnemy[i].GetComponent<Enemy>().flyPositionNumber = 0;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < listPlasma.Count; i++)
            {
                listPlasma[i].transform.position += listPlasma[i].transform.forward * Time.deltaTime * velocityPlasma;
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

            for (int i = 0; i < listRocket.Count; i++)
            {
                distanceTargetMove = Vector3.Distance(listRocket[i].transform.position, ship.transform.position);

                if (distanceTargetMove > minimumDistanceProton)
                {
                    listRocket[i].transform.position += listRocket[i].transform.forward * Time.deltaTime * velocityRocket;
                    Vector3 targetDirection = ship.transform.position - listRocket[i].transform.position;
                    float singleStep = targetingRocket * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(listRocket[i].transform.forward, targetDirection, singleStep, 0.0f);
                    listRocket[i].transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
        }
    }
}
