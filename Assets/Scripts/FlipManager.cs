// using UnityEngine;
// using System;

// public class FlipManager : MonoBehaviour
// {
//     public Transform player;
//     public GameObject[] flippableObjects;
//     public KeyCode flipKey = KeyCode.LeftArrow;
//     private static bool isFlipped = false;
//     public static event Action OnFlip;
//     private bool canFlip = true;
//     private Renderer playerRenderer;

//     public static bool IsFlipped
//     {
//         get { return isFlipped; }
//     }

//     void Start()
//     {
//         PauseManager.OnPause += HandlePause;
//         playerRenderer = player.GetComponent<Renderer>();
//         playerRenderer.material.color = Color.white; // Set initial color to white
//     }

//     void OnDestroy()
//     {
//         PauseManager.OnPause -= HandlePause;
//     }

//     void HandlePause(bool isPaused)
//     {
//         canFlip = !isPaused;
//     }

//     void Update()
//     {
//         if (canFlip && Input.GetKeyDown(flipKey))
//         {
//             ToggleFlip();
//             OnFlip?.Invoke();
//         }
//     }

//     void ToggleFlip()
//     {
//         isFlipped = !isFlipped;

//         // Flip player color
//         if (playerRenderer != null)
//         {
//             playerRenderer.material.color = playerRenderer.material.color == Color.white ? Color.black : Color.white;
//         }

//         foreach (GameObject obj in flippableObjects)
//         {
//             if (obj != player.gameObject)
//             {
//                 float relativePositionX = obj.transform.position.x - player.position.x;
//                 relativePositionX = -relativePositionX;
//                 obj.transform.position = new Vector3(player.position.x + relativePositionX, obj.transform.position.y, obj.transform.position.z);

//                 Vector3 scale = obj.transform.localScale;
//                 scale.x *= -1;
//                 obj.transform.localScale = scale;
//             }
//         }
//     }
// }

using UnityEngine;
using System;
using System.Collections.Generic;

public class FlipManager : MonoBehaviour
{
    public Transform player;
    public GameObject[] flippableObjects;
    public KeyCode flipKey = KeyCode.LeftArrow;
    private static bool isFlipped = false;
    public static event Action OnFlip;
    private bool canFlip = true;
    private SpriteRenderer playerSpriteRenderer;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    public static bool IsFlipped
    {
        get { return isFlipped; }
    }

    void Start()
    {
        PauseManager.OnPause += HandlePause;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.color = Color.white; // Set initial color to white

        // Store original colors of objects tagged as "Walkable_plain"
        foreach (GameObject obj in flippableObjects)
        {
            if (obj.CompareTag("Walkable_plain"))
            {
                SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (objSpriteRenderer != null)
                {
                    originalColors[obj] = objSpriteRenderer.color;
                }
            }
        }
    }

    void OnDestroy()
    {
        PauseManager.OnPause -= HandlePause;
    }

    void HandlePause(bool isPaused)
    {
        canFlip = !isPaused;
    }

    void Update()
    {
        if (canFlip && Input.GetKeyDown(flipKey))
        {
            ToggleFlip();
            OnFlip?.Invoke();
        }
    }

    void ToggleFlip()
    {
        isFlipped = !isFlipped;

        // Flip player color
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = playerSpriteRenderer.color == Color.white ? Color.black : Color.white;
        }

        foreach (GameObject obj in flippableObjects)
        {
            if (obj != player.gameObject)
            {
                // Flip position and scale for non-player objects
                float relativePositionX = obj.transform.position.x - player.position.x;
                relativePositionX = -relativePositionX;
                obj.transform.position = new Vector3(player.position.x + relativePositionX, obj.transform.position.y, obj.transform.position.z);

                Vector3 scale = obj.transform.localScale;
                scale.x *= -1;
                obj.transform.localScale = scale;
            }

            // Change color of sprites tagged as "Walkable_plain"
            if (obj.CompareTag("Walkable_plain"))
            {
                SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (objSpriteRenderer != null)
                {
                    if (isFlipped)
                    {
                        // Change to black when flipped
                        objSpriteRenderer.color = Color.black;
                    }
                    else if (originalColors.ContainsKey(obj))
                    {
                        // Revert to the original color when unflipped
                        objSpriteRenderer.color = originalColors[obj];
                    }
                }
            }
        }
    }
}
