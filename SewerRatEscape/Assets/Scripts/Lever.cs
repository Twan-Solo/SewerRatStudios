using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A lever the player can pull once. Fires a UnityEvent so you can
/// hook up whatever it triggers in the Inspector
/// /// </summary>
public class Lever : MonoBehaviour, IInteractable
{
    public UnityEvent onPull;
    public string promptText = "Pull Lever";

    private bool hasBeenPulled;

    public void Interact()
    {
        if (hasBeenPulled)
        {
            return;
        }

        hasBeenPulled = true;
        onPull.Invoke();

        Debug.Log(gameObject.name + " pulled ");
    }
    public string GetPromptText()
    {
        if (hasBeenPulled)
        {
            return "";
        }

        return promptText;
    }
}
