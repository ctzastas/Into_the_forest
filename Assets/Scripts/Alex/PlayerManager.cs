using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] bool isDead;

    public int Health { get; set; }

    // Start is called before the first frame update
    void Start() {
        isDead = false;
        Health = 1;

    }

    // Global player damage system
    public void Damage() {
        Health--;
        if (Health < 1) {
            isDead = true;
            Debug.Log("Player is dead");
        }
    }
}