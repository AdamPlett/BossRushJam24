using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask grappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;
    public CrosshairManager cm;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;


    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        if (grappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }
    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;
        //switches crosshair to grapple crosshair
        cm.grappleCrosshair(true);

        grappling = true;

        //Freezes the players ability to move
        //pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grappleable))
        {
            grapplePoint = hit.point;
            cm.changeGrappleColor(new Color(0, 255, 0));
            //(nameof(cm.changeGrappleColor(new Color(0, 255, 255))), .25);
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            cm.changeGrappleColor(new Color(255, 0, 0));
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        //creates the grapple rope
        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        //pm.freeze = false;
        cm.changeGrappleColor(new Color(0, 255, 255));
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }
    public void StopGrapple()
    {
        grappling = false;
        //allows player movement
        //pm.freeze = false;

        //starts cooldown
        grapplingCdTimer = grapplingCd;
        //disables line renderer (grapple rope)
        lr.enabled = false;
        //restores default color and disables grappling crosshair
        cm.changeGrappleColor(new Color(0, 255, 255));
        cm.grappleCrosshair(false);
    }
}
