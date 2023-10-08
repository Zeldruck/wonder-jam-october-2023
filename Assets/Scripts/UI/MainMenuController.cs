using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string playSceneName;
    [SerializeField] private string creditsSceneName;
    [SerializeField] private string returnSceneName;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void PlayClicked()
    {
        SceneManager.LoadScene(playSceneName);
    }

    public void CreditsClicked()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    public void ReturnClicked()
    {
        SceneManager.LoadScene(returnSceneName);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
