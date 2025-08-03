using UnityEngine;

public class ButtonSpawnDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private SafeHandler safe;
    [SerializeField] private string interactText = "Interact";
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
        StageManager.Instance.SpawnDoor();

        AudioManager.Instance.Play("Safe");
        safe.SetLockInteract(false);
    }

    public void SetLockInteract(bool lockInteract)
    {
        this.lockInteract = lockInteract;
    }
}
