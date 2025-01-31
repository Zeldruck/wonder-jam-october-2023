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
    [SerializeField] private float slideSpeedDecrease = 0.1f;
    [SerializeField] private float slideMinSpeed = 1f;
    [SerializeField] private float cameraSlideDropTime = 0.4f;
    [SerializeField] private Vector3 cameraSlideOffset = new Vector3(0, -0.5f, 0);
    [SerializeField] private bool canSlide; //TODO: remove serialize field
    [SerializeField] private bool canDash; //TODO: remove serialize field
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
    private int airJumpCount = 0;
    private int jumpCount;
    private float jumpCooldownTimer;
    private Rigidbody rb;
    private CapsuleCollider capsCollider;
    private Player player;

    private bool hasInputEnabled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsCollider = GetComponent<CapsuleCollider>();
        player = GetComponent<Player>();

        hasInputEnabled = false;
        canSlide = false;
        canDash = false;
        SetAirJumpCount(0);
    }
    
    private void FixedUpdate()
    {
        if (!hasInputEnabled) return;
        
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
                if (rb.velocity.magnitude > 0.2f)
                    player.PlayerSound.PlayFootsteps();
                else
                    player.PlayerSound.StopFootsteps();
            }
            else
                player.PlayerSound.StopFootsteps();
        }
        else
            player.PlayerSound.StopFootsteps();
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
        if (slideCooldownTimer > 0)
            slideCooldownTimer -= Time.deltaTime;
        rb.velocity = inputVelocity + extraVelocity;
    }

    public void InputEnabled(bool enabled)
    {
        hasInputEnabled = enabled;
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

    public void SetCanSlide(bool value)
    {
        canSlide = value;
    }

    public void SetCanDash(bool value)
    {
        canDash = value;
    }

    public void AttemptDash()
    {
        if (!hasInputEnabled) return;
        
        if (dashCooldownTimer <= 0 && movementInput.magnitude >= 0.2f && canDash)
            StartCoroutine(PerformDash());
    }

    public void AttemptSlide()
    {
        if (!hasInputEnabled) return;
        
        if (slideCooldownTimer <= 0 && movementInput.magnitude >= 0.2f && IsGrounded() && canSlide)
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
        if (!hasInputEnabled) return;
        
        jumpCooldownTimer = jumpCooldownReset;
        inputVelocity.y = jumpForce;
        player.PlayerSound.PlayJump();
    }

    private IEnumerator PerformSlide()
    {
        isSliding = true;
        player.PlayerSound.PlaySlide();
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
            if (!IsGrounded())
                player.PlayerSound.Pause();
            else
                player.PlayerSound.Resume();
            yield return null;
        }
        timer = 0;
        isSliding = false;
        player.PlayerSound.StopSlide();
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
        player.PlayerSound.PlayDash();
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}
