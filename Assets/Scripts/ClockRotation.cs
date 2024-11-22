using System.Collections;
using UnityEngine;

public class ClockRotation : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float waitTime = 1f;
    private bool isRotating = false;
    private Coroutine rotationCoroutine;

    void Update()
    {
        if (!isRotating)
        {
            rotationCoroutine = StartCoroutine(RotateHourglass());
        }
    }

    IEnumerator RotateHourglass()
    {
        isRotating = true;

        yield return RotateToAngle(transform.eulerAngles.z + 180f);

        yield return new WaitForSeconds(waitTime);

        yield return RotateToAngle(transform.eulerAngles.z - 180f);

        yield return new WaitForSeconds(waitTime);

        isRotating = false;
    }

    IEnumerator RotateToAngle(float targetAngle)
    {
        float currentAngle = transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        while (Mathf.Abs(deltaAngle) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            step = Mathf.Min(step, Mathf.Abs(deltaAngle));
            float direction = Mathf.Sign(deltaAngle);

            currentAngle += step * direction;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
        isRotating = false;
    }
}
