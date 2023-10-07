using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnLook(InputAction.CallbackContext context)
    {
        playerLook.SetIsStick(context.control.name == "rightStick");
        playerLook.SetLookInput(context.ReadValue<Vector2>());
    }
    #endregion
}
