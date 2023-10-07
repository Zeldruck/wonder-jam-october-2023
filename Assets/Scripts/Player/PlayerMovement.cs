using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float speed = 7;
    [SerializeField] private float extraSpeedDecrease = 3;
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldownReset = 0.2f;
    [SerializeField] private Vector3 groundCheckPosition = new Vector3(0, -0.55f, 0);
    [SerializeField] private float groundCheckRadius = 0.48f;
    [SerializeField] private float gravityForce;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.9f;
    [Header("Slide")]
    [SerializeField] private float slideInitialSpeedIncreaseMod = 1f;
    //TODO: timer for speed increase? to limit player from spamming sliding just for the speed
    [SerializeField] private float slideSpeedDecrease = 0.1f;
    [SerializeField] private float slideRotation = 90f;
    [SerializeField] private float slideMinSpeed = 1f;
    private bool isSliding;
    private bool slideInput;
    private bool isDashing;
    private float dashCooldownTimer;
    private Vector2 movementInput;
    private bool jumpInput;
    private Vector3 direction;
    private Vector3 inputVelocity;
    private Vector3 extraVelocity;
    private int airJumpCount = 1;
    private int jumpCount;
    private float jumpCooldownTimer;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        inputVelocity = rb.velocity;

        if (!isDashing && !isSliding)
        {
            BuildHorizontalMovement();
            BuildVerticalMovement();
            FollowCameraRotation();
            if (IsGrounded())
            {
                if (extraVelocity.magnitude > 0.2f)
                {
                    float reduction = (1 - extraSpeedDecrease * Time.fixedDeltaTime / extraVelocity.magnitude);
                    extraVelocity *= reduction;
                }
                else extraVelocity = Vector3.zero;
            }
        }
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
        rb.velocity = inputVelocity + extraVelocity;
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
        if (movementInput.magnitude >= 0.2f && IsGrounded())
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
            direction = direction.x * transform.right + direction.z * transform.forward;
            direction *= speed;
        }
        else
        {
            direction = Vector3.zero;
            inputVelocity.x = 0;
            inputVelocity.z = 0;
            extraVelocity.x = 0;
            extraVelocity.z = 0;
        }
        inputVelocity.x = direction.x;
        inputVelocity.z = direction.z;

    }

    private void BuildVerticalMovement()
    {
        if (jumpInput && jumpCooldownTimer <= 0)
        {
            if (IsGrounded())
                PerformJump();
            else if (jumpCount > 0)
            {
                PerformJump();
                jumpCount--;
            }
        }

        if (IsGrounded())
            jumpCount = airJumpCount;
        else
            inputVelocity.y -= gravityForce * Time.fixedDeltaTime;

    }


    private void FollowCameraRotation()
    {
        Vector3 rotation = Camera.main.transform.rotation.eulerAngles;
        rotation.x = 0f;
        transform.rotation = Quaternion.Euler(rotation);

    }

    private bool IsGrounded()
    {
        Collider[] hits;
        hits = Physics.OverlapSphere(groundCheckPosition + transform.position, groundCheckRadius);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }


    private void PerformJump()
    {
        jumpCooldownTimer = jumpCooldownReset;
        inputVelocity.y = jumpForce;
    }

    private IEnumerator PerformSlide()
    {
        isSliding = true;
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
        isSliding = false;
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown + dashTime;

        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        direction = direction.x * transform.right + direction.z * transform.forward;
        extraVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}
