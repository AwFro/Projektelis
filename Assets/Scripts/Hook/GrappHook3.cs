﻿using UnityEngine;
using System.Collections;

public partial class GrappHook {

    void unHook()
    {
        {
            if (hook.enabled && (Input.GetKey(KeyCode.Space)))
            {
                player.GetComponent<Rigidbody2D>().velocity = (new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, jumpHeight));
                mScript.canDoubleJ = false;
            }
            if (hinge.enabled)
            {
                playerPhysics.mass = 1;
                playerPhysics.constraints = RigidbodyConstraints2D.FreezeRotation;
                player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, 0);
                mScript.enabled = true;
                perjungimoTimerAtm = perjungimoTimer;
                playerPhysics.drag = 0f;
            }
            spring.enabled = false; hook.enabled = false; hinge.enabled = false; line.enabled = false;  isHooked = false; pasiHookino = false; //viska atjungia
            springTimerAtm = springTimer;
            mScript.graplinghook = false;
            GameObject.Find("hookLook").GetComponent<SpriteRenderer>().enabled = false;
            linijaOn = false;
            playerPhysics.gravityScale = 50;
        }
    }

    void checkIfTouches()
    { //jeigu nieko nepasiekia (atstumas tarp sovinio ir zaidejo)
        float distance = Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y));
        if (distance > 100 || distance < -100)
        {
            linijaOn = false;
            line.enabled = false;
            hook.enabled = false;
            isShot = false;
            GameObject.Find("hookLook").GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject.GetComponentInChildren<Rigidbody2D>());
        }
    }

    void mouseClick()
    {
        {   // suranda peles koord., atema is ju zaidejo vieta ir taip suranda sovimo trajektorija.
            linijaOn = true;
            GameObject.Find("hookLook").GetComponent<SpriteRenderer>().enabled = true;
            try { gameObject.AddComponent<Rigidbody2D>(); } catch { };
            hook.enabled = false; isHooked = false; isShot = true;
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            Transform tr = location.transform;
            gameObject.transform.position = tr.position;
            target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Vector2 trajectory = target - new Vector2(location.transform.position.x, location.transform.position.y);
            trajectory.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg + 270);
            transform.rotation = rotation; //transform.rotation = rotation;
            rb2d.velocity = trajectory * speed;            
            playerPhysics.gravityScale = 50;
            timer = delay;          

        }
    }

    void aukstyn(bool upDown)
    {
        springOff = true;
        HookoPerjunginejimas(1); perjungimoTimerAtm = perjungimoTimer;
        if (upDown)
        {
            hook.enabled = true;
            hook.distance = Vector2.Distance(gameObject.transform.position, location.transform.position);
            if (hook.distance > 5) hook.distance = hook.distance - 1f;
            pasikeiteDistance();
        }
        else
        {                
            if (hook.distance < 100 && (player.transform.position.y < gameObject.transform.position.y)) hook.distance = hook.distance + 1f;
           pasikeiteDistance();
       }
    }
}
