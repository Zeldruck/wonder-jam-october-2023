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
    [SerializeField] private float gravityForce;
    [Header("Groundcheck")]
    [SerializeField] private Vector3 groundCheckPosition = new Vector3(0, -0.55f, 0);
    [SerializeField] private float groundCheckRadius = 0.48f;
    [SerializeField] private LayerMask groundMask;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.9f;
    [Header("Slide")]
    [SerializeField] private float slideInitialSpeedIncreaseMod = 1f;
    [SerializeField] private float slideCooldown = 1f;
    //TODO: timer for speed increase? to limit player from spamming sliding just for the speed
    [SerializeField] private float slideSpeedDecrease = 0.1f;
    [SerializeField] private float slideMinSpeed = 1f;
    [SerializeField] private float cameraSlideDropTime = 0.4f;
    [SerializeField] private Vector3 cameraSlideOffset = new Vector3(0, -0.5f, 0);
    private bool isSliding;
    private bool slideInput;
    private bool isDashing;
    private float dashCooldownTimer;
    private float slideCooldownTimer;
    private Vector2 movementInput;
    private bool jumpInput;
    private Vector3 direction;
    private Vector3 inputVelocity;
    private Vector3 extraVelocity;
    private int airJumpCount = 1;
    private int jumpCount;
    private float jumpCooldownTimer;
    private Rigidbody rb;
    private CapsuleCollider capsCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsCollider = GetComponent<CapsuleCollider>();
    }
    private void FixedUpdate()
    {

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
        if(slideCooldownTimer > 0)
            slideCooldownTimer -= Time.deltaTime;
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
        if (slideCooldownTimer <= 0 && movementInput.magnitude >= 0.2f && IsGrounded())
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
        else if (IsGrounded())
        {
            inputVelocity.y = 0;
            jumpCount = airJumpCount;
        }
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
        hits = Physics.OverlapSphere(groundCheckPosition + transform.position, groundCheckRadius, groundMask);
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
        slideCooldownTimer = slideCooldown;
        capsCollider.center = new Vector3(0, -0.5f, 0);
        capsCollider.height /= 2;
        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        direction = direction.x * transform.right + direction.z * transform.forward;
        Vector3 desiredVelocity = direction * slideInitialSpeedIncreaseMod * speed;
        float timer = 0;
        while (timer < cameraSlideDropTime)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, cameraSlideOffset, timer / cameraSlideDropTime);
            extraVelocity = Vector3.Lerp(extraVelocity, desiredVelocity, timer / cameraSlideDropTime);
            timer += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = cameraSlideOffset;
        extraVelocity = direction * slideInitialSpeedIncreaseMod * speed;

        while (slideInput && rb.velocity.magnitude > slideMinSpeed)
        {
            float reduction = (1 - extraSpeedDecrease * slideSpeedDecrease * Time.deltaTime / rb.velocity.magnitude);
            extraVelocity *= reduction;
            inputVelocity *= reduction;
            yield return null;
        }
        timer = 0;
        isSliding = false;
        while (timer < cameraSlideDropTime)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, Vector3.zero, timer / cameraSlideDropTime);
            timer += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = Vector3.zero;
        capsCollider.center = Vector3.zero;
        capsCollider.height *= 2;
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
