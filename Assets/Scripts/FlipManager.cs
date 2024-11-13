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

//     public static bool IsFlipped
//     {
//         get { return isFlipped; }
//     }

//     void Start()
//     {
//         PauseManager.OnPause += HandlePause;
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
//             if (OnFlip != null)
//             {
//                 OnFlip.Invoke();
//             }
//         }
//     }

//     void ToggleFlip()
//     {
//         isFlipped = !isFlipped;

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

public class FlipManager : MonoBehaviour
{
    public Transform player;
    public GameObject[] flippableObjects;
    public KeyCode flipKey = KeyCode.LeftArrow;
    private static bool isFlipped = false;
    public static event Action OnFlip;
    private bool canFlip = true;
    private Renderer playerRenderer;

    public static bool IsFlipped
    {
        get { return isFlipped; }
    }

    void Start()
    {
        PauseManager.OnPause += HandlePause;
        playerRenderer = player.GetComponent<Renderer>();
        playerRenderer.material.color = Color.white; // Set initial color to white
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
        if (playerRenderer != null)
        {
            playerRenderer.material.color = playerRenderer.material.color == Color.white ? Color.black : Color.white;
        }

        foreach (GameObject obj in flippableObjects)
        {
            if (obj != player.gameObject)
            {
                float relativePositionX = obj.transform.position.x - player.position.x;
                relativePositionX = -relativePositionX;
                obj.transform.position = new Vector3(player.position.x + relativePositionX, obj.transform.position.y, obj.transform.position.z);

                Vector3 scale = obj.transform.localScale;
                scale.x *= -1;
                obj.transform.localScale = scale;
            }
        }
    }
}
