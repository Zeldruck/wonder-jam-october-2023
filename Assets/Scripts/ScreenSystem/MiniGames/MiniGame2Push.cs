using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MiniGame2Push : MiniGame
{
    public Canvas gameUI;
    public GameObject prefabTimeStep;
    public GameObject panelTimer;
    public TMP_Text textPushes;

    public Image buttonNorth;
    public Image buttonEast;
    public Image buttonSouth;
    public Image buttonWest;
    public TMP_Text keyboardW;
    public TMP_Text keyboardA;
    public TMP_Text keyboardS;
    public TMP_Text keyboardD;

    public InputToPush toPush = InputToPush.North;
    public float time = 10f;
    public int numberOfStepTime = 10;
    public float timeBetweenSteps = 1f;
    public int numberOfPushes = 10;
    public float maxFillWidth = 980f;
    public float spaceBetweenSteps = 5f;
    public float minAnchoredPosX = 500f;

    public override event CustomEventHandler OnEndMiniGame;

    [Header("------- DEBUG -------")]

    public float currentTime = 0f;
    public int currentNumberOfPushesLeft = 0;
    public int currentTimeSteps = 0;
    public List<GameObject> timeSteps = new();
    public int difficulty = 1;

    public override void EndMiniGame(bool isWon)
    {
        Debug.Log("EndMiniGame2");

        timeSteps.Clear();
        difficulty++;

        OnEndMiniGame?.Invoke(isWon);
    }

    public override void StartMiniGame()
    {
        Debug.Log("StartMiniGame2");

        for(int i = 0; i < numberOfStepTime; i++)
        {
            GameObject timeStep = Instantiate(prefabTimeStep, panelTimer.transform);

            Image img = timeStep.GetComponent<Image>();
            float width = maxFillWidth / numberOfStepTime - spaceBetweenSteps;
            img.rectTransform.sizeDelta = new Vector2(width, img.rectTransform.rect.height);
            img.rectTransform.anchoredPosition = new Vector2(
            img.rectTransform.anchoredPosition.x - minAnchoredPosX + (width / 2 + spaceBetweenSteps * 2) + i * (width + spaceBetweenSteps),
            img.rectTransform.anchoredPosition.y);
            
            timeSteps.Add(timeStep);
        }

        currentTimeSteps = numberOfStepTime - 1;
        currentNumberOfPushesLeft = numberOfPushes * difficulty;
        currentTime = 0f;
        textPushes.text = currentNumberOfPushesLeft.ToString();

        toPush = Random.Range(0, 4) switch
        {
            0 => InputToPush.North,
            1 => InputToPush.East,
            2 => InputToPush.South,
            3 => InputToPush.West,
            _ => InputToPush.North
        };
        
        switch(toPush)
        {
            case InputToPush.North:
                buttonNorth.gameObject.SetActive(true);
                buttonEast.gameObject.SetActive(false); 
                buttonWest.gameObject.SetActive(false);
                buttonSouth.gameObject.SetActive(false);
                keyboardW.gameObject.SetActive(true);
                keyboardA.gameObject.SetActive(false);
                keyboardD.gameObject.SetActive(false);
                keyboardS.gameObject.SetActive(false);
                break;
            case InputToPush.South:
                buttonNorth.gameObject.SetActive(false);
                buttonEast.gameObject.SetActive(false); 
                buttonWest.gameObject.SetActive(false);
                buttonSouth.gameObject.SetActive(true);
                keyboardW.gameObject.SetActive(false);
                keyboardA.gameObject.SetActive(false);
                keyboardD.gameObject.SetActive(false);
                keyboardS.gameObject.SetActive(true);
                break;
            case InputToPush.East:
                buttonNorth.gameObject.SetActive(false);
                buttonEast.gameObject.SetActive(true);
                buttonWest.gameObject.SetActive(false);
                buttonSouth.gameObject.SetActive(false);
                keyboardW.gameObject.SetActive(false);
                keyboardA.gameObject.SetActive(false);
                keyboardD.gameObject.SetActive(true);
                keyboardS.gameObject.SetActive(false);
                break;
            case InputToPush.West:
                buttonNorth.gameObject.SetActive(false);
                buttonEast.gameObject.SetActive(false);
                buttonWest.gameObject.SetActive(true);
                buttonSouth.gameObject.SetActive(false);
                keyboardW.gameObject.SetActive(false);
                keyboardA.gameObject.SetActive(true);
                keyboardD.gameObject.SetActive(false);
                keyboardS.gameObject.SetActive(false);
                break;

        }

    }

    public override void UpdateGameUI()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= timeBetweenSteps)
        {
            currentTime = 0f;
            timeSteps[currentTimeSteps].gameObject.SetActive(false);
            currentTimeSteps--;
        }

        if (currentTimeSteps < 0)
            EndMiniGame(false);
    }

    public override void UpdateMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public void OnActionNorth(InputAction.CallbackContext context)
    {
        if(context.action.ReadValue<float>() > 0.5f && context.action.triggered)
        {
            if (toPush == InputToPush.North)
            {
                Debug.Log("North Action");
                UpdateGame();
            }
        }
    }

    public void OnActionSouth(InputAction.CallbackContext context)
    {
        if (context.action.ReadValue<float>() > 0.5f && context.action.triggered)
        {
            if (toPush == InputToPush.South)
            {
                Debug.Log("South Action");
                UpdateGame();
            }
        }
    } 

    public void OnActionEast(InputAction.CallbackContext context)
    {
        if (context.action.ReadValue<float>() > 0.5f && context.action.triggered)
        {
            if (toPush == InputToPush.East)
            {
                Debug.Log("East Action");
                UpdateGame();
            }
        }
    }

    public void OnActionWest(InputAction.CallbackContext context)
    {
        if (context.action.ReadValue<float>() > 0.5f && context.action.triggered)
        {
            if (toPush == InputToPush.West)
            {
                Debug.Log("West Action");
                UpdateGame();
            }
        }
    }

    public override void OnShowPanel(InputAction.CallbackContext context)
    {
        gameUI.gameObject.SetActive(!gameUI.gameObject.activeSelf);
    }

    public void UpdateGame()
    {
        currentNumberOfPushesLeft--;
        textPushes.text = currentNumberOfPushesLeft.ToString();
        if (currentNumberOfPushesLeft <= 0)
            EndMiniGame(true);
    }

    public enum InputToPush
    {
        North,
        East,
        South,
        West
    }
}
