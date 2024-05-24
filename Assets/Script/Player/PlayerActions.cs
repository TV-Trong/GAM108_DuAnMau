﻿using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        public Transform bowPosition;
        public GameObject arrowObj;
        private Speaker speaker;
        private Animator animator;
        private bool isFiring = false;
        private PlayerMovement playerMovement;

        private float dashDuration = 0.2f;

        private void Start()
        {
            speaker = FindObjectOfType<Speaker>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }


        public void OnFire(InputAction.CallbackContext context)
        {
            if (!isFiring && !IsPlayingFireAnimation())
            {
                speaker.PlayAudioOneShot("Fire");
                isFiring = true;
                Quaternion arrowRotation = playerMovement.isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
                Instantiate(arrowObj, bowPosition.position, arrowRotation /*Quaternion.identity*/);
                animator.Play("Fire");
                StartCoroutine(WaitForAnimation());
            }
        }

        private IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(0.1f);
            while (IsPlayingFireAnimation())
            {
                yield return null;
            }
            isFiring = false;
        }

        private bool IsPlayingFireAnimation()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Fire");
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.performed && playerMovement.IsGrounded() && !playerMovement.isRolling)
            {
                
                playerMovement.isRolling = false;
                StartCoroutine(DashCoroutine());
            }
        }

        private IEnumerator DashCoroutine()
        {
            playerMovement.isRolling = true;
            animator.Play("Roll");
            speaker.PlayAudioOneShot("Roll");
            float elapsedTime = 0f;
            Vector3 moveDirection = transform.right * (playerMovement.isFacingRight ? 1 : -1) * playerMovement.dashPower;

            while (elapsedTime < dashDuration)
            {
                transform.Translate(moveDirection * Time.deltaTime, Space.World);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerMovement.isRolling = false;
        }
    }
}
    