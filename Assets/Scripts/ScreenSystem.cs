using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSystem : MonoBehaviour
{
    public GameObject panelChoices;
    public Button buttonAttack;
    public Button buttonHeal;
    public Button buttonPowerUps;
    public MiniGameSystem miniGameSystem;

    [Header("---- DEBUG -------")]
    public bool isMiniGameRunning = false;
    public bool isMiniGameFinished = false;
    public bool isMiniGameWon = false;
    public bool isMiniGameLost = false;
    public bool isMiniGamePaused = false;
    public bool isPowerUpsLock = true;

    private void Awake()
    {
        buttonAttack.onClick.AddListener(OnClickAttack);
        buttonHeal.onClick.AddListener(OnClickHeal);
        buttonPowerUps.onClick.AddListener(OnClickPowerUps);
    }

    public void OnClickAttack()
    {
        Debug.Log("Attack");
    }

    public void OnClickHeal()
    {
        Debug.Log("Heal");  

    }

    public void OnClickPowerUps()
    {
        Debug.Log("PowerUps");
    }

    void DisplayChoices()
    {
        if(isPowerUpsLock)
        {
            buttonPowerUps.enabled = false;
        }
        else 
        { 
            buttonPowerUps.enabled = true; 
        }
    }

    // Render Target
    // Render One Camera for Each Game
    //
    void DisplayGame()
    {

    }
}
