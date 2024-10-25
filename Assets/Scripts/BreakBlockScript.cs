using System.Collections;
using UnityEngine;

public class BreakBlockScript : MonoBehaviour
{
    public float timeToBreak = 0.5f;
    public float respawnTime = 1.0f;
    private bool isPlayerOnBlock = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 offScreenPosition = new Vector3(-1000, -1000, 0);
    private float initialTimeToBreak;
    private Vector3 initialRelativePosition;
    public Transform referenceObject;
    private bool isFlipped = false;

    void Start()
    {

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialTimeToBreak = timeToBreak;
        if (referenceObject != null)
            initialRelativePosition = transform.position - referenceObject.position;
        else
            Debug.LogError("Reference object not set for BreakBlockScript.");
        FlipManager.OnFlip += OnWorldFlip;

    }
    void OnDestroy()
    {
        FlipManager.OnFlip -= OnWorldFlip;
    }

    void Update()
    {

        if (isPlayerOnBlock)
        {
            timeToBreak -= Time.deltaTime;
            if (timeToBreak <= 0)
            {
                StartCoroutine(BreakAndRespawnBlock());
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBlock = true;
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBlock = false;
            timeToBreak = initialTimeToBreak;
        }
    }


    IEnumerator BreakAndRespawnBlock()
    {



        transform.position = offScreenPosition;



        yield return new WaitForSeconds(respawnTime);
        Vector3 newPosition = referenceObject.position + initialRelativePosition;
        if (isFlipped)
        {
            newPosition.x = referenceObject.position.x - initialRelativePosition.x;
        }


        transform.position = newPosition;
        transform.rotation = initialRotation;



        timeToBreak = initialTimeToBreak;
    }
    private void OnWorldFlip()
    {
        isFlipped = !isFlipped;
    }
}
