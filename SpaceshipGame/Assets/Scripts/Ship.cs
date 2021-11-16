using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    [Header("Bools")]
    public bool isMoving;
    public bool isShoot;
    public bool isShooting;
    public bool isDead;
    public bool isDeadFighter;
    public bool isDeadBomber;
    public bool isDeadInterceptor;
    public bool isDeadBreaker;

    [Header("Stats")]
    public string shipName;
    public float shieldFighter;
    public float shieldBomber;
    public float shieldInterceptor;
    public float shieldBreaker;
    private float startShieldFighter;
    private float startShieldBomber;
    private float startShieldInterceptor;
    private float startShieldBreaker;

    public float thrust;
    public float thrustHigh;
    public float thrustLow;
    public float acceleration;
    public float decceleration;
    public float handling;
    public float handlingHigh;
    public float handlingLow;
    public float velocity;
    public float cooldown;
    public float targeting;
    public float blasters;

    [Header("Scripts")]
    public Enemies scriptEnemies;
    public PlayerMovement scriptPlayerMovement;
    public GameSettings scriptGameSettings;

    [Header("Lists")]
    public List<GameObject> listProjectiles;

    [Header("World")]
    public GameObject ship;
    public GameObject[] shipModel;
    public GameObject targetMove;
    public GameObject targetEnemy;
    public Transform[] pointShoot;
    public Vector3 targetMovePosition;
    public GameObject instances;

    [Header("Barrels")]
    public GameObject barrelLeftFighter;
    public GameObject barrelRightFighter;
    public GameObject barrelBomber;
    public GameObject barrelLeftInterceptor;
    public GameObject barrelRightInterceptor;
    public GameObject barrelBreaker;

    [Header("Projectiles")]
    public GameObject projectileFighter;
    public GameObject projectileBomber;
    public GameObject projectileInterceptor;
    public GameObject projectileBreaker;

    [Header("Flak Cannon")]
    public int numberOfProjectiles;
    public float spreadAngle;
    List<Quaternion> pellets;

    private Vector3 startPoint;
    private const float radius = 1f;

    //Input
    public Vector3 previousPosition;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] clipShootFighter;
    public AudioClip[] clipShootBomber;
    public AudioClip[] clipShootInterceptor;
    public AudioClip[] clipShootBreaker;

    [Header("Distance")]
    private float distanceEnemy;
    public float distanceEnemyShortest;
    private float distanceProton;
    private float distanceTargetMove;
    private float minimumDistanceToTarget = 2.0f;
    private RaycastHit hitTapped;
    private RaycastHit hitAim;

    [Header("UI")]
    public Image shieldBar;
    public GameObject screenGameOver;
    public Image[] imageDead;

    [Header("Particles")]
    public ParticleSystem particleDestroyed;

    private void Start()
    {
        previousPosition = targetMove.transform.position;
        audioSource = gameObject.GetComponent<AudioSource>();
        distanceEnemyShortest = 200f;
        startShieldFighter = 70.0f;
        startShieldBomber = 120.0f;
        startShieldInterceptor = 50.0f;
        startShieldBreaker = 90.0f;
        acceleration = 10.0f;

        StartCoroutine(TargetEnemy());
        StartCoroutine(TargetProton());
    }

    private void Awake()
    {
        pellets = new List<Quaternion>(numberOfProjectiles);
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    public IEnumerator ShootProjectile()
    {
        if (isShoot && !isDead)
        {
            isShooting = true;

            if (shipName == "Fighter")
            {
                var newProjectile1 = Instantiate(projectileFighter, barrelLeftFighter.transform.position, barrelLeftFighter.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile1);
                newProjectile1.GetComponent<Projectile>().scriptShip = this;
                newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile1.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootFighter[0], 0.2f);

                var newProjectile2 = Instantiate(projectileFighter, barrelRightFighter.transform.position, barrelRightFighter.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile2);
                newProjectile2.GetComponent<Projectile>().scriptShip = this;
                newProjectile2.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile2.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootFighter[0], 0.2f);
            }
            else if (shipName == "Bomber")
            {
                var newProjectile1 = Instantiate(projectileBomber, barrelBomber.transform.position, barrelBomber.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile1);
                newProjectile1.GetComponent<Projectile>().scriptShip = this;
                newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile1.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootBomber[Random.Range(0, clipShootBomber.Length)], 0.6f);
            }
            else if (shipName == "Interceptor")
            {
                var newProjectile1 = Instantiate(projectileInterceptor, barrelLeftInterceptor.transform.position, barrelLeftInterceptor.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile1);
                newProjectile1.GetComponent<Projectile>().scriptShip = this;
                newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile1.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootInterceptor[Random.Range(0, clipShootInterceptor.Length)], 0.5f);

                var newProjectile2 = Instantiate(projectileInterceptor, barrelRightInterceptor.transform.position, barrelRightInterceptor.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile2);
                newProjectile2.GetComponent<Projectile>().scriptShip = this;
                newProjectile2.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile2.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootInterceptor[Random.Range(0, clipShootInterceptor.Length)], 0.5f);
            }
            else if (shipName == "Breaker")
            {
                for(int i = 0; i < numberOfProjectiles; i++)
                {
                    pellets[i] = new Quaternion(0f, Random.rotation.y, 0f, Random.rotation.w);
                    var newProjectile1 = Instantiate(projectileBreaker, barrelBreaker.transform.position, barrelBreaker.transform.rotation) as GameObject;
                    listProjectiles.Add(newProjectile1);
                    newProjectile1.GetComponent<Projectile>().scriptShip = this;
                    newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                    newProjectile1.transform.parent = instances.transform;
                    newProjectile1.transform.rotation = Quaternion.RotateTowards(newProjectile1.transform.rotation, pellets[i], spreadAngle);
                    newProjectile1.GetComponent<Rigidbody>().AddForce(newProjectile1.transform.right * velocity);
                    i++;
                }
                
                audioSource.PlayOneShot(clipShootBreaker[Random.Range(0, clipShootBreaker.Length)], 0.6f);
            }
        }

        yield return new WaitForSeconds(cooldown);
        isShooting = false;

        if (isShoot && !isDead)
        {
            StartCoroutine(ShootProjectile());
        }
        else
        {
            StopCoroutine(ShootProjectile());
        }
    }


    public void SwitchShip(string name)
    {
        if ((name == "Fighter" && shieldFighter <= 0f || (name == "Bomber" && shieldBomber <= 0f) || name == "Interceptor" && shieldInterceptor <= 0f) || (name == "Breaker" && shieldBreaker <= 0f))
        {
            return;
        }
        else
        {
            if (name != shipName)
            {
                for (int i = 0; i < listProjectiles.Count; i++)
                {
                    var thisProjectile = listProjectiles[i];
                    listProjectiles.Remove(listProjectiles[i]);
                    Destroy(thisProjectile);
                }
            }

                switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(false);
                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(false);
                    break;
                case "Interceptor":
                    shipModel[2].gameObject.SetActive(false);
                    break;
                case "Breaker":
                    shipModel[3].gameObject.SetActive(false);
                    break;
                default:
                    print("Not a ship 1");
                    break;
            }

            shipName = name;
            particleDestroyed.gameObject.transform.parent = transform;
            particleDestroyed.gameObject.transform.position = transform.position;

            switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldFighter / startShieldFighter;
                    thrustHigh = 60.0f;
                    scriptPlayerMovement.moveSpeed = 60.0f;
                    acceleration = 1.0f;
                    decceleration = 10.0f;
                    handlingHigh = 8.0f;
                    velocity = 200.0f;
                    cooldown = 0.1f;
                    blasters = 3.0f;
                    targeting = 0.0f;
                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldBomber / startShieldBomber;
                    thrustHigh = 40.0f;
                    scriptPlayerMovement.moveSpeed = 40.0f;
                    acceleration = 1.0f;
                    decceleration = 10.0f;
                    handlingHigh = 8.0f;
                    velocity = 80.0f;
                    cooldown = 1.0f;
                    blasters = 10.0f;
                    targeting = 4.0f;
                    break;
                case "Interceptor":
                    shipModel[2].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;
                    thrustHigh = 70.0f;
                    scriptPlayerMovement.moveSpeed = 70.0f;
                    acceleration = 2.0f;
                    decceleration = 10.0f;
                    handlingHigh = 8.0f;
                    velocity = 250.0f;
                    cooldown = 0.25f;
                    blasters = 1.0f;
                    targeting = 10.0f;
                    break;
                case "Breaker":
                    shipModel[3].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldBreaker / startShieldBreaker;
                    thrustHigh = 60.0f;
                    scriptPlayerMovement.moveSpeed = 60.0f;
                    acceleration = 1.0f;
                    decceleration = 10.0f;
                    handlingHigh = 8.0f;
                    velocity = 200.0f;
                    cooldown = 0.5f;
                    blasters = 5.0f;
                    targeting = 0.0f;
                    break;
                default:
                    print("Not a ship 2");
                    break;
            }

            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            thrust = thrustLow;
        }
    }

    public void ShootProjectileOn()
    {
        if ((shipName == "Fighter" && isDeadFighter) || (shipName == "Bomber" && isDeadBomber) || (shipName == "Interceptor" && isDeadInterceptor) || (shipName == "Breaker" && isDeadBreaker))
        {
            return;
        }
        else if (!isShoot && !isShooting)
        {
            isShoot = true;
            StartCoroutine(ShootProjectile());
        }
    }

    public void ShootProjectileOff()
    {
        isShoot = false;
        StopCoroutine(ShootProjectile());
    }

    public void TakeDamage(float amount)
    {
        if (scriptGameSettings.difficulty == 1)
        {
            amount = amount / 2.0f;
        }
        else if (scriptGameSettings.difficulty == 2)
        {
            amount = amount * 2.0f;
        }

        switch (shipName)
        {
            case "Fighter":
                shieldFighter -= amount;
                shieldBar.fillAmount = shieldFighter / startShieldFighter;
                break;
            case "Bomber":
                shieldBomber -= amount;
                shieldBar.fillAmount = shieldBomber / startShieldBomber;
                break;
            case "Interceptor":
                shieldInterceptor -= amount;
                shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;
                break;
            case "Breaker":
                shieldBreaker -= amount;
                shieldBar.fillAmount = shieldBreaker / startShieldBreaker;
                break;
            default:
                print("Not a ship 3");
                break;
        }

        if (shieldFighter <= 0f && shieldBomber <= 0f && shieldInterceptor <= 0f && shieldBreaker <= 0f)
        {
            isDead = true;
            screenGameOver.SetActive(true);
        }

        if ((shipName == "Fighter" && shieldFighter <= 0f) || (shipName == "Bomber" && shieldBomber <= 0f) || (shipName == "Interceptor" && shieldInterceptor <= 0f) || (shipName == "Breaker" && shieldBreaker <= 0f))
        {
            particleDestroyed.gameObject.transform.parent = instances.transform;
            particleDestroyed.Play();
            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(false);
                    isDeadFighter = true;
                    imageDead[0].enabled = true;
                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(false);
                    isDeadBomber = true;
                    imageDead[1].enabled = true;
                    break;
                case "Interceptor":
                    shipModel[2].gameObject.SetActive(false);
                    isDeadInterceptor = true;
                    imageDead[2].enabled = true;
                    break;
                case "Breaker":
                    shipModel[1].gameObject.SetActive(false);
                    isDeadBreaker = true;
                    imageDead[1].enabled = true;
                    break;
                default:
                    print("Not a ship 4");
                    break;
            }
        }
    }

    public IEnumerator TargetProton()
    {
        for (int i = 0; i < scriptEnemies.listProton.Count; i++)
        {
            distanceProton = Vector3.Distance(ship.transform.position, scriptEnemies.listProton[i].transform.position);

            if (distanceProton > 0 && distanceProton < 1000f)
            {
                if (distanceProton < distanceEnemyShortest)
                {
                    distanceEnemyShortest = distanceProton;
                    targetEnemy = scriptEnemies.listProton[i];
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        StartCoroutine(TargetProton());
    }

    public IEnumerator TargetEnemy()
    {
        for (int i = 0; i < scriptEnemies.listEnemy.Count; i++)
        {
            distanceEnemy = Vector3.Distance(ship.transform.position, scriptEnemies.listEnemy[i].transform.position);

            if (distanceEnemy > 0 && distanceEnemy < 1000f)
            {
                if (distanceEnemy < distanceEnemyShortest)
                {
                    distanceEnemyShortest = distanceEnemy;
                    targetEnemy = scriptEnemies.listEnemy[i];
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        StartCoroutine(TargetEnemy());
    }

    private void FixedUpdate()
    {
        if (listProjectiles.Count > 0)
        {
            for (int i = 0; i < listProjectiles.Count; i++)
            {
                if (listProjectiles[i] != null)
                {
                    listProjectiles[i].transform.position += listProjectiles[i].transform.forward * Time.deltaTime * velocity;
                }
            }
        }

        if (shipName == "Interceptor")
        {
            if (targetEnemy != null)
            {
                for (int i = 0; i < listProjectiles.Count; i++)
                {
                    //float step = targeting * Time.deltaTime;
                    //listProjectiles[i].transform.position = Vector3.MoveTowards(listProjectiles[i].transform.position, targetEnemy.transform.position, step);
                    Vector3 targetDirection = targetEnemy.transform.position - listProjectiles[i].transform.position;
                    float singleStep = targeting * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(listProjectiles[i].transform.forward, targetDirection, singleStep, 0.0f);
                    listProjectiles[i].transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown("space"))
            {
                if (!isShooting)
                {
                    ShootProjectileOn();
                }
            }
            else if (Input.GetKeyUp("space"))
            {
                ShootProjectileOff();
            }

            if (previousPosition != targetMove.transform.position)
            {
                isMoving = true;
            }
            else
            {
                //isMoving = false;
            }

            previousPosition = targetMove.transform.position;

            if (isMoving)
            {
                targetMovePosition = new Vector3(targetMove.transform.position.x, targetMove.transform.position.y, targetMove.transform.position.z);
                distanceTargetMove = Vector3.Distance(ship.transform.position, targetMovePosition);

                if (distanceTargetMove > minimumDistanceToTarget)
                {
                    ship.transform.position += transform.forward * Time.deltaTime * thrust;

                    if (thrust < thrustHigh)
                    {
                        thrust += acceleration;

                        if (thrust > thrustHigh)
                        {
                            thrust = thrustHigh;
                        }
                    }

                    if (handling < handlingHigh)
                    {
                        handling += acceleration;

                        if (handling > handlingHigh)
                        {
                            handling = handlingHigh;
                        }
                    }

                    Vector3 targetDirection = targetMovePosition - transform.position;
                    float singleStep = handling * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
                else if (distanceTargetMove <= minimumDistanceToTarget && isMoving)
                {
                    isMoving = false;
                }
            }
            else
            {
                ship.transform.position += transform.forward * Time.deltaTime * thrust;

                if (thrust > thrustLow)
                {
                    thrust -= decceleration;

                    if (thrust < thrustLow)
                    {
                        thrust = thrustLow;
                    }
                }

                if (handling > handlingLow)
                {
                    handling -= decceleration;

                    if (handling < handlingLow)
                    {
                        handling = handlingLow;
                    }
                }

                Vector3 targetDirection = targetMovePosition - transform.position;
                float singleStep = handling * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}
