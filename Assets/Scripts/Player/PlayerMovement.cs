using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float DASH_COOLDOWN = 0.9f;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashCooldownTimer;

    private float verticalMovement;
    private Vector2 movementInput;
    private bool jumpInput;
    private Vector3 direction;
    private int airJumpCount = 1;
    private int jumpCount;
    private float jumpCooldownReset = 0.3f;
    private float jumpCooldownTimer;
    private CharacterController charaController;
    private Rigidbody rb;

    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        charaController.detectCollisions = false;
    }
    private void Update()
    {
        if (!isDashing)
        {
            BuildHorizontalMovement();
            BuildVerticalMovement();
            charaController.Move(direction);
        }
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
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

    public void AttemptDash()
    {
        if (dashCooldownTimer <= 0 && movementInput.magnitude >= 0.2f)
            StartCoroutine(PerformDash());
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

    private IEnumerator PerformDash()
    {
        isDashing = true;
        dashCooldownTimer = DASH_COOLDOWN + dashTime;
        charaController.enabled = false;
        rb.velocity = Vector3.zero;

        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

        Vector3 newVelocity = Vector3.zero;
        newVelocity.x = moveDirection.x * dashSpeed;
        newVelocity.z = moveDirection.z * dashSpeed;
        rb.velocity = newVelocity;

        yield return new WaitForSeconds(dashTime);
        charaController.enabled = true;
        isDashing = false;
    }
}
