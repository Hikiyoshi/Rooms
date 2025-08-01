using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bong");
        if (other.TryGetComponent(out PlayerInteract playerInteract))
        {
            Debug.Log("NextState");
            GameManager.Instance.NextStage();
        }
    }
}
