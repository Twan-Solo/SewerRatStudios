using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Raycasts from the camera to detect interactable objects.
/// Checks for IInteractable on whatever the player is looking at.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;

    private Camera mainCam;
    private IInteractable currentTarget;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        CheckForInteractable();

        if (currentTarget != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentTarget.Interact();
        }
}

private void CheckForInteractable()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
           
           if (interactable != null)
            {
                currentTarget = interactable;
                // TODO: UI prompt using currentTarget.GetPromptText()
                return;
            }
        }

        currentTarget = null;
        // TODO: Hide UI prompt
    }
}
