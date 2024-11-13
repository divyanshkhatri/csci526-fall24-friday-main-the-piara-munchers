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

    private readonly Color flipColor = new Color(0.537f, 0.925f, 0.671f); // #664343
    private readonly Color defaultColor = Color.white; 
    private readonly Color defaultBackgroundColor = new Color(0.149f, 0.443f, 0.502f); // #257180
    private readonly Color flipBackgroundColor = new Color(0.1176f, 0.2627f, 0.2941f); // #1E434B

    public static bool IsFlipped
    {
        get { return isFlipped; }
    }

    void Start()
    {
        PauseManager.OnPause += HandlePause;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.color = defaultColor; 

        // Set initial background color
        Camera.main.backgroundColor = defaultBackgroundColor;

        foreach (GameObject obj in flippableObjects)
        {
            if (obj == null) continue;
            if (obj.CompareTag("Walkable_plain"))
            {
                SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (objSpriteRenderer != null)
                {
                    originalColors[obj] = objSpriteRenderer.color;
                }
            }

            foreach (Transform child in obj.transform)
            {
                if (child.CompareTag("Walkable_plain"))
                {
                    SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
                    if (childSpriteRenderer != null)
                    {
                        originalColors[child.gameObject] = childSpriteRenderer.color;
                    }
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

        // Toggle player color based on flip state
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = isFlipped ? flipColor : defaultColor;
        }

        // Toggle background color based on flip state
        Camera.main.backgroundColor = isFlipped ? flipBackgroundColor : defaultBackgroundColor;

        foreach (GameObject obj in flippableObjects)
        {
            Debug.Log("Flippable Object: " + obj.name + ", Tag: " + obj.tag);

            if (obj != player.gameObject)
            {
                float relativePositionX = obj.transform.position.x - player.position.x;
                relativePositionX = -relativePositionX;
                obj.transform.position = new Vector3(player.position.x + relativePositionX, obj.transform.position.y, obj.transform.position.z);

                Vector3 scale = obj.transform.localScale;
                scale.x *= -1;
                obj.transform.localScale = scale;
            }

            HandleWalkablePlain(obj);

            foreach (Transform child in obj.transform)
            {
                if (child.CompareTag("Walkable_plain"))
                {
                    HandleWalkablePlain(child.gameObject);
                }
            }
        }
    }

    private void HandleWalkablePlain(GameObject obj)
    {
        SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (objSpriteRenderer != null)
        {
            if (originalColors.ContainsKey(obj))
            {
                objSpriteRenderer.color = originalColors[obj];
            }
        }
    }
}
