using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Explosion : MonoBehaviour
{
    public float damage;
    public eMine mineType;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Hit");

        if (other.gameObject.tag.Equals("Destructible") && mineType == eMine.explosive)
        {
            Debug.Log("destructible object hit" );
            BreakObject destroy = other.gameObject.GetComponent<BreakObject>();

            if (other.gameObject.GetComponent<BreakObject>()!=null)
            {
                destroy.breakObject();
            }
        }
        if (other == gm.playerCollider)
        {
            gm.ph.DamagePlayer(damage);
        }

    }

}
