using UnityEngine;

public class ButtonOpenDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform doorPrefab;
    [SerializeField] private Transform doorSpawnPosition;

    [SerializeField] private string interactText = "Push Button";
    [SerializeField] private bool lockInteract = false;

    public string GetInteractText()
    {
        return interactText;
    }

    public bool GetLockInteract()
    {
        return lockInteract;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        Transform doorTransform = Instantiate(doorPrefab, doorSpawnPosition.position, Quaternion.identity);
    }
}
