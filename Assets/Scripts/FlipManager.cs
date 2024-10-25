using UnityEngine;
using System;

public class FlipManager : MonoBehaviour
{
    public Transform player;
    public GameObject[] flippableObjects;
    public KeyCode flipKey = KeyCode.LeftArrow;
    private static bool isFlipped = false;
    public static event Action OnFlip;

    public static bool IsFlipped
    {
        get { return isFlipped; }
    }
    void Update()
    {

        if (Input.GetKeyDown(flipKey))
        {
            ToggleFlip();
            if (OnFlip != null)
            {
                OnFlip.Invoke();
            }
        }
    }



    void ToggleFlip()
    {
        isFlipped = !isFlipped;

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
