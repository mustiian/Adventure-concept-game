using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = PlayerManager.instance.GetComponent<PlayerMovement>();
    }

    public void MovementAnimation()
    {
        if (playerMovement.Direction.x == 0 && playerMovement.Direction.z == 0)
        {
            animator.SetTrigger("Idle");
        }
        else if (playerMovement.Direction.x > 0 && playerMovement.Direction.z == 0)
        {
            animator.SetTrigger("WalkRight");
        }
        else if (playerMovement.Direction.x < 0 && playerMovement.Direction.z == 0)
        {
            animator.SetTrigger("WalkLeft");
        }
        else if (playerMovement.Direction.z < 0 )
        {
            animator.SetTrigger("WalkFront");
        }
        else if (playerMovement.Direction.z > 0)
        {
            animator.SetTrigger("WalkBack");
        }
    }
}
