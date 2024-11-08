using UnityEngine;
using UnityEngine.UI;

public class LaserCountdownTimer : MonoBehaviour
{
    public Text countdownText;
    public float laserBlinkInterval = 5f;
    private float countdown;

    void Start()
    {
        countdown = laserBlinkInterval;
        countdownText.gameObject.SetActive(true);
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
            ResetCountdown();
        }
    }

    public void ResetCountdown()
    {
        countdown = laserBlinkInterval;
    }
}
