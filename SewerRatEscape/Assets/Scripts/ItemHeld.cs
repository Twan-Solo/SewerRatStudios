using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHeld : MonoBehaviour
{
    [Header("Pickup Settings")]
    [Tooltip("The prefab to spawn when picked up")]
    public GameObject itemPrefab;

    [Tooltip("Empty object in front of the player to hold the item")]
    public Transform holdPoint;

    private GameObject currentItem; // stores the object currently held

    [Header("Start with Item")]
    [Tooltip("If true, player starts the game holding this item")]
    public bool startWithItem = true;

    private void Start()
    {
        // Automatically spawn the item at start if desired
        if (startWithItem && itemPrefab != null && holdPoint != null)
        {
            PickUp();
        }
    }

    // Call this to pick up the item
    public void PickUp()
    {
        if (itemPrefab == null || holdPoint == null) return;

        // Destroy previous item if any
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        // Spawn the prefab at the hold point
        currentItem = Instantiate(itemPrefab, holdPoint.position, holdPoint.rotation);
        currentItem.transform.SetParent(holdPoint);

        // Assign PlayerInput to FireProjectile
        FireProjectile fireScript = currentItem.GetComponent<FireProjectile>();
        if (fireScript != null)
        {
            fireScript.Init(GetComponent<PlayerInput>());
        }
    }

    // Call this to drop the item
    public void Drop()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
            currentItem = null;
        }
    }
}