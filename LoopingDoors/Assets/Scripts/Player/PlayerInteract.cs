using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Keybinds"), Space]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Attributes"), Space]
    [SerializeField] private float interactRange = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            IInteractable interactable = GetInteractableObject();
            if (!interactable.GetLockInteract())
            {
                interactable.Interact(transform);
            }
        }
    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactableObj))
            {
                interactableList.Add(interactableObj);
            }
        }

        IInteractable closetInteractableObj = null;

        foreach (IInteractable obj in interactableList)
        {
            if (closetInteractableObj == null)
            {
                closetInteractableObj = obj;
            }
            else
            {

                if (Vector3.Distance(transform.position, closetInteractableObj.GetTransform().position)
                > Vector3.Distance(transform.position, obj.GetTransform().position))
                {
                    closetInteractableObj = obj;
                }
            }
        }

        return closetInteractableObj;
    }
}
