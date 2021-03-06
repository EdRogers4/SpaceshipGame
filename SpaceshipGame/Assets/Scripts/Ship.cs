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
    public bool isBoosting;
    public bool isBarrelRoll;
    public bool isDead;
    public bool isDeadFighter;
    public bool isDeadBomber;
    public bool isDeadInterceptor;
    public bool isDeadBreaker;
    public bool isUpgradeFighter;
    public bool isUpgradeBomber;
    public bool isUpgradeInterceptor;
    public bool isUpgradeBreaker;
    public bool isPickupShield;
    public bool isFirstSelectShip;

    [Header("Movement")]
    public bool isKeyboard;
    public float moveSpeed;
    public Rigidbody rb;
    public VariableJoystick variableJoystick;
    private Vector3 moveDirection;
    private float moveX;
    private float moveZ;
    public float rotationLeft = 360;
    public float rotationspeed = 10;

    [Header("Stats")]
    public string shipName;
    public float pickupShieldAmount;
    public float pickupShieldCount;
    public float pickupShieldInterval;
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
    public float velocity;
    public float cooldown;
    public float targeting;
    public float blasters;
    public float boost;
    public float boostHigh;
    public float boostLow;
    public float boostAcceleration;
    public float boostDecceleration;
    public float boostMeter;
    public float boostMeterHigh;

    [Header("Scripts")]
    public Enemies scriptEnemies;
    public GameSettings scriptGameSettings;

    [Header("Lists")]
    public List<GameObject> listProjectiles;

    [Header("World")]
    public GameObject ship;
    public GameObject[] shipModel;
    public GameObject targetEnemy;
    public Transform[] pointShoot;
    public Vector3 targetMovePosition;
    public GameObject instances;
    public GameObject playerIndicator;
    public GameObject playerSpawnIndicator;
    public GameObject boarderCollisionEffect;
    private Vector3 effectStartPosition;
    public Animator animatorLevel;

    [Header("Effects")]
    public GameObject boostExhaustFighter;
    public GameObject boostExhaustBomber;
    public GameObject boostExhaustInterceptor;
    public GameObject boostExhaustBreaker;

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

    public GameObject projectileLaserGreen;
    public GameObject projectileNukeGreen;
    public GameObject projectileSniperGreen;
    public GameObject projectileLaserViolet;
    public GameObject projectileNukeViolet;
    public GameObject projectileSniperViolet;

    //Input
    public Vector3 previousPosition;

    [Header("Animations")]
    public Animator animatorCamera;
    public Animator animatorSelectShip;

    [Header("Sound")]
    private AudioSource audioSource;
    public AudioClip[] clipShootFighter;
    public AudioClip[] clipShootBomber;
    public AudioClip[] clipShootInterceptor;
    public AudioClip[] clipShootBreaker;
    public AudioClip[] clipShipDestroyed;
    public AudioClip[] clipShipWhoosh;
    public AudioClip[] clipShipBarrelRoll;
    public AudioClip clipSelectShip;
    public AudioClip clipShipSwap;
    public AudioClip clipShipBeep;
    public AudioClip clipWeaponUpgrade;
    public AudioClip[] clipShieldUpgrade;
    public bool[] isPlayLowHealth;

    [Header("Distance")]
    private float distanceEnemy;
    public float distanceEnemyShortest;
    private float distanceProton;
    private RaycastHit hitTapped;
    private RaycastHit hitAim;

    [Header("UI")]
    public Image shieldBar;
    public Image boostBar;
    public GameObject screenGameOver;
    public Image[] imageDead;

    [Header("Particles")]
    public ParticleSystem particleDestroyed;
    public ParticleSystem particleSwitchShip;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        thrustHigh = 0f;
        distanceEnemyShortest = 200f;
        startShieldFighter = 70.0f;
        startShieldBomber = 90.0f;
        startShieldInterceptor = 50.0f;
        startShieldBreaker = 120.0f;
        effectStartPosition = boarderCollisionEffect.transform.position;

        scriptGameSettings.animatorMessage.SetBool("isSelectShip", true);
        playerIndicator.SetActive(false);
        playerSpawnIndicator.SetActive(true);
        animatorLevel.speed = 0.0f;
        scriptGameSettings.audioSourceMusic.Pause();
        scriptGameSettings.isMusicPaused = true;

        StartCoroutine(TargetEnemy());
        StartCoroutine(TargetProton());
    }

    public IEnumerator CameraShakeBoost()
    {
        animatorCamera.SetBool("isBoost", true);
        yield return new WaitForSeconds(0.5f);
        animatorCamera.SetBool("isBoost", false);
    }

    public IEnumerator CameraShakeRandom()
    {
        animatorCamera.SetInteger("Shake", Random.Range(1, 3));
        yield return new WaitForSeconds(0.25f);
        animatorCamera.SetInteger("Shake", 0);
    }

    public void StartBarrelRoll()
    {
        if (!isFirstSelectShip)
        {
            return;
        }
        else if (!isBarrelRoll)
        {
            isBarrelRoll = true;
            gameObject.layer = 7;

            switch (shipName)
            {
                case "Fighter":
                    audioSource.PlayOneShot(clipShipBarrelRoll[0], 0.5f);
                    break;
                case "Bomber":
                    audioSource.PlayOneShot(clipShipBarrelRoll[1], 2.0f);
                    break;
                case "Interceptor":
                    audioSource.PlayOneShot(clipShipBarrelRoll[2], 0.75f);
                    break;
                case "Breaker":
                    audioSource.PlayOneShot(clipShipBarrelRoll[3], 0.25f);
                    break;
                default:
                    print("Not a ship 7");
                    break;
            }
        }
    }

    public void StartBoosting()
    {
        if (!isFirstSelectShip)
        {
            return;
        }
        else if (boostMeter >= boostMeterHigh)
        {
            isBoosting = true;

            switch (shipName)
            {
                case "Fighter":
                    boostExhaustFighter.SetActive(true);
                    audioSource.PlayOneShot(clipShipWhoosh[0], 1.0f);
                    break;
                case "Bomber":
                    boostExhaustBomber.SetActive(true);
                    audioSource.PlayOneShot(clipShipWhoosh[1], 2.0f);
                    break;
                case "Interceptor":
                    boostExhaustInterceptor.SetActive(true);
                    audioSource.PlayOneShot(clipShipWhoosh[2], 1.0f);
                    break;
                case "Breaker":
                    boostExhaustBreaker.SetActive(true);
                    audioSource.PlayOneShot(clipShipWhoosh[3], 1.0f);
                    break;
                default:
                    print("Not a ship 5");
                    break;
            }
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
                audioSource.PlayOneShot(clipShootFighter[0], 0.1f);

                var newProjectile2 = Instantiate(projectileFighter, barrelRightFighter.transform.position, barrelRightFighter.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile2);
                newProjectile2.GetComponent<Projectile>().scriptShip = this;
                newProjectile2.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile2.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootFighter[1], 0.1f);
            }
            else if (shipName == "Bomber")
            {
                var newProjectile1 = Instantiate(projectileBomber, barrelBomber.transform.position, barrelBomber.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile1);
                newProjectile1.GetComponent<Projectile>().scriptShip = this;
                newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile1.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootBomber[Random.Range(0, clipShootBomber.Length)], 0.5f);
            }
            else if (shipName == "Interceptor")
            {
                var newProjectile1 = Instantiate(projectileInterceptor, barrelLeftInterceptor.transform.position, barrelLeftInterceptor.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile1);
                newProjectile1.GetComponent<Projectile>().scriptShip = this;
                newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile1.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootInterceptor[Random.Range(0, clipShootInterceptor.Length)], 0.25f);

                var newProjectile2 = Instantiate(projectileInterceptor, barrelRightInterceptor.transform.position, barrelRightInterceptor.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile2);
                newProjectile2.GetComponent<Projectile>().scriptShip = this;
                newProjectile2.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile2.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShootInterceptor[Random.Range(0, clipShootInterceptor.Length)], 0.25f);
            }
            else if (shipName == "Breaker")
            {
                for (int i = 0; i <= pointShoot.Length; i++)
                {
                    var newProjectile1 = Instantiate(projectileBreaker, pointShoot[i].position, pointShoot[i].rotation) as GameObject;
                    listProjectiles.Add(newProjectile1);
                    newProjectile1.GetComponent<Projectile>().scriptShip = this;
                    newProjectile1.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                    newProjectile1.transform.parent = instances.transform;
                    newProjectile1.GetComponent<Rigidbody>().AddForce(newProjectile1.transform.right * velocity);
                    i++;
                }

                audioSource.PlayOneShot(clipShootBreaker[Random.Range(0, clipShootBreaker.Length)], 0.3f);
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
        if ((scriptGameSettings.isPause) || ((name == "Fighter" && shieldFighter <= 0f || (name == "Bomber" && shieldBomber <= 0f) || name == "Interceptor" && shieldInterceptor <= 0f) || (name == "Breaker" && shieldBreaker <= 0f)))
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

            if (!isFirstSelectShip)
            {
                isFirstSelectShip = true;
                animatorSelectShip.SetBool("isSelectShip", true);
                StartCoroutine(scriptGameSettings.AudioClipPlayGetReady());
            }

            shipName = name;
            particleDestroyed.gameObject.transform.parent = transform;
            particleDestroyed.gameObject.transform.position = transform.position;
            playerIndicator.SetActive(true);
            playerSpawnIndicator.SetActive(false);
            animatorLevel.speed = 1.0f;
            scriptGameSettings.animatorMessage.SetBool("isSelectShip", false);
            particleSwitchShip.transform.position = transform.position;
            particleSwitchShip.Play();
            audioSource.PlayOneShot(clipSelectShip, 0.55f);
            audioSource.PlayOneShot(clipShipSwap, 0.55f);
            audioSource.PlayOneShot(clipShipBeep, 0.55f);

            if (scriptGameSettings.isMusicPaused)
            {
                scriptGameSettings.audioSourceMusic.Play();
                scriptGameSettings.isMusicPaused = false;
            }

            switch (shipName)
            {
                case "Fighter":
                    shipModel[0].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldFighter / startShieldFighter;
                    thrustHigh = 60.0f;
                    acceleration = 4.0f;
                    decceleration = 4.0f;
                    handling = 400.0f;
                    boostHigh = 150f;
                    velocity = 200.0f;
                    targeting = 0.0f;
                    
                    if (!isUpgradeFighter)
                    {
                        cooldown = 0.125f;
                        blasters = 3.0f;   
                    }
                    else
                    {
                        cooldown = 0.1f;
                        blasters = 5.0f;  
                    }

                    break;
                case "Bomber":
                    shipModel[1].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldBomber / startShieldBomber;
                    thrustHigh = 40.0f;
                    acceleration = 4.0f;
                    decceleration = 4.0f;
                    handling = 600.0f;
                    boostHigh = 130f;
                    velocity = 80.0f;
                    targeting = 4.0f;

                    if (!isUpgradeBomber)
                    {
                        cooldown = 1.00f;
                        blasters = 7.0f;    
                    }
                    else
                    {
                        cooldown = 0.75f;
                        blasters = 15.0f;
                    }

                    break;
                case "Interceptor":
                    shipModel[2].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;
                    thrustHigh = 70.0f;
                    acceleration = 2.0f;
                    decceleration = 4.0f;
                    handling = 500.0f;
                    boostHigh = 200f;
                    velocity = 250.0f;
                    targeting = 10.0f;

                    if (!isUpgradeInterceptor)
                    {                 
                        cooldown = 0.15f;
                        blasters = 0.5f;                  
                    }
                    else
                    {
                        cooldown = 0.1f;
                        blasters = 1f;
                    }

                    break;
                case "Breaker":
                    shipModel[3].gameObject.SetActive(true);
                    shieldBar.fillAmount = shieldBreaker / startShieldBreaker;
                    thrustHigh = 50.0f;
                    acceleration = 1.0f;
                    decceleration = 4.0f;
                    handling = 300.0f;
                    boostHigh = 120f;
                    velocity = 200.0f;
                    targeting = 0.5f;

                    if (!isUpgradeBreaker)
                    { 
                        cooldown = 0.75f;
                        blasters = 2.0f; 
                    }
                    else
                    {
                        cooldown = 0.5f;
                        blasters = 3.0f;
                    }

                    break;
                default:
                    print("Not a ship 2");
                    break;
            }

            gameObject.layer = 11;
            boarderCollisionEffect.GetComponent<MeshRenderer>().enabled = true;
            thrust = thrustLow;
        }
    }

    public void ShootProjectileOn()
    {
        if ((!isFirstSelectShip) || ((shipName == "Fighter" && isDeadFighter) || (shipName == "Bomber" && isDeadBomber) || (shipName == "Interceptor" && isDeadInterceptor) || (shipName == "Breaker" && isDeadBreaker)))
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
        if (!scriptEnemies.isEnemyDestroyed)
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

                    if (shieldFighter <= 30f && !isPlayLowHealth[0])
                    {
                        isPlayLowHealth[0] = true;
                        StartCoroutine(scriptGameSettings.AudioClipPlayHealthLow(0));
                    }

                    break;
                case "Bomber":
                    shieldBomber -= amount;
                    shieldBar.fillAmount = shieldBomber / startShieldBomber;

                    if (shieldBomber <= 30f && !isPlayLowHealth[1])
                    {
                        isPlayLowHealth[1] = true;
                        StartCoroutine(scriptGameSettings.AudioClipPlayHealthLow(0));
                    }

                    break;
                case "Interceptor":
                    shieldInterceptor -= amount;
                    shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;

                    if (shieldInterceptor <= 30f && !isPlayLowHealth[2])
                    {
                        isPlayLowHealth[2] = true;
                        StartCoroutine(scriptGameSettings.AudioClipPlayHealthLow(0));
                    }

                    break;
                case "Breaker":
                    shieldBreaker -= amount;
                    shieldBar.fillAmount = shieldBreaker / startShieldBreaker;

                    if (shieldBreaker <= 30f && !isPlayLowHealth[3])
                    {
                        isPlayLowHealth[3] = true;
                        StartCoroutine(scriptGameSettings.AudioClipPlayHealthLow(0));
                    }

                    break;
                default:
                    print("Not a ship 3");
                    break;
            }

            if (shieldFighter <= 0f && shieldBomber <= 0f && shieldInterceptor <= 0f && shieldBreaker <= 0f)
            {
                isDead = true;
                screenGameOver.SetActive(true);
                StartCoroutine(scriptGameSettings.AudioClipPlayGameOver());
            }

            if ((shipName == "Fighter" && shieldFighter <= 0f) || (shipName == "Bomber" && shieldBomber <= 0f) || (shipName == "Interceptor" && shieldInterceptor <= 0f) || (shipName == "Breaker" && shieldBreaker <= 0f))
            {
                particleDestroyed.gameObject.transform.parent = instances.transform;
                particleDestroyed.Play();
                audioSource.PlayOneShot(clipShipDestroyed[Random.Range(0, clipShipDestroyed.Length)], 0.4f);
                gameObject.layer = 7;
                boarderCollisionEffect.GetComponent<MeshRenderer>().enabled = false;
                playerIndicator.SetActive(false);
                playerSpawnIndicator.SetActive(true);
                animatorLevel.speed = 0.0f;
                scriptGameSettings.audioSourceMusic.Pause();
                scriptGameSettings.isMusicPaused = true;
                StartCoroutine(scriptGameSettings.AudioClipPlayHealthLow(1));

                if (!isDead)
                {
                    scriptGameSettings.animatorMessage.SetBool("isSelectShip", true);
                }

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
                        shipModel[3].gameObject.SetActive(false);
                        isDeadBreaker = true;
                        imageDead[3].enabled = true;
                        break;
                    default:
                        print("Not a ship 4");
                        break;
                }
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

    void ProcessInputs()
    {
        if (isKeyboard)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveZ = Input.GetAxisRaw("Vertical");
        }
        else
        {
            moveX = -variableJoystick.Horizontal;
            moveZ = -variableJoystick.Vertical;
        }

        moveDirection = new Vector3(moveX, transform.position.y, moveZ).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector3(moveDirection.x * thrust, 0f, moveDirection.z * thrust);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, handling * Time.deltaTime);

            if (thrust < thrustHigh)
            {
                thrust += acceleration;
            }
        }
        else
        {
            if (thrust > thrustLow)
            {
                thrust -= decceleration;
                rb.velocity = transform.forward * thrust;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
        }

        if ((shipName == "Fighter" && shieldFighter > 0f) || (shipName == "Bomber" && shieldBomber > 0f) || (shipName == "Interceptor" && shieldInterceptor > 0f) || (shipName == "Breaker" && shieldBreaker > 0f))
        {
            if (isBoosting)
            {
                rb.velocity = transform.forward * boost;
            }
            if (!isBoosting && boostMeter < 30f)
            {
                rb.velocity = transform.forward * boost;
            }
        }

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
            ProcessInputs();
        }

        if ((shipName == "Fighter" && shieldFighter > 0f) || (shipName == "Bomber" && shieldBomber > 0f) || (shipName == "Interceptor" && shieldInterceptor > 0f) || (shipName == "Breaker" && shieldBreaker > 0f))
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

            if (Input.GetKeyDown("left shift"))
            {
                StartBoosting();
            }

            if (Input.GetKeyDown("q"))
            {
                StartBarrelRoll();
            }

            if (isPickupShield)
            {
                if (pickupShieldCount > 0f)
                {
                    switch (shipName)
                    {
                        case "Fighter":
                            shieldFighter += pickupShieldInterval;
                            if (shieldFighter > startShieldFighter)
                            {
                                shieldFighter = startShieldFighter;
                            }
                            shieldBar.fillAmount = shieldFighter / startShieldFighter;
                            break;
                        case "Bomber":
                            shieldBomber += pickupShieldInterval;
                            if (shieldBomber > startShieldBomber)
                            {
                                shieldBomber = startShieldBomber;
                            }
                            shieldBar.fillAmount = shieldBomber / startShieldBomber;
                            break;
                        case "Interceptor":
                            shieldInterceptor += pickupShieldInterval;
                            if (shieldInterceptor > startShieldInterceptor)
                            {
                                shieldInterceptor = startShieldInterceptor;
                            }
                            shieldBar.fillAmount = shieldInterceptor / startShieldInterceptor;
                            break;
                        case "Breaker":
                            shieldBreaker += pickupShieldInterval;
                            if (shieldBreaker > startShieldBreaker)
                            {
                                shieldBreaker = startShieldBreaker;
                            }
                            shieldBar.fillAmount = shieldBreaker / startShieldBreaker;
                            break;
                        default:
                            print("Not a ship 8");
                            break;
                    }

                    pickupShieldCount -= pickupShieldInterval;
                }
                else
                {
                    pickupShieldCount = pickupShieldAmount;
                    isPickupShield = false;
                }
            }

            if (isBarrelRoll)
            {
                float rotation = rotationspeed * Time.deltaTime;

                if (rotationLeft > rotation)
                {
                    rotationLeft -= rotation;
                }
                else
                {
                    rotation = rotationLeft;
                    rotationLeft = 360;
                    isBarrelRoll = false;
                    gameObject.layer = 11;
                }

                
                switch (shipName)
                {
                    case "Fighter":
                        shipModel[0].transform.Rotate(0, 0, rotation);
                        break;
                    case "Bomber":
                        shipModel[1].transform.Rotate(0, 0, rotation);
                        break;
                    case "Interceptor":
                        shipModel[2].transform.Rotate(0, 0, rotation);
                        break;
                    case "Breaker":
                        shipModel[3].transform.Rotate(0, 0, rotation);
                        break;
                    default:
                        print("Not a ship 6");
                        break;
                }
            }

            if (isBoosting && boostMeter > 0f)
            {
                boostMeter -= 1.1f;
                boostBar.fillAmount = boostMeter / boostMeterHigh;
            }
            else if (isBoosting && boostMeter <= 0f)
            {
                isBoosting = false;
                boostBar.color = new Color32(255, 0, 0, 100);
            }
            else if (!isBoosting && boostMeter < boostMeterHigh)
            {
                boostMeter += 0.51f;
                boostBar.fillAmount = boostMeter / boostMeterHigh;
            }
            else if (!isBoosting && boostMeter > boostMeterHigh)
            {
                boostMeter = boostMeterHigh;
                boostBar.fillAmount = boostMeter / boostMeterHigh;
                boostBar.color = new Color32(0, 255, 255, 150);

                switch (shipName)
                {
                    case "Fighter":
                        boostExhaustFighter.SetActive(false);
                        break;
                    case "Bomber":
                        boostExhaustBomber.SetActive(false);
                        break;
                    case "Interceptor":
                        boostExhaustInterceptor.SetActive(false);
                        break;
                    case "Breaker":
                        boostExhaustBreaker.SetActive(false);
                        break;
                    default:
                        print("Not a ship 5");
                        break;
                }
            }

            if (isBoosting)
            {
                if (boost < boostHigh)
                {
                    boost += boostAcceleration;
                }
                else if (boost > boostHigh)
                {
                    boost = boostHigh;
                }
            }
            else if (!isBoosting)
            {
                if (boost > 0f)
                {
                    boost -= boostDecceleration;
                }
                else if (boost < 0f)
                {
                    boost = 0f;
                }
            }
        }
        else if (isShooting)
        {
            isShoot = false;
        }
    }

    public IEnumerator PlayClipShieldUpgrade()
    {
        audioSource.PlayOneShot(clipShieldUpgrade[0], 1.0f);
        audioSource.PlayOneShot(clipShieldUpgrade[1], 1.0f);
        yield return new WaitForSeconds(0.7f);
        audioSource.PlayOneShot(clipShieldUpgrade[2], 1.0f);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Pickup")
        {
            if (collision.gameObject.GetComponent<Pickup>().isShield)
            {
                collision.gameObject.GetComponent<Pickup>().ShieldUpgrade();
                StartCoroutine(PlayClipShieldUpgrade());
                isPickupShield = true;
            }
            else if (collision.gameObject.GetComponent<Pickup>().isWeapon)
            {
                collision.gameObject.GetComponent<Pickup>().WeaponUpgrade();
                scriptGameSettings.AudioClipPlayUltimate();
                audioSource.PlayOneShot(clipWeaponUpgrade, 1.0f);

                switch (shipName)
                {
                    case "Fighter":
                        projectileFighter = projectileLaserViolet;
                        isUpgradeFighter = true;
                        cooldown = 0.1f;
                        blasters = 5.0f;
                        break;
                    case "Bomber":
                        projectileBomber = projectileNukeViolet;
                        isUpgradeBomber = true;
                        cooldown = 0.75f;
                        blasters = 15.0f;
                        break;
                    case "Interceptor":
                        projectileInterceptor = projectileSniperViolet;
                        isUpgradeInterceptor = true;
                        cooldown = 0.1f;
                        blasters = 1.0f;
                        break;
                    case "Breaker":
                        projectileBreaker = projectileLaserViolet;
                        isUpgradeBreaker = true;
                        cooldown = 0.5f;
                        blasters = 3.0f;
                        break;
                    default:
                        print("Not a ship 9");
                        break;
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Boarder")
        {
            boarderCollisionEffect.transform.position = collision.contacts[0].point;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Boarder")
        {
            boarderCollisionEffect.transform.position = effectStartPosition;
        }
    }
}
