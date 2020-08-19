using UnityEngine;

public class Game : MonoBehaviour {
  Player player;
  SmartCamera smartCamera;


  void Start() {
    player = FindObjectOfType<Player>();
    smartCamera = FindObjectOfType<SmartCamera>();
  }

  void Update() {
    if (player.CharacterController.isGrounded) {
      smartCamera.LookAt(player.transform);
    }
  }

  void FixedUpdate() {
    float dt = Time.deltaTime;
    Vector3 motion = Physics.gravity;

    player.Velocity.x = 0;
    player.Velocity.z = 0;
    if (Input.GetKey(KeyCode.A)) {
      motion += -player.Speed * Vector3.forward;
    } else if (Input.GetKey(KeyCode.D)) {
      motion += player.Speed * Vector3.forward;
    }
    player.Velocity += motion * dt;
    player.CharacterController.Move(player.Velocity);
  }
}