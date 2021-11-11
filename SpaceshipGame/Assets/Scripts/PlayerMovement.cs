using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isKeyboard;
    public float moveSpeed;
    public Rigidbody rb;
    public VariableJoystick variableJoystick;
    public Ship scriptShip;

    private Vector3 moveDirection;
    private float moveX;
    private float moveZ;

    void Update()
    {
        if (!scriptShip.isDead)
        {
            ProcessInputs();
        }
    }

    void FixedUpdate()
    {
        if (!scriptShip.isDead)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        if (isKeyboard)
        {
            moveX = -Input.GetAxisRaw("Horizontal");
            moveZ = -Input.GetAxisRaw("Vertical");
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
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.z * moveSpeed);
    }
}
