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

    //Bools
    public bool isMoving;
    public bool isTapped;
    public bool isShoot;

    //Stats
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

    public AudioSource audioSource;
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
        audioSource = gameObject.GetComponent<AudioSource>();
        distanceEnemyShortest = targeting;
        StartCoroutine(TargetEnemy());
        //StartCoroutine(Shoot());
    }

    /*
    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(cooldown);

        for (int i = 0; i < pointShoot.Length; i++)
        {
            if (Physics.Raycast(pointShoot[i].position, pointShoot[i].TransformDirection(Vector3.forward), out hitAim, targeting))
            {
                if (hitAim.transform.tag == "Enemy" && targetEnemy != null)
                {
                    var newProjectile = Instantiate(projectile, gunFront.transform.position, gunFront.transform.rotation) as GameObject;
                    listProjectiles.Add(newProjectile);
                    newProjectile.GetComponent<Projectile>().scriptShip = this;
                    newProjectile.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                    newProjectile.transform.parent = instances.transform;
                    audioSource.PlayOneShot(clipShoot, 0.1f);
                }
            }
        }

        StartCoroutine(Shoot());
    }
    */

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

    public IEnumerator TargetEnemy()
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < scriptEnemies.listEnemy.Count; i++)
        {
            distanceEnemy = Vector3.Distance(ship.transform.position, scriptEnemies.listEnemy[i].transform.position);

            if (distanceEnemy > 0 && distanceEnemy < targeting)
            {
                if (distanceEnemy <= distanceEnemyShortest)
                {
                    distanceEnemyShortest = distanceEnemy;
                    targetEnemy = scriptEnemies.listEnemy[i];
                }
            }
        }

        StartCoroutine(TargetEnemy());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            isMoving = true;
            isTapped = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTapped = false;
        }

        if (listProjectiles.Count > 0)
        {
            for (int i = 0; i < listProjectiles.Count; i++)
            {
                if (listProjectiles[i] != null)
                {
                    listProjectiles[i].transform.position += listProjectiles[i].transform.up * Time.deltaTime * velocity;
                }
            }
        }

        if (isMoving)
        {
            if (isTapped)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitTapped, 1000))
                {
                    targetMovePosition = new Vector3(hitTapped.point.x, targetMove.transform.position.y, hitTapped.point.z);
                    targetMove.transform.position = targetMovePosition;
                }
            }
            
            distanceTargetMove = Vector3.Distance(ship.transform.position, targetMovePosition);

            if (distanceTargetMove > minimumDistanceToTarget)
            {
                ship.transform.position += transform.forward * Time.deltaTime * thrust;

                if (thrust < thrustHigh)
                {
                    thrust += acceleration;
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

            /*
            Vector3 targetDirection = targetEnemy.transform.position - transform.position;
            float singleStep = handling * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            */
        }
    }
}
