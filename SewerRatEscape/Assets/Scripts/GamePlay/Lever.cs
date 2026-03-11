using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// A lever the player can pull once. Fires a UnityEvent so you can
/// hook up whatever it triggers in the Inspector. Optionally destroys
/// assigned GameObjects when pulled.
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{
    [Header("Lever Settings")]
    public UnityEvent onPull;                // Actions triggered when lever is pulled
    public string promptText = "Pull Lever";
    public bool destroyAfterPull = false;    // Destroy lever itself after pulling

    [Header("Optional Objects to Destroy")]
    public List<GameObject> objectsToDestroy; // Assign any GameObjects to destroy

    private bool hasBeenPulled;

    /// <summary>
    /// Called by the player to interact with this lever
    /// </summary>
    public void Interact()
    {
        if (hasBeenPulled) return;

        hasBeenPulled = true;

        // Fire all assigned actions in the UnityEvent
        onPull.Invoke();

        // Set object inactive
        if (objectsToDestroy != null)
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        Debug.Log(gameObject.name + " pulled");

        // Optional: destroy the lever itself
        if (destroyAfterPull)
            Destroy(gameObject);
    }

    /// <summary>
    /// Returns the prompt text for UI
    /// </summary>
    public string GetPromptText()
    {
        return hasBeenPulled ? "" : promptText;
    }
}
