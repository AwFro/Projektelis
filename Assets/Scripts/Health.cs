﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public float MaxHealth = 100f;
    public float CurrentHealth;
    public float HealthLeft;
    public GameObject Bar;
    GameObject Player;
    Movement mscript;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        mscript = Player.GetComponent<Movement>();
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(HealthLeft);
       if (mscript.FDamage == true)
        {   if (mscript.FallDamage < 0) mscript.FallDamage = mscript.FallDamage * -1;
            CurrentHealth = CurrentHealth - mscript.FallDamage;
            HealthLeft = MaxHealth -(MaxHealth - CurrentHealth);
            HealthBarDepletion(HealthLeft);
            mscript.FDamage = false;
        }
       
    }

    public void HealthBarDepletion(float HealthLeft)
    {
        Bar.transform.localScale = new Vector3(Bar.transform.localScale.x - (mscript.FallDamage / 300 ), Bar.transform.localScale.y, Bar.transform.localScale.z);
        if(CurrentHealth <= 0)
        {
            GameObject.Find("HealthBar").GetComponent<playerDeath>().enabled = true;
            GameObject.Find("Player").GetComponent<Animator>().SetBool("Dead", true);
        }
    }
    public void Damage(float Reduced)
    {
        CurrentHealth = CurrentHealth - Reduced;
        Bar.transform.localScale = new Vector3(Bar.transform.localScale.x - (Reduced / 300), Bar.transform.localScale.y, Bar.transform.localScale.z);
        if (CurrentHealth <= 0)
        {
            GameObject.Find("HealthBar").GetComponent<playerDeath>().enabled = true;
            GameObject.Find("Player").GetComponent<Animator>().SetBool("Dead", true);
        }
    }
}

