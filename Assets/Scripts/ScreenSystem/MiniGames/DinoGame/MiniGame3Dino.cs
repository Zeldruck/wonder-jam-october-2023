using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Net.Sockets;

public class MiniGame3Dino : MiniGame
{
    public DinoPlayer dinoPlayer;
    public DinoGround dinoGround;
    public DinoSpawner dinoSpawner;
    public TMP_Text scoreText;
    public int scoreToReach = 5;
    public float gameSpeed = 1.0f;

    [Header("----- DEBUG -----")]
    public int score = 0;

    public override event CustomEventHandler OnEndMiniGame;

    public override void EndMiniGame(bool isWon)
    {
        OnEndMiniGame?.Invoke(isWon);
    }

    public override void OnShowPanel(InputAction.CallbackContext context)
    {
        //gameUI.gameObject.SetActive(!gameUI.gameObject.activeSelf);
    }

    public override void StartMiniGame()
    {
        score = 0;
    }

    public override void UpdateGameUI()
    {
        scoreText.text = score.ToString();
    }

    public override void UpdateMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public void IncrementScore()
    {
        score++;

        if(score == scoreToReach)
        {
            EndMiniGame(true);
        }
    }
}
