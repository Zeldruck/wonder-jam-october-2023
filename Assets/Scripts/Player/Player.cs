using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Collections.Specialized.BitVector32;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponentInChildren<PlayerLook>();
    }
    #region Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovement.SetInputValue(context.ReadValue<Vector2>());
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        playerMovement.SetJumpValue(context.ReadValue<float>() > 0.5f);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
            playerMovement.AttemptDash();
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if(context.action.ReadValue<float>() > 0.5f && context.action.triggered)
            playerMovement.AttemptSlide();
        playerMovement.SetSlideValue(context.ReadValue<float>() > 0.5f);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        playerLook.SetIsStick(context.control.name == "rightStick");
        playerLook.SetLookInput(context.ReadValue<Vector2>());
    }
    #endregion
}
