using UnityEngine;

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

    public void UpdateStage(int newStage)
    {
        Stage = newStage;
    }
}