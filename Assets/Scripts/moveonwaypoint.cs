using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveonwaypoint : MonoBehaviour
{
    public List<GameObject> waypoints;
    public float speed = 3;
    int index = 0;

    void Start()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Waypoints list is not initialized or empty.");
            return;
        }

        Debug.Log("Waypoints initialized with " + waypoints.Count + " points.");
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            return;
        }

        Vector3 destination = waypoints[index].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newPos;

        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.05f)
        {
            if (index < waypoints.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0; // Loop back to the first waypoint
            }
        }
    }
}
