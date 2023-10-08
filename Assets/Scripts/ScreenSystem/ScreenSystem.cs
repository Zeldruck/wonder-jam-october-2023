using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScreenSystem : MonoBehaviour
{
    public Player player;
    public Boss.Boss boss;
    public MiniGameSystem miniGameSystem;

    public Image panelScreen;
    public Image panelGame; 
    public Image panelChoices;
    public Button buttonAttack;
    public Button buttonHeal;
    public Button buttonPowerUps;
    public Image panelArrow;
    public Image imageLeftArrow;
    public Image imageRightArrow;

    [Header("------- DEBUG -------")]
    public bool isMiniGameRunning = false;
    public bool isMiniGameFinished = false;
    public bool isPowerUpsLock = true;
    public bool isDisplay = false;
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

        isDisplay = false;
    }

    private void Update()
    {
        Vector2 panelChoicesPos = panelChoices.rectTransform.anchoredPosition;

        if (panelChoicesPos == Vector2.zero)
        {
            imageLeftArrow.enabled = false;
            imageRightArrow.enabled = true;
            buttonAttack.enabled = true;
            buttonHeal.enabled = false;
        }
        else if (panelChoicesPos == new Vector2(-1920, 0) && !isPowerUpsLock)
        {
            imageRightArrow.enabled = false;
        }
        else if (panelChoicesPos == new Vector2(-960, 0) && isPowerUpsLock)
        {
            imageRightArrow.enabled = false;
            imageLeftArrow.enabled = true;
            buttonAttack.enabled = false;
            buttonHeal.enabled = true;
        }
        else
        {
            imageLeftArrow.enabled = true;
            imageRightArrow.enabled = true;
        }

        if (isPowerUpsLock)
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
        panelScreen.gameObject.SetActive(false);
        miniGameSystem.StartShortMiniGame();
        isMiniGameRunning = true;
    }

    public void ChooseLongMiniGame()
    {
        panelScreen.gameObject.SetActive(false);
        miniGameSystem.StartLongMiniGame();
        isMiniGameRunning = true;
    }

    public void ApplyEffect()
    {
        if(isMiniGameWon)
        {
            switch (miniGameType)
            {
                case MiniGameType.Attack:
                    Debug.Log("Attack Positive");
                    boss.ReceiveDamages(25);
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
            switch (miniGameType)
            {
                case MiniGameType.Attack:
                    Debug.Log("Attack Negative");
                    player.ReduceLife();
                    break;
                case MiniGameType.Heal:
                    Debug.Log("Heal Negative");
                    boss.ReceiveHeal(10);
                    break;
                case MiniGameType.PowerUps:
                    Debug.Log("PowerUps Negative");
                    break;
            }
        }

        isMiniGameRunning = false;
        isMiniGameFinished = true;
    }

    public void UnlockPowerUps()
    {
        isPowerUpsLock = false;
    }

    #region Input
    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if (!isMiniGameRunning)
        {
            Vector2 panelChoicesPos = panelChoices.rectTransform.anchoredPosition;
            if (value == Vector2.left && panelChoicesPos.x < 0)
            {
                panelChoices.rectTransform.anchoredPosition += new Vector2(960, 0);
            }
            else if (value == Vector2.right && panelChoicesPos.x > -1920 && !isPowerUpsLock)
            {
                panelChoices.rectTransform.anchoredPosition += new Vector2(-960, 0);
            }
            else if (value == Vector2.right && panelChoicesPos.x > -960 && isPowerUpsLock)
            {
                panelChoices.rectTransform.anchoredPosition += new Vector2(-960, 0);
            }
        }
    }

    public void OnShowPanel(InputAction.CallbackContext context)
    {
        Cursor.visible = !isDisplay;
        Cursor.lockState = !isDisplay ? CursorLockMode.Confined : CursorLockMode.Locked;
        
        if (!isDisplay) player.StopPlayer();
        else player.StartPlayer();
        
        if(!isMiniGameRunning)
        {
            isDisplay = !isDisplay;
            panelScreen.gameObject.SetActive(isDisplay);
        }
        else
        {
            miniGameSystem.currentGame.OnShowPanel(context);
        }
    }
    #endregion
}

public enum MiniGameType
{
    Attack,
    Heal,
    PowerUps
}
