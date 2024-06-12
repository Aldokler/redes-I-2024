using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Animator animator;
    private bool attacking = false;
    private bool lookingLeft = false;
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private bool grounded;
    [SerializeField] private int knockback;

    [SerializeField] private GameObject hitBoxA;
    [SerializeField] private GameObject hitBoxB;
    [SerializeField] private GameObject hitBoxC;

    [SerializeField] private AudioSource sfxFire;
    [SerializeField] private AudioSource sfxWater;
    [SerializeField] private AudioSource sfxPlant;
    [SerializeField] private AudioSource sfxJump;

    public override void OnNetworkSpawn()
    {
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();

        if (OwnerClientId == 1)
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        manageAnimations();
        Move();
        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();
    }

    private void Move()
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        if (Input.GetAxis("Horizontal") > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

    }
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        grounded = false;
        sfxJump.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }

    public void pushback()
    {
        var direction = -transform.right;
        body.AddForce(direction * knockback);

    }

    private void manageAnimations()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > 0 && lookingLeft) { girar(); }
        else if (horizontal < 0 && !lookingLeft) { girar(); }
        animator.SetFloat("Moving", Mathf.Abs(horizontal));

        if (!attacking)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetBool("Fire", true);
                attacking = true;
                hitBoxA.SetActive(true);
                sfxFire.Play();
                StartCoroutine(fireAttackCoroutine());
            }
            if (Input.GetButtonDown("Fire2"))
            {
                animator.SetBool("Water", true);
                attacking = true;
                hitBoxB.SetActive(true);
                sfxWater.Play();
                StartCoroutine(waterAttackCoroutine());
            }
            if (Input.GetButtonDown("Fire3"))
            {
                animator.SetBool("Plant", true);
                attacking = true;
                hitBoxC.SetActive(true);
                sfxPlant.Play();
                StartCoroutine(plantAttackCoroutine());
            }
        }

    }

    private void girar()
    {
        lookingLeft = !lookingLeft;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    IEnumerator fireAttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Fire", false);
        attacking = false;
        hitBoxA.SetActive(false);
    }

    IEnumerator waterAttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Water", false);
        attacking = false;
        hitBoxB.SetActive(false);
    }

    IEnumerator plantAttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Plant", false);
        attacking = false;
        hitBoxC.SetActive(false);
    }
}
