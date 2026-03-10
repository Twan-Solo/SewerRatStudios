using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Toggleable flashlight with a rechargable battery bar.
/// Battery drains when on and charges when off.
/// </summary>
public class Flashlight : MonoBehaviour
{
    [Header("Light")]
    public Light spotLight;

    [Header("Battery")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainRate = 10f;
    public float rechargeRate = 5f;

    [Header("Audio")]
    public AudioClip toggleSound;

    private bool isOn;

    private void Start()
    {
        spotLight.enabled = false;
        isOn = false;
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Toggle();
        }

        if (isOn)
        {
            currentBattery -= drainRate * Time.deltaTime;

            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                Toggle();
            }
        }
        else
        {
            currentBattery = Mathf.Min(currentBattery + rechargeRate * Time.deltaTime, maxBattery);
        }
    }

    private void Toggle()
    {
        if (!isOn && currentBattery <= 0f)
        {
            return;
        }

        isOn = !isOn;
        spotLight.enabled = isOn;

        if (toggleSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(toggleSound);
        }
    }

    public float GetBatteryNormalized()
    {
        return currentBattery / maxBattery;
    }
}