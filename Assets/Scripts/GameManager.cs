using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [HideInInspector]
    public UnityEvent OnWaveBegin = new();
    [HideInInspector]
    public UnityEvent OnWaveEnd = new();

    public GameData Data => _data;
    [SerializeField] private GameData _data;
    public int CurrentWave => _currentWave;
    private int _currentWave;
    public bool GameStarted => _gameStarted;
    private bool _gameStarted;

    private float timerStartTime;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("There's two game managers, destroying one!");
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        _gameStarted = true;
        _currentWave = 0;
        StartCoroutine(StartWaveTimer());
    }

    public IEnumerator StartWaveTimer()
    {
        while (true)
        {
            timerStartTime = Time.time;
            OnWaveBegin.Invoke();

            yield return new WaitForSeconds(_data.baseTimer);

            OnWaveEnd.Invoke();

            timerStartTime = Time.time;
            yield return new WaitForSeconds(_data.baseTimer);
        }
    }

    public float GetTime()
    {
        return _data.baseTimer - (Time.time - timerStartTime);
    }
}
