using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour
{
    //Scripts
    public Enemies scriptEnemies;

    //Lists
    public List<GameObject> listProjectiles;

    //Game Objects, Vectors & Transforms
    public GameObject ship;
    public GameObject targetMove;
    public GameObject targetEnemy;
    public GameObject gunFront;
    public GameObject projectile;
    public Transform[] pointShoot;
    public Vector3 targetMovePosition;
    public GameObject instances;

    //Input
    private Vector3 previousPosition;

    //Bools
    public bool isMoving;
    public bool isTapped;
    public bool isShoot;

    //Stats
    public float shields;
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

    private AudioSource audioSource;
    public AudioClip clipShoot;

    //Distance
    private float distanceEnemy;
    public float distanceEnemyShortest;
    private float distanceTargetMove;
    private float minimumDistanceToTarget = 2.0f;
    private RaycastHit hitTapped;
    private RaycastHit hitAim;

    private void Start()
    {
        previousPosition = targetMove.transform.position;
        audioSource = gameObject.GetComponent<AudioSource>();
        distanceEnemyShortest = targeting;
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

    public IEnumerator ShootProjectile()
    {
        if (isShoot)
        {
            var newProjectile = Instantiate(projectile, gunFront.transform.position, gunFront.transform.rotation) as GameObject;
            listProjectiles.Add(newProjectile);
            newProjectile.GetComponent<Projectile>().scriptShip = this;
            newProjectile.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
            newProjectile.transform.parent = instances.transform;
            audioSource.PlayOneShot(clipShoot, 0.1f);
        }

        yield return new WaitForSeconds(cooldown);

        if (isShoot)
        {
            StartCoroutine(ShootProjectile());
        }
    }

    void Update()
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
            isTapped = true;
        }
        else
        {
            isMoving = false;
            isTapped = false;
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
                }

                if (handling < handlingHigh)
                {
                    handling += acceleration;
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
            ship.transform.position += transform.forward * Time.deltaTime * (thrust / 4f);

            if (thrust > thrustLow)
            {
                thrust -= decceleration;
            }

            if (handling > handlingLow)
            {
                handling -= decceleration;
            }

            Vector3 targetDirection = targetMovePosition - transform.position;
            float singleStep = handling * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
