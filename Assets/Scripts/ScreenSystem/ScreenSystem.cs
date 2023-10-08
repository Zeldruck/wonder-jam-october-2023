using UnityEngine;
using UnityEngine.UI;

public class ScreenSystem : MonoBehaviour
{
    public Player player;
    public Boss.Boss boss;
    public Image panelScreen;
    public Image panelGame; 
    public Image panelChoices;
    public Button buttonAttack;
    public Button buttonHeal;
    public Button buttonPowerUps;
    public Image panelArrow;
    public Image imageLeftArrow;
    public Image imageRightArrow;
    public MiniGameSystem miniGameSystem;

    [Header("------- DEBUG -------")]
    public bool isMiniGameRunning = false;
    public bool isMiniGameFinished = false;
    public bool isMiniGamePaused = false;
    public bool isPowerUpsLock = true;
    public MiniGameType miniGameType;

    private bool isMiniGameWon;
    public bool IsMiniGameWon
    {
        get { return isMiniGameWon; }
        set
        {
            isMiniGameWon = value;
            ApplyEffect();
        }
    }

    private void Awake()
    {
        buttonAttack.onClick.AddListener(OnClickAttack);
        buttonHeal.onClick.AddListener(OnClickHeal);
        buttonPowerUps.onClick.AddListener(OnClickPowerUps);
    }

    private void Update()
    {
        Vector2 panelChoicesPos = panelChoices.rectTransform.anchoredPosition;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && panelChoicesPos.x < 0)
        {
            panelChoices.rectTransform.anchoredPosition += new Vector2(960, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && panelChoicesPos.x > -1920 && !isPowerUpsLock)
        {
            panelChoices.rectTransform.anchoredPosition += new Vector2(-960, 0);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) && panelChoicesPos.x > -960 && isPowerUpsLock)
        {
            panelChoices.rectTransform.anchoredPosition += new Vector2(-960, 0);
        }

        if (panelChoicesPos == new Vector2(0, 0))
        {
            imageLeftArrow.enabled = false;
            imageRightArrow.enabled = true;
        }
        else if(panelChoicesPos == new Vector2(-1920, 0) && !isPowerUpsLock)
        {
            imageRightArrow.enabled = false;
        }
        else if(panelChoicesPos == new Vector2(-960, 0) && isPowerUpsLock)
        {
            imageRightArrow.enabled = false;
            imageLeftArrow.enabled = true;
        }
        else
        {
            imageLeftArrow.enabled = true;
            imageRightArrow.enabled = true;
        }
        
        if(isPowerUpsLock)
        {
            buttonPowerUps.gameObject.SetActive(false);
        }
        else
        {
            buttonPowerUps.gameObject.SetActive(true);
        }
    }

    public void OnClickAttack()
    {
        Debug.Log("Attack");
        miniGameType = MiniGameType.Attack;
        ChooseShortMiniGame();
    }

    public void OnClickHeal()
    {
        Debug.Log("Heal");
        miniGameType = MiniGameType.Heal;
        ChooseShortMiniGame();
    }

    public void OnClickPowerUps()
    {
        Debug.Log("PowerUps");
        miniGameType = MiniGameType.PowerUps;
        ChooseLongMiniGame();
    }

    public void ChooseShortMiniGame()
    {
        gameObject.SetActive(false);
        miniGameSystem.StartShortMiniGame();
        isMiniGameRunning = true;
    }

    public void ChooseLongMiniGame()
    {
        gameObject.SetActive(false);
        miniGameSystem.StartLongMiniGame();
        isMiniGameRunning = true;
    }

    public void ApplyEffect()
    {
        if(isMiniGameWon)
        {
            // Do something positive
            switch (miniGameType)
            {
                case MiniGameType.Attack:
                    Debug.Log("Attack Positive");
                    boss.ReceiveDamages(1);
                    break;
                case MiniGameType.Heal:
                    Debug.Log("Heal Positive");
                    player.RegenerateLife();
                    break;
                case MiniGameType.PowerUps:
                    Debug.Log("PowerUps Positive");
                    // Player unlock power ups
                    break;
            }
        }
        else
        {
            // do something negative
            switch (miniGameType)
            {
                case MiniGameType.Attack:
                    Debug.Log("Attack Negative");
                    player.ReduceLife();
                    break;
                case MiniGameType.Heal:
                    Debug.Log("Heal Negative");
                    boss.ReceiveDamages(-1);
                    break;
                case MiniGameType.PowerUps:
                    Debug.Log("PowerUps Negative");
                    break;
            }
        }
    }

    public void UnlockPowerUps()
    {
        isPowerUpsLock = false;
    }
}

public enum MiniGameType
{
    Attack,
    Heal,
    PowerUps
}
