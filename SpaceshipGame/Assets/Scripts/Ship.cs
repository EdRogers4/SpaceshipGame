using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //Scripts
    public Enemies scriptEnemies;

    //Lists
    public List<GameObject> listProjectiles;

    //Game Objects, Vectors & Transforms
    public GameObject ship;
    public Transform targetMove;
    public GameObject gunFront;
    public GameObject projectile;
    public Transform[] pointShoot;
    public Vector3 targetMovePosition;

    //Bools
    public bool isMoving;
    public bool isTapped;

    //Stats
    public float thrust;
    public float handling;
    public float velocity;
    public float cooldown;

    //Private
    private float distanceTargetMove;
    private float minimumDistanceToTarget = 2.0f;
    private RaycastHit hitTapped;
    private RaycastHit hitAim;

    private void Start()
    {
        StartCoroutine(Shoot());
        Debug.Log("Point Shoot length: " + pointShoot.Length);
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(cooldown);

        for (int i = 0; i < pointShoot.Length; i++)
        {
            if (Physics.Raycast(pointShoot[i].position, pointShoot[i].TransformDirection(Vector3.forward), out hitAim, 200f))
            {
                if (hitAim.transform.tag == "Enemy")
                {
                    var newProjectile = Instantiate(projectile, gunFront.transform.position, gunFront.transform.rotation) as GameObject;
                    listProjectiles.Add(newProjectile);
                    newProjectile.GetComponent<Projectile>().scriptShip = this;
                }
                else
                {
                    Debug.Log("Hit: " + hitAim.transform.name);
                }
            }
        }

        StartCoroutine(Shoot());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
                //Find tapped area on screen
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitTapped, 1000))
                {
                    targetMovePosition = new Vector3(hitTapped.point.x, targetMove.position.y, hitTapped.point.z);
                    targetMove.position = targetMovePosition;
                }
            }
            
            //Get distance between the ship and the destination it is moving to
            distanceTargetMove = Vector3.Distance(ship.transform.position, targetMovePosition);

            //If distance between the ship and its destination is greater than 
            if (distanceTargetMove > minimumDistanceToTarget)
            {
                //Move ship forward
                ship.transform.position += transform.forward * Time.deltaTime * thrust;

                // Determine which direction to rotate towards
                Vector3 targetDirection = targetMovePosition - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = handling * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else if (distanceTargetMove <= minimumDistanceToTarget && isMoving)
            {
                isMoving = false;
            }
        }
    }
}
