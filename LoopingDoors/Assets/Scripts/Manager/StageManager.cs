using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    public static int Stage { get; private set; }

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
}