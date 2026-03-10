using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    public AudioClip footstepSound;
    public float stepInterval = 0.5f;

    private CharacterController controller;
    private StarterAssetsInputs input;
    private AudioSource audioSource;

    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<StarterAssetsInputs>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isMoving = input.move != Vector2.zero;

        if (controller.isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(footstepSound);
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}
