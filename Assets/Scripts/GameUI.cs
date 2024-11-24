using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button attemptRiddleButton;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject riddleScreen;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text timerText;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(HandleGameStart);
        restartGameButton.onClick.AddListener(HandleGameStart);
    }

    private void Start()
    {
        GameManager.Instance.OnWaveEnd.AddListener(HandleWaveEnd);
        GameManager.Instance.OnWaveBegin.AddListener(HandleWaveBegin);
    }

    private void Update()
    {
        if (GameManager.Instance.GameStarted)
        {
            string curretTime = GameManager.Instance.GetTime().ToString();
            if (timerText.text != curretTime)
            {
                timerText.text = curretTime;
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWaveEnd.RemoveListener(HandleWaveEnd);
        GameManager.Instance.OnWaveBegin.RemoveListener(HandleWaveBegin);
    }

    private void HandleWaveBegin()
    {
        if (!gameplayScreen.activeInHierarchy)
        {
            gameplayScreen.gameObject.SetActive(true);
        }

        if (riddleScreen.activeInHierarchy)
        {
            riddleScreen.gameObject.SetActive(false);
        }
    }

    private void HandleWaveEnd()
    {
        if (gameplayScreen.activeInHierarchy)
        {
            gameplayScreen.gameObject.SetActive(false);
        }

        if (!riddleScreen.activeInHierarchy)
        {
            riddleScreen.gameObject.SetActive(true);
        }
    }

    private void HandleGameStart()
    {
        if (startScreen.activeInHierarchy)
        {
            startScreen.gameObject.SetActive(false);
        }

        if (gameOverScreen.activeInHierarchy)
        {
            gameOverScreen.gameObject.SetActive(false);
        }

        GameManager.Instance.StartGame();
    }
}
