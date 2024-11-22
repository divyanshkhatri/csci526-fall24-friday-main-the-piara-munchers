using System.Collections;
using UnityEngine;

public class ClockRotation : MonoBehaviour
{
    public float rotationSpeed = 200f; // Speed of rotation
    public float waitTime = 1f; // Wait time between rotations
    private bool isRotating = false; // To control the rotation process
    private Coroutine rotationCoroutine; // Add this line

    void Update()
    {
        if (!isRotating)
        {
            rotationCoroutine = StartCoroutine(RotateHourglass()); // Modify this line
        }
    }

    IEnumerator RotateHourglass()
    {
        isRotating = true;

        // Rotate 180 degrees
        yield return RotateToAngle(transform.eulerAngles.z + 180f);

        // Wait for 1 second
        yield return new WaitForSeconds(waitTime);

        // Rotate back 180 degrees
        yield return RotateToAngle(transform.eulerAngles.z - 180f);

        // Wait for 1 second
        yield return new WaitForSeconds(waitTime);

        isRotating = false; // Allow another rotation cycle to start
    }

    IEnumerator RotateToAngle(float targetAngle)
    {
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            float direction = Mathf.Sign(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle));
            transform.Rotate(0, 0, step * direction);
            yield return null; // Wait for the next frame
        }

        // Snap to the target angle to avoid overshooting
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
    }

    public void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine); // Add this line
            rotationCoroutine = null; // Add this line
        }
        isRotating = true; // This will stop the rotation process
    }
}
