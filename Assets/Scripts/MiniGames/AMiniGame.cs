using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMiniGame : MonoBehaviour
{
    public abstract void StartMiniGame();
    public abstract void EndMiniGame();
    public abstract void UpdateMiniGame();
    public abstract void UpdateGameUI();
}
