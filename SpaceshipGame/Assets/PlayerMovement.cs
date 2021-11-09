using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody rb;
    private Vector3 moveDirection;

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = -Input.GetAxisRaw("Horizontal");
        float moveZ = -Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, transform.position.y, moveZ);
    }

    void Move()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.z * moveSpeed);
    }
}
