using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame2Push : MiniGame
{
    public InputAction pushAction;

    public override event CustomEventHandler OnEndMiniGame;

    public override void EndMiniGame()
    {
        Debug.Log("EndMiniGame2");
        pushAction.Disable();
        pushAction.performed -= PushAction_performed;
        OnEndMiniGame?.Invoke();
    }

    public override void StartMiniGame()
    {
        Debug.Log("StartMiniGame2");
        pushAction.Enable();
        pushAction.performed += PushAction_performed;

    }
    public override void UpdateGameUI()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public void PushAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("PushAction_performed");
        EndMiniGame();
    }
}
