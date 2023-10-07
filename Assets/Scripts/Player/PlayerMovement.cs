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
    [SerializeField] private int airJumpCount = 1;
    [SerializeField] private int jumpCount;
    private float jumpCooldownReset = 0.3f;
    [SerializeField] private float jumpCooldownTimer;

    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
        charaController.detectCollisions = false;
    }
    private void FixedUpdate()
    {
        BuildHorizontalMovement();
        BuildVerticalMovement();
        charaController.Move(direction);

    }
    private void Update()
    {
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;
    }

    public void SetInputValue(Vector2 input)
    {
        movementInput = input;

    }

    public void SetJumpValue(bool input)
    {
        jumpInput = input;
    }

    public void SetAirJumpCount(int value)
    {
        airJumpCount = value;
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
        if (jumpInput && jumpCooldownTimer <= 0)
        {
            if (charaController.isGrounded)
            {
                PerformJump();
            }
            else if (jumpCount > 0)
            {
                PerformJump();
                jumpCount--;
            }
        }
        if (charaController.isGrounded)
        {
            //verticalMovement = Physics.gravity.y * 0.1f;
            jumpCount = airJumpCount;
        }

        if (!charaController.isGrounded)
            verticalMovement += Physics.gravity.y * Time.deltaTime;

        direction.y = verticalMovement * Time.deltaTime;
    }

    private void PerformJump()
    {
        jumpCooldownTimer = jumpCooldownReset;
        verticalMovement = jumpForce;
    }
}
