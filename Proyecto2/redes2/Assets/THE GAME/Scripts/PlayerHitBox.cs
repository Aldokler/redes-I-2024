using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private int result;
    private PlayerAttack target;
    [SerializeField] private int attackType;


    private void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if (transform.CompareTag("Fire") && col.gameObject.CompareTag("Plant"))
        {
            col.gameObject.GetComponentInParent<PlayerMovement>().pushback();
            col.gameObject.GetComponentInParent<PlayerAttack>().health -= 1;
        }
        else if (transform.CompareTag("Fire") && col.gameObject.CompareTag("Water"))
        {
            gameObject.GetComponentInParent<PlayerMovement>().pushback();
            gameObject.GetComponentInParent<PlayerAttack>().health += 1;
        }
        else if (transform.CompareTag("Fire") && col.gameObject.CompareTag("Fire"))
        {
            col.gameObject.GetComponentInParent<PlayerMovement>().pushback();
            gameObject.GetComponentInParent<PlayerMovement>().pushback();
        }

        else if (transform.CompareTag("Water") && col.gameObject.CompareTag("Fire"))
        {
            col.gameObject.GetComponentInParent<PlayerMovement>().pushback();
            col.gameObject.GetComponentInParent<PlayerAttack>().health -= 1;
        }
        else if (transform.CompareTag("Water") && col.gameObject.CompareTag("Plant"))
        {
            gameObject.GetComponentInParent<PlayerMovement>().pushback();
            gameObject.GetComponentInParent<PlayerAttack>().health += 1;
        }
        else if (transform.CompareTag("Water") && col.gameObject.CompareTag("Water"))
        {
            col.gameObject.GetComponentInParent<PlayerMovement>().pushback();
            gameObject.GetComponentInParent<PlayerMovement>().pushback();
        }

        else if (transform.CompareTag("Plant") && col.gameObject.CompareTag("Water"))
        {
            col.gameObject.GetComponentInParent<PlayerMovement>().pushback();
            col.gameObject.GetComponentInParent<PlayerAttack>().health -= 1;
        }
        else if (transform.CompareTag("Plant") && col.gameObject.CompareTag("Fire"))
        {
            gameObject.GetComponentInParent<PlayerMovement>().pushback();
            gameObject.GetComponentInParent<PlayerAttack>().health += 1;
        }
        else if (transform.CompareTag("Plant") && col.gameObject.CompareTag("Plant"))
        {
            col.gameObject.GetComponentInParent<PlayerMovement>().pushback();
            gameObject.GetComponentInParent<PlayerMovement>().pushback();
        }
        */

        if ((transform.parent.CompareTag("Player") && col.gameObject.layer == 6 && col.gameObject.CompareTag("Enemy")) || (transform.parent.CompareTag("Enemy") && col.gameObject.layer == 7 && col.gameObject.CompareTag("Player")))
        {
            target = col.GetComponent<PlayerAttack>();

            switch (attackType)
            {
                case 1:
                    result = target.takeDamageA();
                    break;
                case 2:
                    result = target.takeDamageB();
                    break;
                case 3:
                    result = target.takeDamageC();
                    break;
            }

            if (result == 0)
            {
                gameObject.GetComponentInParent<PlayerMovement>().pushback();
            }
            if (result == -1)
            {
                gameObject.GetComponentInParent<PlayerAttack>().takeCounterDamage();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        switch (attackType)
        {
            case 1:
                Gizmos.color = Color.red;
                break;
            case 2:
                Gizmos.color = Color.blue;
                break;
            case 3:
                Gizmos.color = Color.green;
                break;
        }

        Gizmos.DrawWireSphere(this.transform.position, this.transform.GetComponent<CircleCollider2D>().radius);
    }
}
