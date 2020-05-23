using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSound : MonoBehaviour {

    [SerializeField] AudioSource audioRocks;
    [SerializeField] Rigidbody2D rockRb;
    float speed;

    // Start is called before the first frame update
    void Start() {
        audioRocks = GetComponent<AudioSource>();
        rockRb = GetComponent<Rigidbody2D>();
        audioRocks.Stop();
    }

    void FixedUpdate() {
        speed = rockRb.velocity.magnitude;
    }

    void OnCollisionStay2D(Collision2D other) {
        if (audioRocks.isPlaying == false && speed >= 0.1f && other.gameObject.CompareTag("Ground")) {
            audioRocks.Play();

        } else if (audioRocks.isPlaying == true && speed < 0.06f && other.gameObject.CompareTag("Ground")) {
            audioRocks.Pause();
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (audioRocks.isPlaying == true && other.gameObject.CompareTag("Ground")) {
            audioRocks.Pause();
        }
    }
}