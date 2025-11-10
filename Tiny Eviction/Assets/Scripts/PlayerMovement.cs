using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float playerRunSpeed = 5f;
    public float playerJumpForce = 7f;
    public float playerCrouchSpeed = 1f;

    [Header("Acceleration & Deceleration")]
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float velPower = 1f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool canJump;

    [Header("Gravity")]
    [SerializeField] private float walkingGravityScale = 3f;
    [SerializeField] private float jumpingGravityScale = 1f;
    [SerializeField] private float fallingGravityScale = 3f;

    [Header("Power Ups")]
    [SerializeField] float playerRollSpeed = 9f;
    [SerializeField] float rollingDelay = 0.5f;
    private bool playerCanRolyPoly = false;
    [HideInInspector] public bool playerIsRolling = false;

    [HideInInspector] public bool playerCanDoubleJump = false;
    [SerializeField] GameObject powerUpButton;

    private bool playerCanWallJump = false;
    [SerializeField] float wallJumpTime = 0.2f;
    [SerializeField] float wallSlidingSpeed = 0.3f;
    private float wallJumpTimeCounter;
    private bool isWallSliding = false;
    private float wallDistance = 0.6f;
    private RaycastHit2D wallHit;
    private float direction;
    private float slideDirection;

    [SerializeField] Vector2 wallJumpForce;

    private float groundAngle;
    [HideInInspector] public float rotationAngle;
    [HideInInspector] public float rotationInterpolation = 0.5f;
    [HideInInspector] public bool isCrouched;
    [HideInInspector] public bool isJumping;

    private bool tempCanDoubleJump;
    private float sizeX;
    private float sizeY;
    private float offsetX;
    private float offsetY;
    private float feetSizeX;
    private float feetSizeY;
    private float feetOffsetX;
    private float feetOffsetY;
    private float playerBaseSpeed;
    private bool canMove = true;

    private float ceilingCheckRadius = 1.2f;
    private bool forceCrouch = false;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Rigidbody2D playerRigidBody;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public CapsuleCollider2D playerCollider;
    [HideInInspector] public Animator playerAnimator;



    [Header("Audio")]

    [SerializeField] private AudioSource Jump;

    [SerializeField] private AudioSource Crawl;
    public bool IsNotMoving;

    [Header("Referenced Objects")]
    [SerializeField]
    CapsuleCollider2D feetCollider;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        if (Application.isEditor)
        {
            playerInput.SwitchCurrentActionMap("Test");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Mobile");
        }

        sizeX = playerCollider.size.x;
        sizeY = playerCollider.size.y;
        offsetX = playerCollider.offset.x;
        offsetY = playerCollider.offset.y;
        feetSizeX = feetCollider.size.x;
        feetSizeY = feetCollider.size.y;
        feetOffsetX = feetCollider.offset.x;
        feetOffsetY = feetCollider.offset.y;
        playerBaseSpeed = playerRunSpeed;
        canJump = true;
        slideDirection = 0;

        isCrouched = false;
        tempCanDoubleJump = playerCanDoubleJump;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            slideDirection = 0;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Run();
        if (playerRigidBody.velocity.y < 0)
        {
            playerRigidBody.gravityScale = fallingGravityScale;
        }
        UpdateRotation();
        if (playerIsRolling)
        {
            Rolling();
        }

        direction = (moveInput.x > 0) ? 1f : -1f;
        if (direction == 1)
        {
            wallHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0f), wallDistance, LayerMask.GetMask("Ground"));
        }
        else if (direction == -1)
        {
            wallHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0f), wallDistance, LayerMask.GetMask("Ground"));
        }

        if (wallHit && !IsGrounded() && moveInput.x != 0 && playerCanWallJump && slideDirection != direction)
        {
            isWallSliding = true;
            wallJumpTimeCounter = Time.time + wallJumpTime;
        }
        else if (wallJumpTimeCounter < Time.time)
        {
            isWallSliding = false;
        }
        else
        {
            wallJumpTimeCounter -= Time.deltaTime;
        }

        if (isWallSliding)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Mathf.Clamp(playerRigidBody.velocity.y, wallSlidingSpeed, float.MaxValue));
        }
    }

    private void UpdateRotation()
    {
        // flip character depending on moving direction
        bool isMovingForwards = moveInput[0] > 0;
        bool isMovingBackwards = moveInput[0] < 0;
        if (isMovingForwards)
        {
            transform.localScale = Vector3.one;
        }
        else if (isMovingBackwards)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // rotate character depending on slope it is on
        playerRigidBody.MoveRotation(Mathf.Lerp(playerRigidBody.rotation, rotationAngle, rotationInterpolation));
    }

    void OnPowerUp(InputValue inputValue)
    {
        if (playerCanRolyPoly)
        {
            playerIsRolling = !playerIsRolling;
        }
    }


    void OnMove(InputValue inputValue)
    {
        bool isPlayerMovingHorizontally = Mathf.Abs(playerRigidBody.velocity.x) > 0.01f;
        IsNotMoving = isPlayerMovingHorizontally;
        if (canMove)
        {
            moveInput = inputValue.Get<Vector2>();

        }

        if (isCrouched)
        {
            Crawl.Play();
        }
        if (isPlayerMovingHorizontally)
        {
            Crawl.Stop();
        }
    }

    void OnCrouch(InputValue inputValue)
    {
        if (!isCrouched)
        {
            playerRunSpeed = playerCrouchSpeed;
            isCrouched = true;
            rotateColliderHorizontal();
        }
        else
        {
            RaycastHit2D ceilingCollider = Physics2D.Raycast(playerCollider.bounds.center, Vector2.up, ceilingCheckRadius, 6);
            forceCrouch = ceilingCollider.collider != null ? true : false;
            if (!forceCrouch)
            {
                playerRunSpeed = playerBaseSpeed;
                isCrouched = false;
                rotateColliderVertical();
            }
        }
    }

    public void setPowerUp(int id)
    {
        // set all power ups to false
        playerCanDoubleJump = false;
        playerCanRolyPoly = false;
        playerCanWallJump = false;
        powerUpButton.SetActive(false);

        // check for each power up id and set the appropriate power up to true;
        if (id == 0)
        {
            playerCanDoubleJump = true;
        }
        else if (id == 1)
        {
            playerCanRolyPoly = true;
            powerUpButton.SetActive(true);
        }
        else if (id == 2)
        {
            playerCanWallJump = true;
        }
    }

    private void Rolling()
    {
        isCrouched = false;
        playerRunSpeed = playerRollSpeed;
        playerCollider.size = new Vector2(0.85f, 0.85f);
        playerCollider.offset = new Vector2(0, 0);
        feetCollider.offset = new Vector2(0, -0.44f);
        feetCollider.size = new Vector2(feetSizeX, feetSizeY);
        if (playerRigidBody.velocity.magnitude < 0.1f) StartCoroutine("StopRolling");
    }

    IEnumerator StopRolling()
    {
        float elapsed = 0;

        while (playerRigidBody.velocity.magnitude < 0.1f)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= rollingDelay)
            {
                StopPlayerRolling();
                yield break;
            }
        }
        yield break;
    }

    private void StopPlayerRolling()
    {
        playerIsRolling = false;
        playerRunSpeed = playerBaseSpeed;
        ResetCollider();
        feetCollider.offset = new Vector2(feetOffsetX, feetOffsetY);
        feetCollider.size = new Vector2(feetSizeX, feetSizeY);
    }


    void OnJump(InputValue inputValue)
    {
        if (coyoteTimeCounter > 0f && canJump)
        {
            Jump.Play();
            StopPlayerRolling();
            coyoteTimeCounter = 0;
            canJump = false;
            isJumping = true;
            playerRigidBody.gravityScale = jumpingGravityScale;
            playerRigidBody.AddForce(new Vector2(playerRigidBody.velocity.x, playerJumpForce), ForceMode2D.Impulse);
            Invoke("ResetcanJump", coyoteTime);
            if (!isCrouched)
            {
                rotateColliderHorizontal();
            }
        }
        else if (tempCanDoubleJump)
        {
            playerRigidBody.AddForce(new Vector2(playerRigidBody.velocity.x, playerJumpForce), ForceMode2D.Impulse);
            tempCanDoubleJump = false;
        }
        else if (playerCanWallJump && isWallSliding)
        {
            slideDirection = direction;
            playerRigidBody.velocity = Vector2.zero;
            playerRigidBody.AddForce(new Vector2(-direction * wallJumpForce.x, wallJumpForce.y), ForceMode2D.Impulse);
            StartCoroutine("PreventMove");
        }
    }
    private void ResetcanJump()
    {
        canJump = true;
    }

    IEnumerator PreventMove()
    {
        canMove = false;

        moveInput = direction == 1 ? new Vector2(-1, 1) : Vector2.one;

        yield return new WaitForSeconds(.2f);

        canMove = true;
    }

    private void ResetCollider()
    {
        playerCollider.size = new Vector2(sizeX, sizeY);
        playerCollider.offset = new Vector2(offsetX, offsetY);
    }

    void Run()
    {
        // calculate the direction we want to move in and our desired velocity
        float targetSpeed = moveInput.x * playerRunSpeed;
        //calculate the difference between current velocity and desired velocity
        float speedDif = targetSpeed - playerRigidBody.velocity.x;
        //change the acceleration rate depending on situation
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
        //finaly multiplies by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        //applies force to rigidbody, multiplying by Vector2.right so that it only affects X axis
        playerRigidBody.AddForce(movement * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isJumping = false;
            playerRigidBody.gravityScale = walkingGravityScale;
            if (playerCanDoubleJump)
            {
                tempCanDoubleJump = true;
            }
            getGroundAngle(collision);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            getGroundAngle(collision);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rotationAngle = 0;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(feetCollider.bounds.center, feetCollider.bounds.size, 0f, Vector2.down, .1f, LayerMask.GetMask("Ground"));
    }

    private void getGroundAngle(Collision2D collision)
    {
        Vector2 groundNormal = collision.GetContact(0).normal;
        groundAngle = Mathf.Atan2(groundNormal.x, groundNormal.y) * Mathf.Rad2Deg;
        rotationAngle = Mathf.Clamp(-groundAngle, -45, 45);
    }

    public void rotateColliderHorizontal()
    {
        playerCollider.direction = CapsuleDirection2D.Horizontal;
        playerCollider.size = new Vector2(sizeY, sizeX);
        playerCollider.offset = new Vector2(-0.1f, -0.1f);
        feetCollider.offset = new Vector2(feetOffsetX, -0.41f);
        feetCollider.size = new Vector2(feetSizeX * 2, feetSizeY);

    }
    public void rotateColliderVertical()
    {
        if (!isCrouched)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            playerCollider.direction = CapsuleDirection2D.Vertical;
            playerCollider.size = new Vector2(sizeX, sizeY);
            playerCollider.offset = new Vector2(offsetX, offsetY);
            feetCollider.offset = new Vector2(feetOffsetX, feetOffsetY);
            feetCollider.size = new Vector2(feetSizeX, feetSizeY);
        }
    }
}
