using UnityEngine;

namespace Jastas {
    
    public class Rocks : MonoBehaviour {

        [SerializeField] Rigidbody2D[] rocks;
        [SerializeField] CircleCollider2D[] collider;
        BoxCollider2D player;

        void Start() {

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();

            for (int i = 0; i < rocks.Length; i++) {
                rocks[i] = GameObject.FindGameObjectsWithTag("Rocks")[i].GetComponent<Rigidbody2D>();
                rocks[i].bodyType = RigidbodyType2D.Kinematic;
            }
        }

        void Update() {
            RocksDamage();
        }

        // Activates Rigidbodies from kinematic to dynamic
        void OnTriggerEnter2D(Collider2D other) {
            for (int i = 0; i < rocks.Length; i++) {
                if (other.gameObject.CompareTag("Player")) {
                    rocks[i].bodyType = RigidbodyType2D.Dynamic;
                    rocks[i].mass = 1000;
                }
            }
        }

        // When any object collides with player collider
        // Set damage to the player 
        void RocksDamage() {
            for (int i = 0; i < rocks.Length; i++) {
                foreach (CircleCollider2D rock in collider) {
                    if (rock.IsTouching(player) && rocks[i].velocity.magnitude > 2f) {
                        GameManager.Instance.Player.Damage();
                    }
                }
            }
        }
    }
}