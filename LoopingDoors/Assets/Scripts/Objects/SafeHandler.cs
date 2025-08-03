using UnityEngine;

public class SafeHandler : MonoBehaviour, IInteractable
{

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
        AudioManager.Instance.Play("clock");
        GameManager.Instance.Win();
    }

    public void SetLockInteract(bool lockInteract)
    {
        this.lockInteract = lockInteract;
    }
}
