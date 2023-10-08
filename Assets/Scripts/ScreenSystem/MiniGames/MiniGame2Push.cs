using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MiniGame2Push : MiniGame
{
    public GameObject prefabTimeStep;
    public GameObject panelTimer;
    public TMP_Text textPushes;
    public InputAction pushAction;
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

    public override void EndMiniGame(bool isWon)
    {
        Debug.Log("EndMiniGame2");
        pushAction.Disable();
        pushAction.performed -= PushAction_performed;
        OnEndMiniGame?.Invoke(isWon);
    }

    public override void StartMiniGame()
    {
        Debug.Log("StartMiniGame2");
        pushAction.Enable();
        pushAction.performed += PushAction_performed;

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
        currentNumberOfPushesLeft = numberOfPushes;
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

    public void PushAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("PushAction_performed");
        currentNumberOfPushesLeft--;
        textPushes.text = currentNumberOfPushesLeft.ToString();
        if(currentNumberOfPushesLeft <= 0)
            EndMiniGame(true);
    }
}
