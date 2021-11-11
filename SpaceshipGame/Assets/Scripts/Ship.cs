using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    [Header("Scripts")]
    public Enemies scriptEnemies;
    public PlayerMovement scriptPlayerMovement;

    [Header("Lists")]
    public List<GameObject> listProjectiles;

    [Header("World")]
    public GameObject ship;
    public GameObject[] shipModel;
    public GameObject targetMove;
    public GameObject targetEnemy;
    public GameObject gunFront;
    public GameObject projectile;
    public Transform[] pointShoot;
    public Vector3 targetMovePosition;
    public GameObject instances;

    //Input
    private Vector3 previousPosition;

    [Header("Bools")]
    public bool isMoving;
    public bool isShoot;
    public bool isDead;
    public bool isDeadFighter;
    public bool isDeadBomber;
    public bool isDeadVanguard;
    public bool isDeadScout;
    public bool isDeadBreaker;
    public bool isDeadInterceptor;

    [Header("Stats")]
    public string shipName;
    public float shieldFighter;
    public float shieldBomber;
    public float shieldVanguard;
    public float shieldScout;
    public float shieldBreaker;
    public float shieldInterceptor;
    private float startShieldFighter;
    private float startShieldBomber;
    private float startShieldVanguard;
    private float startShieldScout;
    private float startShieldBreaker;
    private float startShieldInterceptor;

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

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip clipShoot;

    [Header("Distance")]
    private float distanceEnemy;
    public float distanceEnemyShortest;
    private float distanceTargetMove;
    private float minimumDistanceToTarget = 2.0f;
    private RaycastHit hitTapped;
    private RaycastHit hitAim;

    [Header("UI")]
    public Image shieldBar;
    public GameObject screenGameOver;

    [Header("Particles")]
    public ParticleSystem particleDestroyed;

    private void Start()
    {
        previousPosition = targetMove.transform.position;
        audioSource = gameObject.GetComponent<AudioSource>();
        distanceEnemyShortest = 200f;
        startShieldFighter = 60.0f;
        startShieldBomber = 80.0f;
        startShieldVanguard = 70.0f;
        startShieldScout = 50.0f;
        startShieldBreaker = 90.0f;
        startShieldInterceptor = 40.0f;
    }

    public void SwitchShip(string name)
    {
        if ((name == "Fighter" && shieldFighter <= 0f) || (name == "Bomber" && shieldBomber <= 0f) || (name == "Vanguard" && shieldVanguard <= 0f) ||
            (name == "Scout" && shieldScout <= 0f) || (name == "Breaker" && shieldBreaker <= 0f) || (name == "Interceptor" && shieldInterceptor <= 0f))
        {
            return;
        }
        else
        {
            switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(false);
                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(false);
                    break;
                case "Vanguard":
                    shipModel[2].gameObject.SetActive(false);
                    break;
                case "Scout":
                    shipModel[3].gameObject.SetActive(false);
                    break;
                case "Breaker":
                    shipModel[4].gameObject.SetActive(false);
                    break;
                case "Interceptor":
                    shipModel[5].gameObject.SetActive(false);
                    break;
                default:
                    print("Not a ship");
                    break;
            }

            shipName = name;

            switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldFighter / startShieldFighter;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    thrustHigh = 60.0f;
                    scriptPlayerMovement.moveSpeed = 60.0f;
                    acceleration = 1.0f;
                    decceleration = 0.5f;
                    handlingHigh = 4.0f;
                    velocity = 200.0f;
                    cooldown = 0.1f;
                    blasters = 3.0f;
                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldBomber / startShieldBomber;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    thrustHigh = 30.0f;
                    scriptPlayerMovement.moveSpeed = 30.0f;
                    acceleration = 2.0f;
                    decceleration = 1.0f;
                    handlingHigh = 8.0f;
                    velocity = 50.0f;
                    cooldown = 0.5f;
                    blasters = 8.0f;
                    break;
                case "Vanguard":
                    shipModel[2].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldVanguard / startShieldVanguard;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    thrustHigh = 90.0f;
                    scriptPlayerMovement.moveSpeed = 90.0f;
                    acceleration = 10.0f;
                    decceleration = 10.0f;
                    handlingHigh = 4.0f;
                    velocity = 200.0f;
                    cooldown = 0.5f;
                    blasters = 5.0f;
                    break;
                case "Scout":
                    shipModel[3].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldScout / startShieldScout;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    thrustHigh = 60.0f;
                    scriptPlayerMovement.moveSpeed = 60.0f;
                    acceleration = 2.0f;
                    decceleration = 6.0f;
                    handlingHigh = 2.0f;
                    velocity = 125.0f;
                    cooldown = 1.0f;
                    blasters = 5.0f;
                    break;
                case "Breaker":
                    shipModel[4].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldBreaker / startShieldBreaker;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    thrustHigh = 30.0f;
                    scriptPlayerMovement.moveSpeed = 30.0f;
                    acceleration = 1.0f;
                    decceleration = 0.5f;
                    handlingHigh = 2.0f;
                    velocity = 50.0f;
                    cooldown = 1.0f;
                    blasters = 10.0f;
                    break;
                case "Interceptor":
                    shipModel[5].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    thrustHigh = 120.0f;
                    scriptPlayerMovement.moveSpeed = 120.0f;
                    acceleration = 10.0f;
                    decceleration = 10.0f;
                    handlingHigh = 8.0f;
                    velocity = 125.0f;
                    cooldown = 0.1f;
                    blasters = 1.0f;
                    break;
                default:
                    print("Not a ship");
                    break;
            }
        }
    }

    public void ShootProjectileOn()
    {
        if (!isShoot)
        {
            isShoot = true;
            StopCoroutine(ShootProjectile());
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
            case "Vanguard":
                shieldVanguard -= amount;
                shieldBar.fillAmount = shieldVanguard / startShieldVanguard;
                break;
            case "Scout":
                shieldScout -= amount;
                shieldBar.fillAmount = shieldScout / startShieldScout;
                break;
            case "Breaker":
                shieldBreaker -= amount;
                shieldBar.fillAmount = shieldBreaker / startShieldBreaker;
                break;
            case "Interceptor":
                shieldInterceptor -= amount;
                shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;
                break;
            default:
                print("Not a ship");
                break;
        }

        if (shieldFighter <= 0f && shieldBomber <= 0f && shieldVanguard <= 0f && shieldScout <= 0f && shieldBreaker <= 0f && shieldInterceptor <= 0f)
        {
            isDead = true;
            screenGameOver.SetActive(true);
        }

        if ((shipName == "Fighter" && shieldFighter <= 0f) || (shipName == "Bomber" && shieldBomber <= 0f) || (shipName == "Vanguard" && shieldVanguard <= 0f) ||
        (shipName == "Scout" && shieldScout <= 0f) || (shipName == "Breaker" && shieldBreaker <= 0f) || (shipName == "Interceptor" && shieldInterceptor <= 0f))
        {
            particleDestroyed.Play();
            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(false);
                    isDeadFighter = true;
                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(false);
                    isDeadBomber = true;
                    break;
                case "Vanguard":
                    shipModel[2].gameObject.SetActive(false);
                    isDeadVanguard = true;
                    break;
                case "Scout":
                    shipModel[3].gameObject.SetActive(false);
                    isDeadScout = true;
                    break;
                case "Breaker":
                    shipModel[4].gameObject.SetActive(false);
                    isDeadBreaker = true;
                    break;
                case "Interceptor":
                    shipModel[5].gameObject.SetActive(false);
                    isDeadInterceptor = true;
                    break;
                default:
                    print("Not a ship");
                    break;
            }
        }
    }

    public IEnumerator ShootProjectile()
    {
        if (isShoot && !isDead)
        {
            var newProjectile = Instantiate(projectile, gunFront.transform.position, gunFront.transform.rotation) as GameObject;
            listProjectiles.Add(newProjectile);
            newProjectile.GetComponent<Projectile>().scriptShip = this;
            newProjectile.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
            newProjectile.transform.parent = instances.transform;
            audioSource.PlayOneShot(clipShoot, 0.1f);
        }

        yield return new WaitForSeconds(cooldown);

        if (isShoot && !isDead)
        {
            StartCoroutine(ShootProjectile());
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown("space"))
            {
                ShootProjectileOn();
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
                isMoving = false;
            }

            previousPosition = targetMove.transform.position;

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
