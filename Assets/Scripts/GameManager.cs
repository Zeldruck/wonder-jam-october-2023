using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void GameWon()
    {
        //TODO:do stuff pre-win (victory music, animation, cutscene)
        SceneManager.LoadScene("Victory");
    }

    public void GameLost()
    {
        //TODO: do stuff pre-lose (lose music, animation, cutscene)
        SceneManager.LoadScene("Defeat");
    }
}
