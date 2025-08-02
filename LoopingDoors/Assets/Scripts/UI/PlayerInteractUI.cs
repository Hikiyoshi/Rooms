using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private GameObject containerGameobject;
    [SerializeField] private TMP_Text interactText;

    private void Update()
    {
        IInteractable interactObject = playerInteract.GetInteractableObject();
        if (interactObject != null)
        {
            Show();
            interactText.text = interactObject.GetInteractText();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        containerGameobject.SetActive(true);
    }

    public void Hide()
    {
        containerGameobject.SetActive(false);
    }
}
