using UnityEngine;

public class Game : MonoBehaviour {
  Player player;
  SmartCamera smartCamera;
  Ground ground;

  void Start() {
    player = FindObjectOfType<Player>();
    smartCamera = FindObjectOfType<SmartCamera>();
    ground = FindObjectOfType<Ground>();
  }

  void Update() {
  }
  
  void FixedUpdate() {
    float dt = Time.fixedDeltaTime;
    Vector3 direction = Physics.gravity.normalized;
    Vector3 position = player.transform.position;

    Mover.Execute(FindObjectsOfType<Mover>(), Physics.gravity, dt);
    smartCamera.LookAt(player.transform);
  }
}