﻿using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Slime : EnemyMain
{
    [SerializeField] private GameObject boxPOS;
    [SerializeField] private GameObject playerPOS;

    private Rigidbody2D rb;

    private bool isMovingRight = true;
    void Start()
    {
        health = 2;
        speed = 5;
        damage = 1;
        isDetectedPlayer = false;

        playerPOS = GameObject.Find("Player");

        if (playerPOS == null)
        {
            Debug.LogError("Player object not found");
            return;
        }

        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(MoveCoroutine());
    }
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (isDetectedPlayer)
            {
                // Change direction based on the player's position
                isMovingRight = playerPOS.transform.position.x > transform.position.x;
            }

            // Move normally (patrolling behavior)
            float direction = isMovingRight ? 1f : -1f;
            rb.AddForce(new Vector2(direction * speed, 10f), ForceMode2D.Impulse);

            yield return new WaitForSeconds(1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(10);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BoxEnemy"))
        {
            if (transform.position.x < boxPOS.transform.position.x)
            {
                isMovingRight = true;
            }
            else isMovingRight = false;
        }
    }
}
