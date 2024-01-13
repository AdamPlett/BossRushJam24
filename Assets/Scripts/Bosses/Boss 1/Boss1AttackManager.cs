using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMine { energy, explosive }

public class Boss1AttackManager : MonoBehaviour
{
    [Header("Energy Sword")]
    [SerializeField] private GameObject blade;

    [Space(8)]
    public float meleeRange;
    public float meleeDamage;
    public float meleeCooldown;
    public float meleeTimer;
    public bool canMelee;
    [Space(8)]
    public int comboCounter;

    [Header("Cannon")]
    public float cannonRangeMin;
    public float cannonRangeMax;
    public float cannonCooldown;
    public float cannonTimer;
    public bool canShoot;
    [Space(8)]
    public eMine currentAmmo;
    public GameObject energyMine;
    public GameObject explosiveMine;
    public Transform bulletSpawnPoint;

    [Header("Grapple Hook")]
    public float grappleRange;
    public float grappleCooldown;
    public float grappleTimer;
    public bool canGrapple;

    [Header("Misc")]
    public LayerMask playerLayer;
    public GameObject playerRef;

    public void Update()
    {
        SetCanMelee();
        SetCanShoot();
        SetCanGrapple();
    }

    #region Melee

    public void SetCanMelee()
    {
        if (meleeTimer < meleeCooldown)
        {
            meleeTimer += Time.deltaTime;
            canMelee = false;
        }
        else
        {
            meleeTimer = meleeCooldown;
            canMelee = true;
        }
    }

    public bool CheckMeleeRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);

        if(distanceToPlayer < meleeRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ActivateBlade()
    {
        blade.SetActive(true);
    }

    public void DeactivateBlade()
    {
        blade.SetActive(false);
    }

    #endregion

    #region Cannon

    public void SetCanShoot()
    {
        if (cannonTimer < cannonCooldown)
        {
            cannonTimer += Time.deltaTime;
            canShoot = false;
        }
        else
        {
            cannonTimer = cannonCooldown;
            canShoot = true;
        }
    }

    public bool CheckCannonRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);

        if(distanceToPlayer < cannonRangeMax && distanceToPlayer > cannonRangeMin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Shoot()
    {        
        if(currentAmmo == eMine.energy)
        {
            GameObject mine = Instantiate(energyMine, bulletSpawnPoint);
            mine.GetComponent<Mine>().InitBullet(playerRef);
        }
        else if (currentAmmo == eMine.explosive)
        {
            GameObject mine = Instantiate(explosiveMine, bulletSpawnPoint);
            mine.GetComponent<Mine>().InitBullet(playerRef);
        }
    }

    #endregion

    #region Grapple

    public void SetCanGrapple()
    {
        if (grappleTimer < grappleCooldown)
        {
            grappleTimer += Time.deltaTime;
            canGrapple = false;
        }
        else
        {
            grappleTimer = meleeCooldown;
            canGrapple = true;
        }
    }


    #endregion
}
