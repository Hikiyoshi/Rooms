using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public event Action OnStartStage;
    public event Action OnEndMove;

    public static StageManager Instance { get; private set; }
    public static int Stage { get; private set; }

    [Header("References"), Space]
    [SerializeField] private Transform leftSpikesTramsform;
    [SerializeField] private Transform rightSpikesTramsform;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;

    [Header("Stage Settings"), Space]
    [SerializeField] private Stage currentStage;

    private bool start = false;
    private float _timeWaitToStart;
    private float _timeToEndStage;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one StageManager instance");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStartGame += GameManager_OnStartGame;

        _timeWaitToStart = currentStage.timeWaitToStart;
        _timeToEndStage = currentStage.timeToEndStage;

        Stage = 2;
    }

    private void Update()
    {
        switch (Stage)
        {
            case 2:
                Stage02Handler();
                break;
            case 3:
                break;
        }
    }

    private void Stage02Handler()
    {
        CountTime();

        if (!start)
        {
            return;
        }

        OnStartStage?.Invoke();
    }

    private void CountTime()
    {
        _timeWaitToStart -= Time.deltaTime;

        if (_timeWaitToStart <= 0f)
        {
            start = true;
        }

        if (_timeToEndStage == 0)
        {
            return;
        }

        if (start)
        {
            _timeToEndStage -= Time.deltaTime;
        }

        if (_timeToEndStage < 0f)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void GameManager_OnStartGame()
    {
        UpdateStage(1);
    }

    public void UpdateStage(int newStage)
    {
        Stage = newStage;
    }

    public void loadCurrentStage()
    {
        SceneManager.LoadScene("SceneStage" + Stage);
    }

    public void EndMoveStage2()
    {
        OnEndMove?.Invoke();
    }
}