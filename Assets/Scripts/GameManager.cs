using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Boss.Boss _boss;
    [SerializeField] private Player _player;

    [SerializeField] private AudioSource _music;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public void StartGame()
    {
        _boss.StartBoss();
        _player.StartPlayer();
        _music.Play();
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
