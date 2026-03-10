using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    [Header("Audio")]
    public AudioSource audioSource;    // Assign in inspector
    public AudioClip hoverSound;       // Sound when button gets focus
    public AudioClip clickSound;       // Sound when clicked

    [Header("Optional Animation")]
    public Animator animator;          // Assign if you want animations later
    public string highlightBool = "Highlighted"; // Animator bool for highlight
    public string clickTrigger = "Click";       // Animator trigger for click

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Mouse hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
    }

    // Mouse or controller click
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
        ClickAnimation();
    }

    // Keyboard/controller select
    public void OnSelect(BaseEventData eventData)
    {
        Highlight();
    }

    // Keyboard/controller deselect
    public void OnDeselect(BaseEventData eventData)
    {
        if (animator != null)
            animator.SetBool(highlightBool, false);
    }

    private void Highlight()
    {
        // Play hover sound
        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);

        // Trigger animation if assigned
        if (animator != null)
            animator.SetBool(highlightBool, true);
    }

    private void ClickAnimation()
    {
        if (animator != null)
            animator.SetTrigger(clickTrigger);
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}