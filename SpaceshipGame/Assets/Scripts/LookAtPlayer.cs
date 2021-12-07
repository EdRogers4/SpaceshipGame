using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject ship;
    public float speedRotate;

    void Update()
    {
        if (ship != null)
        {
            Vector3 targetDirection = ship.transform.position - transform.position;
            float singleStep = speedRotate * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
