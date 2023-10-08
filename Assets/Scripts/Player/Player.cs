using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponentInChildren<PlayerLook>();
        currentLives = maxLives;
    }

    [ContextMenu("lose life")]
    public void ReduceLife()
    {
        currentLives--;
        if (currentLives <= 0) //TODO: call game over
            GameManager.instance.GameLost();
    }

    public void RegenerateLife()
    {
        currentLives++;
        if (currentLives > maxLives)
            currentLives = maxLives;
    }

    [ContextMenu("win game")]
    public void DebugWinGame()
    {
        GameManager.instance.GameWon();
    }

    public int GetCurrentLives() => currentLives;

    public void SetAirJumpCount(int value)
    {
        playerMovement.SetAirJumpCount(value);
    }

    public void SetCanSlide(bool value)
    {
        playerMovement.SetCanSlide(value);
    }

    public void SetCanDash(bool value)
    {
        playerMovement.SetCanDash(value);
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
