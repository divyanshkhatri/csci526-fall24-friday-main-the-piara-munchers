using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveonwaypoint : MonoBehaviour
{
    public List<GameObject> waypoints;
    public float speed = 3;
    int index = 0 ;



    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
    Vector3 destination = waypoints[index].transform.position;
    Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position,speed * Time.deltaTime);
    transform.position = newPos;

    float distance = Vector3.Distance(transform.position,destination);
    if (distance <= 0.05)
    {
        if(index < waypoints.Count-1)
        index++;
    }

    }

    // private void OnCollisionEnter2D(other:Collider)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.gameObject.transform.parent = transform;
    //     }
    // }

    // private void OnCollisionExit2D(other:Collider)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.gameObject.transform.parent = null;
    //     }
    // }

}
