using UnityEngine;

public class Game : MonoBehaviour {
  Player player;

  void Start() {
    player = FindObjectOfType<Player>();
  }

  void FixedUpdate() {
    if (Input.GetKey(KeyCode.A)) {
      player.CharacterController.Move(-player.Speed * Time.deltaTime * Vector3.forward);
    } else if (Input.GetKey(KeyCode.D)) {
      player.CharacterController.Move(player.Speed * Time.deltaTime * Vector3.forward);
    }
  }
}