using UnityEngine;

/// <summary>
/// Applies camera shake after Cinemachine updates by offsetting the camera in LateUpdate.
/// </summary>
[DefaultExecutionOrder(1000)]
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.08f;
    public float shakeSpeed = 25f;

    private float shakeTimer;
    private float currentForce;
    private float seed;
    private Transform camTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Shake(float force = 1f)
    {
        shakeTimer = shakeDuration;
        currentForce = force;
        // Random seed so each shake feels different
        seed = Random.Range(0f, 1000f);
    }

    private void LateUpdate()
    {
        if (shakeTimer <= 0f) return;

        if (camTransform == null)
        {
            if (Camera.main != null)
            {
                camTransform = Camera.main.transform;
            }
            else
            {
                return;
            }
        }

        float progress = shakeTimer / shakeDuration;
        float magnitude = shakeMagnitude * currentForce * progress;

        // Perlin noise gives smooth continuous movement instead of random jerking
        float time = Time.time * shakeSpeed;
        float x = (Mathf.PerlinNoise(seed, time) * 2f - 1f) * magnitude;
        float y = (Mathf.PerlinNoise(seed + 100f, time) * 2f - 1f) * magnitude;

        camTransform.localPosition += new Vector3(x, y, 0f);

        shakeTimer -= Time.deltaTime;
    }
}