using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.NextStage();
        }
    }
}
