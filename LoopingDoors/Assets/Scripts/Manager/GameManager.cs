using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References"), Space]
    [SerializeField] private GameObject blinkGameObject;
    [SerializeField] private GameObject fadeOutGameObject;
    [SerializeField] private GameObject fadeInGameObject;
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
            StartCoroutine(WakeUp());
        }
        else
        {
            StartCoroutine(fadeIn());
        }
    }

    public void NextStage()
    {
        Debug.Log("Next Stage");
        StageManager.Instance.UpdateStage(2);

        StartCoroutine(fadeOut());
        
        Invoke(nameof(loadNextStage), 1f);
    }
    private void loadNextStage()
    {
        SceneManager.LoadScene("SceneStage" + StageManager.Stage);
    }

    private IEnumerator WakeUp()
    {
        playerMovement.enabled = false;
        playerCamera.enabled = false;
        blinkGameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        blinkGameObject.GetComponent<Animator>().SetTrigger("blinkEye");
        yield return new WaitForSeconds(2.5f);
        playerMovement.enabled = true;
        playerCamera.enabled = true;
    }

    private IEnumerator fadeOut()
    {
        fadeOutGameObject.SetActive(true);
        fadeOutGameObject.GetComponent<Animator>().SetTrigger("fadeoutTrig");
        yield return new WaitForSeconds(1.5f);
        fadeOutGameObject.SetActive(false);
    }
    private IEnumerator fadeIn()
    {
        fadeInGameObject.SetActive(true);
        fadeInGameObject.GetComponent<Animator>().SetTrigger("fadeinTrig");
        yield return new WaitForSeconds(1f);
        fadeInGameObject.SetActive(false);
    }
}
