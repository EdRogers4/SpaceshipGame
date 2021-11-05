using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    //Public Spawn Variables
    public List<GameObject> listEnemy;
    public List<Transform> listSpawnpoint;
    public GameObject prefabEnemy;

    //Private Spawn Variables
    private GameObject spawnedEnemy;

    //Enemy Targets
    public GameObject ship;

    //Enemy Stats
    public float thrust;
    public float handling;

    //Private
    public float distanceTargetMove;
    private float minimumDistanceToTarget = 2.0f;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < listSpawnpoint.Count; i++)
        {
            spawnedEnemy = Instantiate(prefabEnemy, listSpawnpoint[i].position, Quaternion.identity);
            listEnemy.Add(spawnedEnemy);
        }

        yield return new WaitForSeconds(15.0f);
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            //Get distance between the ship and the destination it is moving to
            distanceTargetMove = Vector3.Distance(listEnemy[i].transform.position, ship.transform.position);

            //If distance between the ship and its destination is greater than 
            if (distanceTargetMove > minimumDistanceToTarget)
            {
                //Move ship forward
                listEnemy[i].transform.position += listEnemy[i].transform.forward * Time.deltaTime * thrust;

                // Determine which direction to rotate towards
                Vector3 targetDirection = ship.transform.position - listEnemy[i].transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = handling * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(listEnemy[i].transform.forward, targetDirection, singleStep, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(listEnemy[i].transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                listEnemy[i].transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}
