using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Boss1SlamState : Boss1BaseState
{
    public Boss1SlamState(Boss1StateMachine stateMachine) : base(stateMachine) { }

    private bool animationSet;

    private Vector3 playerPos;

    private float stateTimer = 0f;

    public override void Enter()
    {
        stateMachine.isGrappling = true;
        stateMachine.weapons.canGrapple = false;

        Debug.Log("Entering Slam State");

        stateMachine.anim.SwitchAnimation(stateMachine.anim.Slam1Hash);

        gm.boss1.activeState = eB1.slamming;
    }

    public override void Tick()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer > 5f)
        {
            stateMachine.SwitchToMoveState();
        }

        stateMachine.LookAtPlayer();

        if (stateMachine.weapons.noHit)
        {
            stateMachine.SwitchToMoveState();
        }
        else
        {
            if (stateMachine.weapons.pulling)
            {
                if (animationSet)
                {
                    playerPos = stateMachine.playerRef.transform.position;

                    if (stateMachine.playerRef.GetComponent<PlayerMovement>().grounded)
                    {
                        ScreenShake.Shake(1f, 2f);
                        gm.ph.DamagePlayer(stateMachine.weapons.slamDamage);

                        Instantiate(stateMachine.weapons.slamFX, gm.playerRef.transform.position, Quaternion.identity);
                        
                        stateMachine.SwitchToMoveState();
                    }
                    else
                    {
                        playerPos.y -= Time.deltaTime * stateMachine.weapons.slamSpeed;

                        stateMachine.weapons.lineRender.SetPosition(0, stateMachine.weapons.lineRender.transform.position);
                        stateMachine.weapons.lineRender.SetPosition(1, stateMachine.playerRef.transform.position);
                    }

                    stateMachine.playerRef.transform.position = playerPos;
                }
                else
                {
                    SetAnimation();
                }
            }
            else
            {
                if (stateMachine.weapons.grappleBullet.activeSelf)
                {
                    stateMachine.weapons.bulletScript.MoveBullet(3f);

                    stateMachine.weapons.lineRender.SetPosition(0, stateMachine.weapons.lineRender.transform.position);
                    stateMachine.weapons.lineRender.SetPosition(1, stateMachine.weapons.grappleBullet.transform.position);
                }
            }
        }

    }

    public override void Exit()
    {
        stateMachine.isGrappling = false;
        stateMachine.weapons.slamTimer = 0f;

        stateMachine.weapons.DeactivateGrapple();

        stateMachine.weapons.noHit = false;
        
        gm.pm.freeze = false;
        gm.boss1.weapons.pulling = false;
        Debug.Log("Exiting Slam State");
    }

    private void SetAnimation()
    {
        stateMachine.anim.SwitchAnimation(stateMachine.anim.Slam2Hash);
        animationSet = true;
    }
}
