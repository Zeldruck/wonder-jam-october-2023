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
    [SerializeField] private float dashCooldown = 0.9f;
    [Header("Slide")]
    [SerializeField] private float slideInitialSpeedIncreaseMod = 1f;
    //timer for speed increase? to limit player from spamming sliding just for the speed
    [SerializeField] private float slideSpeedDecrease = 0.1f;
    [SerializeField] private float slideRotation = 90f;
    [SerializeField] private float slideMinSpeed = 1f;
    private bool isSliding;
    private bool slideInput;
    private bool isDashing;
    private float dashCooldownTimer;
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
        if (!isDashing && !isSliding)
        {
            BuildHorizontalMovement();
            BuildVerticalMovement();
            FollowCameraRotation();
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

    public void AttemptSlide()
    {
        if (movementInput.magnitude >= 0.2f && charaController.isGrounded)
            StartCoroutine(PerformSlide());
    }

    public void SetSlideValue(bool value)
    {
        slideInput = value;
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

    private void FollowCameraRotation()
    {
        Vector3 rotation = Camera.main.transform.rotation.eulerAngles;
        rotation.x = 0f;
        transform.rotation = Quaternion.Euler(rotation);

    }

    private void PerformJump()
    {
        jumpCooldownTimer = jumpCooldownReset;
        verticalMovement = jumpForce;
    }

    private IEnumerator PerformSlide()
    {
        isSliding = true;
        charaController.enabled = false;
        rb.useGravity = true;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(-slideRotation, currentRotation.y, currentRotation.z);
        transform.rotation = rotation;

        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

        float tempSpeed = slideInitialSpeedIncreaseMod * speed;
        Vector3 newVelocity = Vector3.zero;
        newVelocity.x = moveDirection.x * tempSpeed;
        newVelocity.z = moveDirection.z * tempSpeed;
        rb.velocity = newVelocity;
        while (slideInput && rb.velocity.magnitude > slideMinSpeed)
        {
            tempSpeed -= slideSpeedDecrease * Time.deltaTime;
            newVelocity = rb.velocity;
            newVelocity.x = moveDirection.x * tempSpeed;
            newVelocity.z = moveDirection.z * tempSpeed;
            rb.velocity = newVelocity;
            yield return null;
        }
        currentRotation = transform.rotation.eulerAngles;
        rotation = Quaternion.Euler(0, currentRotation.y, currentRotation.z);
        transform.rotation = rotation;
        charaController.enabled = true;
        rb.useGravity = false;
        isSliding = false;
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown + dashTime;
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
