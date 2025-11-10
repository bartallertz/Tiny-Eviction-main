using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovementScript;
    Animator playerAnimator;
    private bool isLanding = false;
    private SpriteRenderer playerRenderer;
    [HideInInspector] public bool isHurt = false;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }
    private void UpdateAnimationState()
    {
        // toggle running animation
        bool isPlayerMovingHorizontally = Mathf.Abs(playerMovementScript.playerRigidBody.velocity.x) > 0.01f;
        playerAnimator.SetBool("isRunning", isPlayerMovingHorizontally);

        // toggle crouching animation
        playerAnimator.SetBool("isCrouching", playerMovementScript.isCrouched);

        // toggle jumping/falling animation
        float verticalMovement = playerMovementScript.playerRigidBody.velocity.y;
        bool isPlayerMovingVertically = ((Mathf.Abs(verticalMovement) > Mathf.Epsilon) && !playerMovementScript.IsGrounded());
        playerAnimator.SetBool("isVerticalMovement", isPlayerMovingVertically);
        playerAnimator.SetBool("isJumping", playerMovementScript.isJumping);
        playerAnimator.SetFloat("yVelocity", verticalMovement);
        isLanding = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Landing");

        // toggle rolling animation
        playerAnimator.SetBool("isRolling", playerMovementScript.playerIsRolling);

        // toggle hurt animation
        playerAnimator.SetBool("isHurt", isHurt);
    }

    private void rotateColliderVertical(){
        playerMovementScript.rotateColliderVertical();
    }

    private void rotateColliderHorizontal(){
        playerMovementScript.rotateColliderHorizontal();
    }

    private void resetIsHurt()
    {
        isHurt = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();
    }
}
