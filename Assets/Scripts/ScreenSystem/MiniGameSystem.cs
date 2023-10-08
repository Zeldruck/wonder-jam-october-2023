using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameSystem : MonoBehaviour
{
    public ScreenSystem screenSystem;
    public List<MiniGame> shortMiniGames = new();
    public List<MiniGame> longMiniGames = new();
    public MiniGame currentGame;

    private void Awake()
    {
        foreach(MiniGame miniGame in shortMiniGames)
        {
            miniGame.gameObject.SetActive(false);
        }

        foreach(MiniGame miniGame in longMiniGames)
        {
            miniGame.gameObject.SetActive(false);
        }
    }

    public void StartShortMiniGame()
    {
        int random = Random.Range(0, shortMiniGames.Count);
        currentGame = shortMiniGames[random];
        currentGame.StartMiniGame();
        currentGame.OnEndMiniGame += CurrentGame_OnEndMiniGame;
        currentGame.gameObject.SetActive(true);
    }

    public void StartLongMiniGame()
    {
        int random = Random.Range(0, shortMiniGames.Count);
        currentGame = longMiniGames[random];
        currentGame.StartMiniGame();
        currentGame.OnEndMiniGame += CurrentGame_OnEndMiniGame;
        currentGame.gameObject.SetActive(true);
    }

    private void CurrentGame_OnEndMiniGame(bool isWon)
    {
        currentGame.OnEndMiniGame -= CurrentGame_OnEndMiniGame;
        currentGame.gameObject.SetActive(false);
        currentGame = null;

        screenSystem.IsMiniGameWon = isWon;
        screenSystem.gameObject.SetActive(true);
    }

    public void Update()
    {
        if(currentGame)
            currentGame.UpdateGameUI();
    }
}
