using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassFlip : MonoBehaviour
{
private bool isFlipped = false;  // Track if the compass is flipped or not
    private SpriteRenderer spriteRenderer;  // Reference to the compass's sprite renderer

    void Start()
    {
        // Get the SpriteRenderer component of the compass (or whatever component you want to flip)
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if the "Back" button is pressed (you can adjust the key if needed)
        if (Input.GetKeyDown(KeyCode.LeftArrow))  // 'Escape' is commonly used for back actions on PC
        {
            FlipCompass();
        }
    }

    void FlipCompass()
    {
        // Flip the compass sprite by changing its local scale on the x-axis
        if (spriteRenderer != null)
        {
            isFlipped = !isFlipped;
            Vector3 newScale = transform.localScale;
            newScale.x = isFlipped ? -Mathf.Abs(newScale.x) : Mathf.Abs(newScale.x);  // Flip the scale
            transform.localScale = newScale;
        }
    }
}
