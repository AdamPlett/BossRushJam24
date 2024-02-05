using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public enum eB2 { none, idle, moving, attacking, teleporting, spawningLaser, staggered, dead, dancing }

public class Boss2StateMachine : StateMachine
{
    public eB2 activeState;
    [Space(6)]
    public float stateTimer;
    public float maxStateTime;

    [Header("Movement Variables")]
    public eDir moveDirection;
    public float moveSpeed;
    public float rotationSpeed;
    public bool freeze;
    public bool onGroundLayer;

    [Header("Boss Components")]
    public Boss2Health health;
    public Boss2Animation anim;
    public Boss2AttackManager attacks;
    public GameObject bossModel;
    public PortalManager portalManager;

    [Header("Environment Detection")]
    public LayerMask environment;

    [Header("Teleportation")]
    public Transform[] groundPoints;
    public Transform[] tpPoints;
    public Transform tpTarget;
    public Transform prevTarget;
    public float tpDamageThreshold;
    public float damageThresholdIncrement;

    [Header("Teleport FX")]
    public GameObject vanishPortal;
    public ParticleSystem vanishParticles;

    private float tpCounter;

    // Start is called before the first frame update
    void Start()
    {
        onGroundLayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!attacks.shootingLaser)
        {
            LookAtPlayer();
        }

        CheckHealth();
    }

    public void LookAtPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(GetDirectionToPlayer());

        RotateBoss(targetRotation);
    }

    public void RotateBoss(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Adjust x and z back to zero, thus keeping enemy level with the ground
        Quaternion tempRot = transform.rotation;

        tempRot.x = 0;
        tempRot.z = 0;

        transform.rotation = tempRot;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(gm.playerRef.transform.position, transform.position);
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (gm.playerRef.transform.position -transform.position).normalized;
    }

    private void CheckHealth()
    {
        if(health.currentHealth <= health.maxHealth - tpDamageThreshold && !gm.boss2.attacks.shootingLaser)
        {
            TeleportBoss();
            tpDamageThreshold += damageThresholdIncrement;
        }
    }

    private void TeleportBoss()
    {
        if(tpCounter > 2)
        {
            tpCounter = 0;

            int randomInt = Random.Range(0, groundPoints.Length);

            if (groundPoints[randomInt] != tpTarget)
            {
                onGroundLayer = true;
                StartCoroutine(Teleport(groundPoints[randomInt]));
            }
        }
        else
        {
            int randomInt = Random.Range(0, tpPoints.Length);

            if (tpPoints[randomInt] != tpTarget)
            {
                onGroundLayer = false;
                StartCoroutine(Teleport(tpPoints[randomInt]));
            }
        }
    }

    IEnumerator Teleport(Transform point)
    {
        tpTarget = point;

        yield return new WaitForSeconds(0.25f);

        vanishPortal.SetActive(true);

        yield return new WaitForSeconds(0.75f);

        vanishParticles.Play();

        yield return new WaitForSeconds(0.5f);

        anim.SwitchAnimation(anim.JumpHash);

        yield return new WaitForSeconds(1f);

        transform.position = point.position;

        yield return new WaitForSeconds(1f);

        vanishPortal.SetActive(false);
        anim.SwitchAnimation(anim.IdleHash);

        tpCounter++;
    }

    public void Dance()
    {
        anim.SwitchAnimation(anim.DanceHash);
    }

    public void Die()
    {
        freeze = true;
        anim.SwitchAnimation(anim.DeathHash);
    }

    #region State-Switchers

    public void SwitchToIdleState()
    {
        //SwitchState(new Boss2IdleState(this));
    }

    public void SwitchToMoveState()
    {
        SwitchState(new Boss2MoveState(this));
    }

    public void SwitchToAttackState()
    {
        SwitchState(new Boss2AttackState(this));
    }

    public void SwitchToTeleportState()
    {
        SwitchState(new Boss2TeleportState(this));
    }

    public void SwitchToStaggerState()
    {
        //SwitchState(new Boss2StaggerState(this));
    }

    public void SwitchToDeadState()
    {
        //SwitchState(new Boss2DeathState(this));
    }

    public void SwitchToDanceState()
    {
        //SwitchState(new Boss2DanceState(this));
    }

    #endregion
}
