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

    private bool isOn;

    private void Start()
    {
        // Start with the flashlight off
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

            // Ran out, force it off
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
        // Don't let them turn it on with a dead battery
        if (!isOn && currentBattery <= 0f)
        {
            return;
        }

        isOn = !isOn;
        spotLight.enabled = isOn;
    }

    /// <summary>
    /// Returns battery as a 0 to 1 value for UI display.
    /// </summary>
    public float GetBatteryNormalized()
    {
        return currentBattery / maxBattery;
    }
}
