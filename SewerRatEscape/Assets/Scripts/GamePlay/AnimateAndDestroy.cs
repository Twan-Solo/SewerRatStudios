using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEngine.InputSystem; // New Input System
#endif

public class AnimateAndDestroy : MonoBehaviour
{
    [Header("Collision Settings")]
    public string playerTag = "Player";          // Tag to detect collision

    [Header("Animation Settings (Optional)")]
    public Animator animator;                    // Animator to play animation
    public string triggerName = "Play";          // Trigger parameter name
    public float animationDuration = 1f;         // Duration to wait before destroying target

    [Header("Sound Settings (Optional)")]
    public AudioSource audioSource;              // AudioSource to play sound
    public AudioClip triggerSound;               // Sound to play once

    [Header("Object to Destroy")]
    public GameObject objectToDestroy;           // Object to destroy after delay

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        TriggerEvent();
    }

    private void TriggerEvent()
    {
        hasTriggered = true;

        // Play animation if assigned
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            bool triggerExists = false;
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                {
                    triggerExists = true;
                    break;
                }
            }

            if (triggerExists)
                animator.SetTrigger(triggerName);
            else
                Debug.LogWarning($"Animator parameter '{triggerName}' does not exist on {animator.gameObject.name}");
        }

        // Play sound once if assigned
        if (audioSource != null && triggerSound != null)
        {
            audioSource.PlayOneShot(triggerSound);
        }

        // Start coroutine to destroy target after delay
        if (objectToDestroy != null)
        {
            StartCoroutine(DestroyAfterDelay());
        }

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} triggered!");
#endif
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(animationDuration);

        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void Update()
    {
        if (hasTriggered) return;

        // F12 debug trigger
        if (Keyboard.current != null && Keyboard.current.f12Key.wasPressedThisFrame)
        {
            TriggerEvent();
        }
    }
#endif
}
