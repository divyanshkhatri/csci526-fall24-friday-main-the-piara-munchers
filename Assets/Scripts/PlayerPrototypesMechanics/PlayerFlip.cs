using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private bool isFacingLeft;
    
    public void OnFlip()
    {
        if (IsWalkingLookingWrong())
            Flip(); 
    }

    void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private bool IsWalkingLookingWrong() => Input.GetAxisRaw("Horizontal") > 0 && isFacingLeft ||
                                            Input.GetAxisRaw("Horizontal") < 0 && !isFacingLeft;
    
}
