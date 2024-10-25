<<<<<<< HEAD
=======
<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowRotation : MonoBehaviour
{
    public KeyCode rotateKey = KeyCode.LeftArrow; // Key to rotate the arrow
    private bool isFlipped = false;  // Tracks whether the arrow is flipped

    void Update()
    {
        // Detect if the rotate key is pressed
        if (Input.GetKeyDown(rotateKey))
        {
            RotateArrow();
        }
    }

    void RotateArrow()
    {
        // Toggle between flipping 180 degrees and reverting back
        if (isFlipped)
        {
            // Reset to the original direction
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            // Flip 180 degrees on the Z-axis (as needed for 2D)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        // Toggle the flipped state
        isFlipped = !isFlipped;
    }
}

=======
<<<<<<< HEAD
>>>>>>> 9012d2579bda88cba695b229e29cda736f046d4b
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowRotation : MonoBehaviour
{
    public KeyCode rotateKey = KeyCode.LeftArrow; // Key to rotate the arrow
    private bool isFlipped = false;  // Tracks whether the arrow is flipped

    void Update()
    {
        // Detect if the rotate key is pressed
        if (Input.GetKeyDown(rotateKey))
        {
            RotateArrow();
        }
    }

    void RotateArrow()
    {
        // Toggle between flipping 180 degrees and reverting back
        if (isFlipped)
        {
            // Reset to the original direction
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            // Flip 180 degrees on the Z-axis (as needed for 2D)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        // Toggle the flipped state
        isFlipped = !isFlipped;
    }
}

<<<<<<< HEAD
=======
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowRotation : MonoBehaviour
{
    public KeyCode rotateKey = KeyCode.LeftArrow; // Key to rotate the arrow
    private bool isFlipped = false;  // Tracks whether the arrow is flipped

    void Update()
    {
        // Detect if the rotate key is pressed
        if (Input.GetKeyDown(rotateKey))
        {
            RotateArrow();
        }
    }

    void RotateArrow()
    {
        // Toggle between flipping 180 degrees and reverting back
        if (isFlipped)
        {
            // Reset to the original direction
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            // Flip 180 degrees on the Z-axis (as needed for 2D)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        // Toggle the flipped state
        isFlipped = !isFlipped;
    }
}

>>>>>>> 4a1ea9a9493455a0e27dafbcdf67fdfbf589e2aa
>>>>>>> 074fd6c3a85606bdc5b933751e7cdfc8c8c50d00
>>>>>>> 9012d2579bda88cba695b229e29cda736f046d4b
