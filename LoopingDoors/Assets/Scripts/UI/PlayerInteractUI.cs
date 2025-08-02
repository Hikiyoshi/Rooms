using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private GameObject containerGameobject;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI eText;
    [SerializeField] private Color canInteractColor;
    [SerializeField] private Color cannotInteractColor;

    private void Update()
    {
        IInteractable interactObject = playerInteract.GetInteractableObject();
        if (interactObject != null)
        {
            Show();
            interactText.text = interactObject.GetInteractText();

            if (interactObject.GetLockInteract())
            {
                eText.color = cannotInteractColor;
            }
            else
            {
                eText.color = canInteractColor;
            }
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
