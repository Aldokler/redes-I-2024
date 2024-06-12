using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
    public NetworkVariable<int> health = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Text healthText0;


    public GameObject game_over_screen;
    public GameObject you_win_screen;

    public override void OnNetworkSpawn()
    {

        if (OwnerClientId == 1)
        {
            healthText0.GetComponent<RectTransform>().position.Set(411, -202,0);
        }
        else
        {
            healthText0.GetComponent<RectTransform>().position.Set(-280, -202, 0);
        }
    }

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
                    Debug.Log("Faiya");

                }
                else if (Input.GetKey(atkB))
                {
                    col = transform.Find("AtkHitboxB").gameObject.GetComponent<CircleCollider2D>();
                    AttackingB = true;
                    Debug.Log("Wata");

                }
                else if (Input.GetKey(atkC))
                {
                    col = transform.Find("AtkHitboxC").gameObject.GetComponent<CircleCollider2D>();
                    AttackingC = true;
                    Debug.Log("Leaf");
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
            Debug.Log(health.Value);
            health.Value -= 1;
            healthText0.text = health.Value.ToString();
            if (health.Value == 0)
            {
                NetworkObject.Despawn();
                you_lose();
            }
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
            Debug.Log(health.Value);
            health.Value -= 1;
            healthText0.text = health.Value.ToString();
            if (health.Value == 0)
            {
                NetworkObject.Despawn();
                you_lose();
            }
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
            Debug.Log(health.Value);
            health.Value -= 1;
            healthText0.text = health.Value.ToString();
            if (health.Value == 0)
            {
                NetworkObject.Despawn();
                you_lose();
            }
            return 1;
        }
    }


    public void takeCounterDamage() {
        health.Value -= 1;
        Debug.Log("took counter dmg");
    }

    public int getHealth() {
        return health.Value;
    }

    public void you_lose()
    {
        game_over_screen.SetActive(true);
    }

    public void you_win()
    {
        you_win_screen.SetActive(true);
    }


}
