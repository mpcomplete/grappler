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

    if (player.CharacterController.isGrounded && Physics.Raycast(downRay, out RaycastHit hit, 1f)) {
      Vector3 tangent = new Vector3(0, -hit.normal.z, hit.normal.y);

      player.transform.forward = direction;
      player.transform.up = hit.normal;
      player.Velocity = Vector3.Project(player.Velocity + dt * Physics.gravity, tangent);
      player.CharacterController.Move(player.Velocity);
      player.CharacterController.Move(-hit.normal * .1f);
      player.FrictionParticles.Play();
    } else {
      player.Velocity += Physics.gravity * dt;
      player.CharacterController.Move(player.Velocity);
      player.FrictionParticles.Stop();
    }

    smartCamera.LookAt(player.transform);
  }
}