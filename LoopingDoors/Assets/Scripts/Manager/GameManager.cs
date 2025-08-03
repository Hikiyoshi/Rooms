using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnStartGame;
    public event Action OnGameOver;

    [Header("References"), Space]
    [SerializeField] private GameObject blinkGameObject;
    [SerializeField] private GameObject fadeOutGameObject;
    [SerializeField] private GameObject fadeInGameObject;
    [SerializeField] private GameObject PauseMenuGameObject;
    [SerializeField] private GameObject GameOverMenuGameObject;
    [SerializeField] private GameObject WinGameObject;


    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCamera playerCamera;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManager instance");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (StageManager.Stage == 0)
        {
            return;
        }

        if (StageManager.Stage == 1)
        {
            StartCoroutine(WakeUp());
        }
        else
        {
            StartCoroutine(fadeIn());
        }
    }

    public void StartGame()
    {
        AudioManager.Instance.Play("RoomAmbience");
        OnStartGame?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();

        playerMovement.enabled = false;
        playerCamera.enabled = false;

        UnLockCursor();

        Time.timeScale = 0;

        GameOverMenuGameObject.SetActive(true);

        if (StageManager.Stage == 2)
        {
            StageManager.Instance.EndMoveStage2();
        }
    }

    public void NextStage()
    {
        int currentStage = StageManager.Stage;
        StageManager.Instance.UpdateStage(++currentStage);

        StartCoroutine(fadeOut());

        Debug.Log("Next Stage: " + StageManager.Stage);
        StartCoroutine(LoadSceneAferFadeOut());
    }
    public IEnumerator LoadSceneAferFadeOut()
    {
        yield return new WaitForSeconds(1.2f);
        StageManager.Instance.loadCurrentStage();
    }

    private IEnumerator WakeUp()
    {
        playerMovement.enabled = false;
        playerCamera.enabled = false;
        blinkGameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        blinkGameObject.GetComponent<Animator>().SetTrigger("blinkEye");
        yield return new WaitForSeconds(2.5f);
        playerMovement.enabled = true;
        playerCamera.enabled = true;
    }

    public IEnumerator fadeOut()
    {
        fadeOutGameObject.SetActive(true);
        fadeOutGameObject.GetComponent<Animator>().SetTrigger("fadeoutTrig");
        yield return new WaitForSeconds(1.5f);
        fadeOutGameObject.SetActive(false);
    }
    public IEnumerator fadeIn()
    {
        fadeInGameObject.SetActive(true);
        fadeInGameObject.GetComponent<Animator>().SetTrigger("fadeinTrig");
        yield return new WaitForSeconds(1f);
        fadeInGameObject.SetActive(false);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenuGameObject.SetActive(true);

        UnLockCursor();
    }

    public void ResumeGame()
    {
        LockCursor();

        PauseMenuGameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReStartGame()
    {
        Time.timeScale = 1;
        StageManager.Instance.UpdateStage(1);
        StageManager.Instance.loadCurrentStage();
    }

    public void Win()
    {
        Time.timeScale = 0;
        WinGameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        playerMovement.enabled = false;
        playerCamera.enabled = false;
        Time.timeScale = 1;
        StartCoroutine(FadeBeforeBackStartScene());
    }
    private IEnumerator FadeBeforeBackStartScene()
    {
        PauseMenuGameObject.SetActive(false);
        GameOverMenuGameObject.SetActive(false);
        StartCoroutine(fadeOut());
        yield return new WaitForSeconds(1.3f);
        StageManager.Instance.UpdateStage(0);
        SceneManager.LoadScene(0);
    }
}
