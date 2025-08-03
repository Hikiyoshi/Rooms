using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnStartGame;

    [Header("References"), Space]
    [SerializeField] private GameObject blinkGameObject;
    [SerializeField] private GameObject fadeOutGameObject;
    [SerializeField] private GameObject fadeInGameObject;
    [SerializeField] private GameObject PauseMenuGameObject;

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
        OnStartGame?.Invoke();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
    }

    public void NextStage()
    {
        StageManager.Instance.UpdateStage(2);

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
        StartCoroutine(fadeOut());
        yield return new WaitForSeconds(1.3f);
        StageManager.Instance.UpdateStage(0);
        SceneManager.LoadScene(0);
    }
}
