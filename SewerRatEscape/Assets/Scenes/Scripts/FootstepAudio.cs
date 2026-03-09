using UnityEngine;

[RequireComponent(typeof(StarterAssets.StarterAssetsInputs))]
[RequireComponent(typeof(CharacterController))]
public class FootstepAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    public AudioClip footstepSound;       // Assign in Inspector
    public float footstepInterval = 0.5f; // Time between steps

    private CharacterController _controller;
    private StarterAssets.StarterAssetsInputs _input;
    private float footstepTimer;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_input.move.magnitude > 0.1f && _controller.isGrounded)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                if (AudioManager.Instance != null && footstepSound != null)
                {
                    AudioManager.Instance.PlaySFX(footstepSound);
                }

                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // reset timer when not moving
        }
    }
}
