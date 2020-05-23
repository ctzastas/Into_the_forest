using UnityEngine;

namespace Jastas {

    public class GameManager : MonoBehaviour {
        
        static GameManager instance;

        void Awake() {
            instance = this;
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        }
    
        // GameManager Instance
        public static GameManager Instance {
            get {
                if (instance == null) {
                    Debug.LogError("Game Manager is null");
                }
                return instance;
            }
        }
    
        // An instance of player
        public PlayerManager Player { get; private set; }
    }
}