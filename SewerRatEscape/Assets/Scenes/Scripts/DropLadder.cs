using UnityEngine;
using System.Collections;

/// <summary>
/// Drops the ladder into position when triggered.
/// </summary>
public class DropLadder : MonoBehaviour
{
    public float dropDistance = 5f;
    public float dropSpeed = 2f;

    [Header("Audio")]
    public AudioClip dropSound;

    private bool hasDropped;

    public void Drop()
    {
        if (hasDropped) return;
        hasDropped = true;
        StartCoroutine(DoDrop());
    }

    private IEnumerator DoDrop()
    {
        if (dropSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(dropSound);
        }

        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * dropDistance;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * dropSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }
}
