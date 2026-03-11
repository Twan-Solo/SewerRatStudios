using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI bar that shows the flashlight battery level.
/// </summary>
public class BatteryUI : MonoBehaviour
{
    public Image fillBar;
    public Color fullColor = Color.green;
    public Color emptyColor = Color.red;

    private Flashlight flashlight;

    private void Update()
    {
        if (flashlight == null)
        {
            flashlight = FindAnyObjectByType<Flashlight>();
            if (flashlight == null) return;
        }

        float battery = flashlight.GetBatteryNormalized();
        fillBar.fillAmount = battery;

        fillBar.color = Color.Lerp(emptyColor, fullColor, battery);
    }
}