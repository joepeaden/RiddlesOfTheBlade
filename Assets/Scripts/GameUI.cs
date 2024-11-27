using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{
    public UnityEvent OnRiddleGuessed = new();

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
    [SerializeField] private TMP_Text riddleText;
    [SerializeField] private TMP_InputField riddleGuessInput;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(HandleGameStart);
        restartGameButton.onClick.AddListener(HandleGameStart);
        attemptRiddleButton.onClick.AddListener(HandleRiddleGuess);
    }

    private void Start()
    {
        GameManager.Instance.OnWaveEnd.AddListener(HandleWaveEnd);
        GameManager.Instance.OnWaveBegin.AddListener(HandleWaveBegin);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
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
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        gameStartButton.onClick.RemoveListener(HandleGameStart);
        restartGameButton.onClick.RemoveListener(HandleGameStart);
        attemptRiddleButton.onClick.RemoveListener(HandleRiddleGuess);
    }

    // all these conditionals are intended to avoid unnecessary SetActives
    // and UI updates - but I think they're probably overkill here

    private void HandleGameOver(bool playerWon)
    {
        if (gameplayScreen.activeInHierarchy)
        {
            gameplayScreen.SetActive(false);
        }

        if (riddleScreen.activeInHierarchy)
        {
            riddleScreen.SetActive(false);
        }

        if (!gameOverScreen.activeInHierarchy)
        {
            gameOverScreen.SetActive(true);
        }

        if (playerWon)
        {
            if (gameOverText.text != "You win!")
            {
                gameOverText.text = "You win!";
            }
        }
        else if (gameOverText.text != "You lose!")
        {
            gameOverText.text = "You lose!";
        }

        timerText.text = "";
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

        // set up riddle screen
        if (!riddleScreen.activeInHierarchy)
        {
            riddleScreen.gameObject.SetActive(true);
        }

        PowerData power = GameManager.Instance.GetCurrentPower();

        if (riddleText.text != power.riddle)
        {
            riddleText.text = power.riddle;
        }
    }

    private void HandleRiddleGuess()
    {
        // Note for postmortem: I am a little unsure if, when you have access
        // to another script, you should still use an event to communicate
        // to that script, or if you should just call a method directly in that
        // script.
        // In this case, I need to tell the game manager that we've guessed a
        // riddle. I could just directly call the method - advantage being
        // that I can clearly see here what will happen on riddle guessed, as
        // opposed to having to look at all references of OnRiddleGuessed to
        // see what will happen. And once you get a lot of events, it can become
        // really confusing to debug, order of operations, etc.
        GameManager.Instance.HandleRiddleGuessed(riddleGuessInput.text);
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
