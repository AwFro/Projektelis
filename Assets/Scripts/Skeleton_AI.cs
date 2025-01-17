﻿using UnityEngine;
using System.Collections;
using System.Threading;


public class Skeleton_AI : MonoBehaviour
{
    private Rigidbody2D larry;
    private Animator anim;
    private bool allowTimer, allowChase = true, walk, swing;
    private float detTimer = 0.2f, AfterKnockTimer=0.35f;
    private int health = 100;
    public float speed,  jumpheight, KnockDuration, KnockPower, maxSpeed,  DoSmthTimer, timer, dirTimer, followTimer=5f, waitTimer=0.1f, waitTimerAtm=0.1f, playerKnockPow;
    public string mode;
    public bool canMove, direction;
    public WallDetection wScript, platform;
    public GroundDetection gScript;
    public PlayerDetection dScript, attackScript;
    public Transform player;
    // Use this for initialization
    void Start()
    {
        larry = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        timer = DoSmthTimer;
        dirTimer = DoSmthTimer * 2;
        mode = "idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (hitTimerAtm > 0) hitTimerAtm -= Time.deltaTime;

        if (!allowChase)
        {
            AfterKnockTimer -= Time.deltaTime;
            if (AfterKnockTimer <= 0)
            {
                AfterKnockTimer = 0.35f;
                allowChase = true;
            }}

        if (larry.velocity.x > maxSpeed)
        {
            larry.velocity = new Vector2(maxSpeed, larry.velocity.y);
        }

        else if (larry.velocity.x < -maxSpeed)
        {
            larry.velocity = new Vector2(-maxSpeed, larry.velocity.y);
        }

        if (mode == "idle") idle();
        else if (mode == "aggressive") goAfter();
        else if (mode == "fight") attack();

        //ANIMATOR
        anim.SetBool("Attacking", swing);
        anim.SetBool("Walking", walk);
        if (larry.velocity.x > 0 || larry.velocity.x < 0)
        {
            walk = true;
        }
        else walk = false; //end animator
        
    }

   
    

    void idle()
    {
        if (timer <= 0)
        {
            canMove = !canMove;
            timer = DoSmthTimer;
        }
        if (dirTimer <= 0)
        {
            direction = !direction;
            dirTimer = DoSmthTimer; //*2 kad eitu maziau
        }
        if (detTimer <= 0)
        {
            if (!platform.walling || wScript.walling) direction = !direction;
            detTimer = 0.2f;
        }


        if (canMove)
        {
            if (direction)
            {
                gameObject.transform.localScale = new Vector3(2, 2, 2);
                larry.AddForce(transform.right * (-speed));
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-2, 2, 2);
                larry.AddForce(transform.right * speed);
            }

        }

        if (dScript.iSeeIt) mode = "aggressive";
        speed = 450f;
        timer -= Time.deltaTime;
        dirTimer -= Time.deltaTime;
        detTimer -= Time.deltaTime;
    }


    void goAfter()
    {
        speed = 900f;       
        if(gScript.ground && wScript.walling && allowChase) { larry.velocity = (new Vector2(larry.velocity.x, jumpheight));     
        }
        if (player.position.x > transform.position.x)
        {
            if (player.position.x - transform.position.x > 1f && allowChase==true)
            {
                gameObject.transform.localScale = new Vector3(-2, 2, 2);
                larry.AddForce(transform.right * speed);
        }}
        else
        {
            if ((transform.position.x  - player.position.x > 1f) && allowChase == true)
            {
                gameObject.transform.localScale = new Vector3(2 , 2, 2);
                larry.AddForce(transform.right * (-speed));
            }}
        if (!dScript.iSeeIt)  allowTimer = true;
        if (allowTimer)
        {
            followTimer -= Time.deltaTime;
            if (followTimer <= 0)
            {
                followTimer = 5f;
                mode = "idle";
                allowTimer = false;
            }
        }
        else if (attackScript.iSeeIt)
        {
            mode = "fight";
            swing = true;
        }
        else swing = false;
    }

    private float hitTimer=1.5f, hitTimerAtm=0.5f;
    void attack()
    {
        if(!attackScript.iSeeIt)
        {
            mode = "aggressive";
        }
        if(hitTimerAtm<=0)
        {
            int puse;
            if (transform.localScale.x == 1) puse = 1;
            else puse = -1;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(puse * 2, 1)*playerKnockPow;
            GameObject.Find("Main Camera").SendMessageUpwards("Damage", 30);
            hitTimerAtm = hitTimer;
        }
    }

    void Damage(int hitpoints)
    {
        health -= hitpoints;
        Debug.Log(health);
        if (health <= 0) Destroy(gameObject);
        else StartCoroutine(KnockBack()); 
    }


    IEnumerator KnockBack()
    {
        yield return new WaitForSeconds(0.1f);
        float timeris=0, z;
        if (player.position.x >= gameObject.transform.position.x) z = -1;
        else z = 1; //puse i kuria skris
        allowChase = false;
        larry.velocity = new Vector2( z*2, 1) * KnockPower;
    }
}
