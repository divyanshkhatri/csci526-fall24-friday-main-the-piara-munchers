using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public delegate void MovementActions();

    public static event MovementActions disableMovement;
    public static event MovementActions enableMovements;

    [Tooltip("Put your attack animation clip")]
    [SerializeField] AnimationClip attackClip;

    private const float ANIMATION_OFFSET = 0.2f;
    private Animator anim;

    private void Awake() => anim = GetComponent<Animator>();
  
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        disableMovement?.Invoke();
        anim.SetBool("Attacking", true);
        
        yield return new WaitForSeconds(attackClip.length + ANIMATION_OFFSET);

        enableMovements?.Invoke();
        anim.SetBool("Attacking", false);
    } 
}
