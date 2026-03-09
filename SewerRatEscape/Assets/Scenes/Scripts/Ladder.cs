using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

/// <summary>
/// Climbable ladder. Interact to mount and dismount.
/// </summary>
public class Ladder : MonoBehaviour, IInteractable
{
    public float climbSpeed = 5f;

    private CharacterController playerController;
    private FirstPersonController fpsController;
    private float savedGravity;
    private bool isClimbing;

    public string GetPromptText()
    {
        return isClimbing ? "Get Off" : "Climb";
    }

    public void Interact()
    {
        if (isClimbing)
        {
            Dismount();
        }
        else
        {
            Mount();
        }
    }

    private void Mount()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        playerController = player.GetComponent<CharacterController>();
        fpsController = player.GetComponent<FirstPersonController>();
        isClimbing = true;

        // Zero out gravity so the player doesn't fall while climbing
        if (fpsController != null)
        {
            savedGravity = fpsController.Gravity;
            fpsController.Gravity = 0f;
        }
    }

    private void Dismount()
    {
        // Restore gravity when getting off
        if (fpsController != null)
        {
            fpsController.Gravity = savedGravity;
        }

        playerController = null;
        fpsController = null;
        isClimbing = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isClimbing && other.CompareTag("Player"))
        {
            Dismount();
        }
    }

    private void Update()
    {
        if (!isClimbing || playerController == null) return;

        // Get vertical input from the player
        Vector2 moveInput = Keyboard.current.wKey.isPressed ? Vector2.up :
                            Keyboard.current.sKey.isPressed ? Vector2.down : Vector2.zero;

        if (moveInput.y != 0f)
        {
            // Bypasses normal movement and directly moves the player up or down the ladder
            Vector3 climbMove = Vector3.up * moveInput.y * climbSpeed * Time.deltaTime;
            playerController.Move(climbMove);
        }
    }
}