using UnityEngine;

public class RespawnTriggerNoDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        RespawnPlayer(other.gameObject);
    }

    private void RespawnPlayer(GameObject player)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (spawnPoint == null)
        {
            Debug.LogError("PlayerSpawn not found!");
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.transform.position = spawnPoint.transform.position;

        if (controller != null)
            controller.enabled = true;

        Debug.Log("Player Respawned");
    }
}
