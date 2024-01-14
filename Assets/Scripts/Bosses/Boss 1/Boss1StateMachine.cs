using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1StateMachine : StateMachine
{
    [Header("Movement Variables")]
    public float moveSpeed;
    public float rotationSpeed;

    [Header("Boss Components")]
    public BossHealth health;
    public Boss1AttackManager weapons;
    public Boss1AnimationManager anim;
    public GameObject bossModel;

    [Header("Misc References")]
    public GameObject playerRef;

    [Header("Booleans")]
    public bool isAttacking;
    public bool isShooting;
    public bool isGrappling;

    void Start()
    {
        SwitchState(new Boss1MoveState(this));
    }

    public void LookAtPlayer()
    {
        Vector3 directionToPlayer = playerRef.transform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Quaternion tempRot = transform.rotation;

        tempRot.x = 0;
        tempRot.z = 0;

        transform.rotation = tempRot;
    }

    public void CheckForPlayer()
    {
        if(CheckFacingPlayer())
        {
            if(weapons.CheckMeleeRange() && weapons.canMelee)
            {
                SwitchToMeleeState();
            }
            else if (weapons.CheckCannonRange() && weapons.canShoot)
            {
                SwitchToShootState();
            }
            else if (weapons.CheckPullRange() && weapons.canGrapple)
            {
                if(weapons.grappleTarget.position.y > transform.position.y + 2f)
                {
                    SwitchToSlamState();
                }
                else
                {
                    SwitchToPullState();
                }
            }
            else if (weapons.CheckGrappleRange() && weapons.canGrapple)
            {
                SwitchToGrappleState();
            }
        }
        else
        {
            LookAtPlayer();
        }
    }

    public bool CheckFacingPlayer()
    {
        Vector3 forward = transform.forward;
        Vector3 toPlayer = (playerRef.transform.position - transform.position).normalized;

        if(Vector3.Dot(forward, toPlayer) < 0.75f)
        {
            return false;
        }

        return true;
    }

    #region State-Switchers

    public void SwitchToMoveState()
    {
        SwitchState(new Boss1MoveState(this));
    }

    public void SwitchToMeleeState()
    {
        SwitchState(new Boss1MeleeState(this));
    }

    public void SwitchToShootState()
    {
        SwitchState(new Boss1ShootState(this));
    }

    public void SwitchToGrappleState()
    {
        SwitchState(new Boss1GrappleState(this));
    }

    public void SwitchToPullState()
    {
        SwitchState(new Boss1PullState(this));
    }

    public void SwitchToSlamState()
    {
        SwitchState(new Boss1SlamState(this));
    }

    public void SwitchToStunnedState()
    {
        //SwitchState(new Boss1StunState(this));
    }

    public void SwitchToDeadState()
    {
        //SwitchState(new Boss1DeathState(this));
    }

    #endregion
}
