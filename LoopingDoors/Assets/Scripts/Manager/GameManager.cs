using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManager instance");
        }

        Instance = this;
    }

    public void NextStage()
    {
        Debug.Log("Next Stage");
    }
}
