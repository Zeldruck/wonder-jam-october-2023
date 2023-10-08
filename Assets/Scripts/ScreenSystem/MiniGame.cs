using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    // Declare a delegate
    public delegate void CustomEventHandler();

    // Declare an event based on the delegate
    public abstract event CustomEventHandler OnEndMiniGame;

    public abstract void StartMiniGame();
    public abstract void EndMiniGame();
    public abstract void UpdateMiniGame();
    public abstract void UpdateGameUI();
}