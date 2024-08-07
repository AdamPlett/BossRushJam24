using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class BossHealthBar2 : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 200f;
    public float health = 200f;
    public float lerpSpeed = 0.0005f;

    [Header("References")]
    public Slider healthSlider;
    public Slider DmgOverTimeSlider;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = gm.bh2.maxHealth;
        health = gm.bh2.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = gm.bh2.currentHealth;
        //if health of entity differs from the slider than change the slider value to current health
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }
        //Slows decreases background dmg over time bar to health bar's current value
        if (healthSlider.value != DmgOverTimeSlider.value)
        {
            DmgOverTimeSlider.value = Mathf.Lerp(DmgOverTimeSlider.value, healthSlider.value, lerpSpeed);
        }
        //Test damage for health bar
        /*if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(10);
        }*/
    }
    //Code to test damage on health bars
    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }
}
