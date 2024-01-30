    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Wallrunning")]
    public LayerMask wall;
    public LayerMask ground;
    public float wallRunForce;
    public float wallJumpForceUp;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    public GameObject wallRef;
    public GameObject lastWall;
    public float coyoteTime;
    private float coyoteTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallRight;
    private bool wallLeft;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    //High counter means lower gravity
    public float gravityCounterForce;


    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovement pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        //coyote time logic
        coyoteTimer -= Time.deltaTime;
        if (pm.wallrunning) coyoteTimer = coyoteTime;
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, ground);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, ground);

        if (wallLeft && wallRef != leftWallhit.transform.gameObject)
        {
            wallRef = leftWallhit.transform.gameObject;
        }
        if (wallRight && wallRef != rightWallhit.transform.gameObject)
        {
            wallRef = rightWallhit.transform.gameObject;
        }
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    
        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
            {
                if (wallRef != lastWall)
                {
                    StartWallRun();
                }
            }

            // wallrun timer
            if (wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            if (wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(jumpKey) && coyoteTimer > 0) WallJump();
        }
        // State 2 - Exiting Wall
        else if (exitingWall)
        {
            if(pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;

            //if(Input.GetKeyDown(jumpKey) && coyoteTimer > 0) WallJump();
        }
        // State 3 - None
        else
        {
            if (pm.wallrunning)
            {
                StopWallRun();
            }
            if (pm.grounded)
            {
                lastWall = null;
                wallRef = null;
            }

            if (Input.GetKeyDown(jumpKey) && coyoteTimer > 0) WallJump();
        }


    
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //changes fov of cam
        cam.DoFov(95f, .25f);
        //rotates the camera to tilt away from wall
        if (wallLeft) cam.DoTilt(-5f, .25f);
        if (wallRight) cam.DoTilt(5f, .25f);

        lastWall = wallRef;

    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        // weaken gravity
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        // reset cam fx
        cam.DoFov(80f, .25f);
        cam.DoTilt(0f, .25f);

    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpForceUp + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

    }
}