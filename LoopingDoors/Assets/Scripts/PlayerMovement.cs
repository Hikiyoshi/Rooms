using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private TMP_Text text;

    [Header("Keybinds"), Space]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement"), Space]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float intervalRunning;
    [SerializeField] private float intervalWaitToRun;

    [Header("Jump"), Space]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    [Header("Crouch"), Space]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeightScale;

    [Header("Ground Check"), Space]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;

    private float verticalInput, horizontalInput;
    private Vector3 moveDirection;
    private float moveSpeed;
    private float _intervalRunning;
    private float _intervalWaitToRun;
    private bool isGrounded;
    private bool canRun = true;
    private bool canJump = true;
    private float startHeightScale;
    private MovementState state;
    public enum MovementState
    {
        walking,
        running,
        crouching,
        jumping,
    }

    private void Start()
    {
        rb.freezeRotation = true;

        startHeightScale = transform.localScale.y;
    }

    private void Update()
    {
        CheckIsGrounded();

        GetInput();

        MovementStateHandler();

        SpeedControl();

        text.text = "Speed: " + new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        ApplyDrag();

        RunningHandler();
    }

    private void RunningHandler()
    {
        //Rest after running for a period of time
        if (state != MovementState.running)
        {
            _intervalWaitToRun -= Time.deltaTime;
            _intervalRunning = intervalRunning;
        }
        else
        {
            //When player is running
            _intervalRunning -= Time.deltaTime;
        }

        if (_intervalWaitToRun < 0f)
        {
            canRun = true;
            _intervalWaitToRun = intervalWaitToRun;
        }

        if (_intervalRunning < 0f)
        {
            canRun = false;
            _intervalRunning = intervalRunning;
        }
    }

    private void MovementStateHandler()
    {
        //Walking State
        state = MovementState.walking;
        moveSpeed = walkSpeed;

        if (!isGrounded)
        {
            //Jumping State
            state = MovementState.jumping;
            return;
        }

        if (Input.GetKey(crouchKey))
        {
            //Crouching State
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            return;
        }

        if (Input.GetKey(runKey) && canRun)
        {
            //Running State
            state = MovementState.running;
            moveSpeed = runSpeed;
            return;
        }
    }

    private void GetInput()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //Run
        if (Input.GetKeyDown(runKey) && canRun)
        {
            _intervalWaitToRun = intervalWaitToRun;
        }

        //Check if not on ground, can not jump or crouch
        if (!isGrounded)
        {
            return;
        }

        //Jump
        if (Input.GetKey(jumpKey) && canJump)
        {
            canJump = false;
            Debug.Log("Jump");
            HandleJump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeightScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startHeightScale, transform.localScale.z);
        }
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, groundLayer);
    }

    private void ApplyDrag()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
            return;
        }

        rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MoveHandler();
    }

    private void MoveHandler()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (isGrounded)
        {
            //On Ground
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            //In Air
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        //Limit velocity if needed
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void HandleJump()
    {
        //Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        canJump = true;
    }
}
