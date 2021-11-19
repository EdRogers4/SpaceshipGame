using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public bool isRotateY;

    void Update()
    {
        if (isRotateY)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
