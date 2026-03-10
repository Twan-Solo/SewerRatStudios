using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    public AudioClip footstepSound;

    private CharacterController controller;
    private StarterAssetsInputs input;
    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<StarterAssetsInputs>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = footstepSound;
        audioSource.loop = true;
    }

    void Update()
    {
        bool isMoving = input.move != Vector2.zero && controller.isGrounded;

        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isMoving && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}