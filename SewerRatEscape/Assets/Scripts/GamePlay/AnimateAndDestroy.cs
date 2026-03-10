using UnityEngine;
using System.Collections;

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
        if (hasTriggered) return;                 // Only trigger once
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;

        // Play animation if assigned
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            // Check if trigger exists in Animator
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
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(animationDuration);

        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }
}
