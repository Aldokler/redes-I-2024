using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerHitBoxB : MonoBehaviour
{
    private int result;
    private PlayerAttack target;
    // Start is called before the first frame update

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((transform.parent.CompareTag("Player") && col.gameObject.layer == 6 && col.gameObject.CompareTag("Enemy")) || (transform.parent.CompareTag("Enemy") && col.gameObject.layer == 7 && col.gameObject.CompareTag("Player")))
        {
            target = col.GetComponent<PlayerAttack>();

            result = target.takeDamageB();

            if (result == 0)
            {
                this.GetComponentInParent<PlayerMovement>().pushback();
            }
            if (result == -1)
            {
                this.GetComponentInParent<PlayerAttack>().takeCounterDamage();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, this.transform.GetComponent<CircleCollider2D>().radius);
    }
}
