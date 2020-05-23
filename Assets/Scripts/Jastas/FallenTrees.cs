using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jastas {

    public class FallenTrees : MonoBehaviour {

        Rigidbody2D fallenObject;                 // Change RigidBody values
        BoxCollider2D collider;                   // Change Collider values
        [SerializeField] float timer = 2f;        // Set waiting time 
        [SerializeField] AudioSource branchAudio;
        float speed;
        bool playOnce;
        int soundCount = 1;

        // Start is called before the first frame update
        void Start() {
            fallenObject = GetComponent<Rigidbody2D>();
            fallenObject.bodyType = RigidbodyType2D.Kinematic;
            collider = GetComponent<BoxCollider2D>();
            branchAudio = GetComponent<AudioSource>();
            branchAudio.Stop();
            playOnce = false;
        }

        void FixedUpdate() {
            speed = fallenObject.velocity.magnitude;
        }

        // if player walks over gameObject start Coroutine
        void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                playOnce = true;
                StartCoroutine(WaitSeconds(timer));
            }

            if (other.gameObject.CompareTag("Player") && speed > 2f) {
                GameManager.Instance.Player.Damage();
            }
        }

        // when player triggers the gameObject change its physics values
        void OnTriggerEnter2D(Collider2D other) {

            if (other.gameObject.CompareTag("Player")) {
                fallenObject.bodyType = RigidbodyType2D.Dynamic;
                collider.isTrigger = false;
                collider.offset = new Vector2(0, 0);
                collider.size = new Vector2(24, 1);
                fallenObject.mass = 1000;
                playOnce = true;
                if (playOnce && soundCount == 1) {
                    PlaySound(playOnce);
                    soundCount -= 1;
                }
            }
        }

        // Wait 2 seconds then make RigidBody dynamic
        IEnumerator WaitSeconds(float timer) {
            yield return new WaitForSeconds(timer);
            fallenObject.bodyType = RigidbodyType2D.Dynamic;
            if (playOnce && soundCount == 1) {
                PlaySound(playOnce);
                soundCount -= 1;
            }
        }
        
        bool PlaySound(bool playOnce) {
            if (playOnce) {
                branchAudio.Play();
                playOnce = false;
            }
            else {
                branchAudio.Stop();
            }
            return playOnce;
        }
    }
}