using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame_2_Push : AMiniGame
{
    public InputAction inputA;
    public InputAction inputB;
    public bool shouldBeB = false;
    public bool shouldBeA = true;
    public override void StartMiniGame()
    {

    }

    public override void EndMiniGame()
    {

    }

    public override void UpdateMiniGame()
    {

    }

    public override void UpdateGameUI()
    {
     
    }

    private void Update()
    {
        if (inputA.triggered)
        {
            Debug.Log("A");

            // TODO: Increment Bar
            if(shouldBeA)
            {
                Debug.Log("Increment Bar: A");
            }
        }
        if (inputB.triggered)
        {
            Debug.Log("B");
            // TODO: Increment Bar
            if (shouldBeA)
            {
                Debug.Log("Increment Bar: B");
            }
        }
    }
}
