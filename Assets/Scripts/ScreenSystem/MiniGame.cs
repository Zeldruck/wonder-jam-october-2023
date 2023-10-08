using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MiniGame : MonoBehaviour
{
    // Declare a delegate
    public delegate void CustomEventHandler(bool isWon);

    // Declare an event based on the delegate
    public abstract event CustomEventHandler OnEndMiniGame;

    public abstract void StartMiniGame();
    public abstract void EndMiniGame(bool isWon);
    public abstract void UpdateMiniGame();
    public abstract void UpdateGameUI();
    public abstract void OnShowPanel(InputAction.CallbackContext context);
}