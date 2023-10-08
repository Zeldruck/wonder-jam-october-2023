using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public static IntroManager instance;

    private bool isFinished;

    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _fadeImage;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    private void Start()
    {
        _videoPlayer.loopPointReached += EndReached;
        
        _videoPlayer.Play();
    }

    private void EndReached(VideoPlayer vp)
    {
        isFinished = true;
        
        _videoPlayer.gameObject.SetActive(false);
        StartCoroutine(IFade());
    }

    private void Update()
    {
        if (isFinished) return;
        
        if (Input.anyKey)
        {
            EndReached(_videoPlayer);
        }
    }

    private IEnumerator IFade()
    {
        float t = 0f;

        while (t <= 1f)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(1f, 0f, t);
            
            _fadeImage.color = new Color(0f, 0f, 0f, a);

            yield return null;
        }
        
        // Done
        // Start game
        GameManager.instance.StartGame();
        _canvas.SetActive(false);
        gameObject.SetActive(false);
    }
}
