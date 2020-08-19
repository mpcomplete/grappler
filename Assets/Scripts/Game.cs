using UnityEngine;

public class Game : MonoBehaviour {
  Player player;
  SmartCamera smartCamera;

  void Start() {
    player = FindObjectOfType<Player>();
    smartCamera = FindObjectOfType<SmartCamera>();
  }

  void Update() {
  }

  void FixedUpdate() {
    float dt = Time.fixedDeltaTime;
    Vector3 direction = Physics.gravity.normalized;
    Vector3 position = player.transform.position;
    Ray downRay = new Ray(position, direction);

    if (player.CharacterController.isGrounded && Physics.Raycast(downRay, out RaycastHit hit, 10f)) {
      Vector3 tangent = new Vector3(0, -hit.normal.z, hit.normal.y);

      Debug.Log("Grounded");
      Debug.DrawRay(position, direction, Color.blue);
      Debug.DrawRay(position, hit.normal, Color.green);
      Debug.DrawRay(position, tangent, Color.red);
      player.transform.forward = direction;
      player.transform.up = hit.normal;
      player.Velocity = Vector3.Project(player.Velocity + dt * Physics.gravity, tangent);
    } else {
      Debug.Log("Air");
      player.Velocity += Physics.gravity * dt;
    }

    player.CharacterController.Move(player.Velocity);
    smartCamera.LookAt(player.transform);
  }
}