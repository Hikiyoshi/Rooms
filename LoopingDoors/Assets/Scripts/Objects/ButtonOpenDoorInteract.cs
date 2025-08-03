using UnityEngine;

public class ButtonOpenDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject OpenDoorGameObject;

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
        Debug.Log("Open Door");
        AudioManager.Instance.Play("Button");
        AudioManager.Instance.Play("OpenDoor");
        Animator animator = OpenDoorGameObject.GetComponent<Animator>();
        animator.SetTrigger("openDoor");
        
        SetLockInteract(true);
    }

    public void SetLockInteract(bool lockInteract)
    {
        this.lockInteract = lockInteract;
    }
}
