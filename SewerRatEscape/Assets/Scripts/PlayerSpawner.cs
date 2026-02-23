using UnityEngine;
using StarterAssets;
using Unity.Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (spawnPoint == null)
        {
            Debug.LogError("PlayerSpawn tag not found!");
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not assigned!");
            return;
        }

        // Spawn player facing spawn direction
        GameObject player = Instantiate(
            playerPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
        );

        // Get FirstPersonController
        FirstPersonController controller = player.GetComponent<FirstPersonController>();

        if (controller != null)
        {
            // Find Cinemachine Camera (CM3 uses CinemachineCamera)
            CinemachineCamera vcam = FindObjectOfType<CinemachineCamera>();

            if (vcam != null)
            {
                vcam.Follow = controller.CinemachineCameraTarget.transform;
                vcam.LookAt = controller.CinemachineCameraTarget.transform;
            }
            else
            {
                Debug.LogError("CinemachineCamera not found in scene!");
            }
        }
    }
}