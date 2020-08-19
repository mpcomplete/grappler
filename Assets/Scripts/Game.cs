using UnityEngine;

public class Game : MonoBehaviour {
  Player player;
  SmartCamera smartCamera;


  void Start() {
    player = FindObjectOfType<Player>();
    smartCamera = FindObjectOfType<SmartCamera>();
  }

  void Update() {
    smartCamera.LookAt(player.transform);
  }

  void FixedUpdate() {
    if (Input.GetKey(KeyCode.A)) {
      player.CharacterController.Move(-player.Speed * Time.deltaTime * Vector3.forward);
    } else if (Input.GetKey(KeyCode.D)) {
      player.CharacterController.Move(player.Speed * Time.deltaTime * Vector3.forward);
    }
  }
}