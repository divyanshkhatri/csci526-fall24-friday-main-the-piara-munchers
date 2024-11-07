using UnityEngine;
using UnityEngine.UI;

public class LaserCountdownTimer : MonoBehaviour
{
    public Text countdownText;        // Reference to the Text UI element for displaying countdown
    public float laserBlinkInterval = 5f;  // Time interval for the laser blinking (e.g., every 5 seconds)
    private float countdown;

    void Start()
    {
        countdown = laserBlinkInterval;
        countdownText.gameObject.SetActive(true);  // Ensure the countdown text is visible initially
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown > 0)
        {
            countdownText.text = "Laser in: " + Mathf.Ceil(countdown).ToString();
        }
        else
        {
            countdownText.text = "Laser Active!";
            ResetCountdown();  // Reset the countdown when it reaches zero
        }
    }

    public void ResetCountdown()
    {
        countdown = laserBlinkInterval;  // Reset the countdown for the next laser blink cycle
    }
}
