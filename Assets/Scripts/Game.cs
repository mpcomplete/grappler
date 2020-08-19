using UnityEngine;

public class Game : MonoBehaviour {
  Player player;
  Camera camera;

  public float CameraDistance = 20f;

  void Start() {
    player = FindObjectOfType<Player>();
    camera = Camera.main;
  }

  void Update() {
    // Update the camera
    {
      Vector3 position = player.transform.position;

      position.x = CameraDistance;
      camera.transform.position = position;
      camera.transform.LookAt(player.transform);
    }
  }

  void FixedUpdate() {
    if (Input.GetKey(KeyCode.A)) {
      player.CharacterController.Move(-player.Speed * Time.deltaTime * Vector3.forward);
    } else if (Input.GetKey(KeyCode.D)) {
      player.CharacterController.Move(player.Speed * Time.deltaTime * Vector3.forward);
    }
  }
}