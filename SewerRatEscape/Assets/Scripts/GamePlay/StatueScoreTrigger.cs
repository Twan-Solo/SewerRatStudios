using UnityEngine;

public class StatueScoreTrigger : MonoBehaviour
{
    [Header("Score Settings")]
    public int scoreValue = 10;

    [Header("Trigger Settings")]
    public bool destroyAfterTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        // Only allow the player to trigger this
        if (!other.transform.root.CompareTag("Player"))
            return;

        Debug.Log("Player collected statue. Score added: " + scoreValue);

        ScoreCounter.Instance?.AddScore(scoreValue);

        if (destroyAfterTrigger)
            Destroy(gameObject);
    }
}