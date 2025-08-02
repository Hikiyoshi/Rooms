using System;
using System.Collections;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] private GameObject MenuGameObject;
    [SerializeField] private GameObject DoorGameObject;
    [SerializeField] private GameObject cameraGameObject;

    private void Start()
    {
        GameManager.Instance.OnStartGame += GameManager_OnStartGame;
    }

    private void GameManager_OnStartGame()
    {
        MenuGameObject.SetActive(false);
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        var cameraAnim = cameraGameObject.GetComponent<Animator>();
        cameraAnim.SetTrigger("play");
        yield return new WaitForSeconds(.8f);
        DoorGameObject.GetComponent<Animator>().SetTrigger("openDoor");
        yield return new WaitForSeconds(1.5f);
        cameraAnim.SetTrigger("play");
        yield return new WaitForSeconds(.7f);
        StartCoroutine(GameManager.Instance.fadeOut());
        yield return new WaitForSeconds(1.2f);
        StageManager.Instance.loadCurrentStage();
    }
}
