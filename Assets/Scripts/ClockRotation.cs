using UnityEngine;

public class ClockRotation : MonoBehaviour
{
    public float rotationSpeed = 200f;
    private bool shouldRotate = true;
    private float targetAngle;
    private float currentDelay;
    public float delayBetweenRotations = 1f;

    void Start()
    {
        targetAngle = transform.eulerAngles.z + 180f;
        currentDelay = delayBetweenRotations;
    }

    void Update()
    {
        if (!shouldRotate) return;

        float currentAngle = transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (Mathf.Abs(deltaAngle) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            float rotation = Mathf.MoveTowards(0, deltaAngle, step);
            transform.Rotate(0, 0, rotation);
            currentDelay = delayBetweenRotations;
        }
        else
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                targetAngle = transform.eulerAngles.z + 180f;
                currentDelay = delayBetweenRotations;
            }
        }
    }

    public void StopRotation()
    {
        shouldRotate = false;
    }

    public void StartRotation()
    {
        shouldRotate = true;
    }
}
