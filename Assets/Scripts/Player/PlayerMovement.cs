using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float verticalMovement;
    private Vector2 movementInput;
    private bool jumpInput;
    private Vector3 direction;
    private CharacterController charaController;

    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
        charaController.detectCollisions = false;
    }

    private void Update()
    {
        BuildHorizontalMovement();
        BuildVerticalMovement();
        charaController.Move(direction);
    }

    public void SetInputValue(Vector2 input)
    {
        movementInput = input;

    }

    public void SetJumpValue(bool input)
    {
        jumpInput = input;
    }

    private void BuildHorizontalMovement()
    {
        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (direction.magnitude >= 0.2f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;


            Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            direction.x = moveDirection.x * speed * Time.deltaTime;
            direction.z = moveDirection.z * speed * Time.deltaTime;
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    private void BuildVerticalMovement()
    {
        if (jumpInput)
        {
            if (charaController.isGrounded)
            {
                verticalMovement = jumpForce;
            }
        }
        else if (charaController.isGrounded)
            verticalMovement = 0;

        if (!charaController.isGrounded)
            verticalMovement += Physics.gravity.y * Time.deltaTime;

        direction.y = verticalMovement * Time.deltaTime;
    }


}
