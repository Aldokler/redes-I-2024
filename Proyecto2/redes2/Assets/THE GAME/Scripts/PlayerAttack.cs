using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{

    public float timeBtwAttack;
    public float startTimeBtwAttack;

    public float attackRange;
    public int damage;

   
    public bool AttackingA = false;
    public bool AttackingB = false;
    public bool AttackingC = false;

    public KeyCode keys, atkA, atkB, atkC;

    CircleCollider2D col = null;
    public float duration;
    private float durationT;
    public int health;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (AttackingA == true || AttackingB == true || AttackingC == true)
        {
            if (col.enabled == true)
            {
                if (durationT > 0)
                {
                    durationT -= Time.deltaTime; ;
                }
                else
                {
                    col.enabled = false;

                    AttackingA = false;
                    AttackingB = false;
                    AttackingC = false;
                    durationT = duration;
                }
            }
        }
        

        if (timeBtwAttack <= 0){

            if (Input.GetKey(atkA) || Input.GetKey(atkB) || Input.GetKey(atkC)) {

                if (Input.GetKey(atkA)) {
                    col = transform.Find("AtkHitboxA").gameObject.GetComponent<CircleCollider2D>();
                    AttackingA = true;


                }else if (Input.GetKey(atkB))
                {
                    col = transform.Find("AtkHitboxB").gameObject.GetComponent<CircleCollider2D>();
                    AttackingB = true;


                }else if (Input.GetKey(atkC))
                {
                    col = transform.Find("AtkHitboxC").gameObject.GetComponent<CircleCollider2D>();
                    AttackingC = true;
                }

                if (col.enabled == false) { 
                    col.enabled = true; 
                }

                timeBtwAttack = startTimeBtwAttack;
            }


        } else{
            timeBtwAttack -= Time.deltaTime;
        }

    }

    public int takeDamageA()
    {
        if (AttackingA)
        {

            //Debug.Log("Push");
            return 0;

        }
        else if (AttackingB)
        {
            Debug.Log("Counter");
            return -1;
        }
        else {

            Debug.Log("Took dmg A");
            health -= 1;
            return 1;
        }
    }

    public int takeDamageB()
    {
        if (AttackingB)
        {

            //Debug.Log("Push");
            return 0;

        }
        else if (AttackingC)
        {
            Debug.Log("Counter");
            return -1;
        }
        else
        {

            Debug.Log("Took dmg B");
            health -= 1;
            return 1;
        }
    }

    public int takeDamageC()
    {
        if (AttackingC)
        {

            //Debug.Log("Push");
            return 0;

        }
        else if (AttackingA)
        {
            Debug.Log("Counter");
            return -1;
        }
        else
        {

            Debug.Log("Took dmg C");
            health -= 1;
            return 1;
        }
    }


    public void takeCounterDamage() {
        health -= 1;
        Debug.Log("took counter dmg");
    }

    public int getHealth() {
        return health;
    }


}
