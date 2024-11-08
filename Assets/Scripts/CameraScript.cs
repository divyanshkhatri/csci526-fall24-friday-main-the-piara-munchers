using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.4f;
    public float heightOffsetPercentage = 0.15f;

    void LateUpdate()
    {

        Camera cam = GetComponent<Camera>();
        float cameraHeight;


        if (cam.orthographic)
        {
            cameraHeight = cam.orthographicSize * 2f;
        }
        else
        {
            cameraHeight = 2f * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * Mathf.Abs(transform.position.z - player.position.z);
        }


        float heightOffset = cameraHeight * heightOffsetPercentage;


        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y + heightOffset, transform.position.z);


        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);


        transform.position = smoothedPosition;
    }
}
