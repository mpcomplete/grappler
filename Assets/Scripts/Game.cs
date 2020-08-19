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
    Ray downRay = new Ray(position + Vector3.up*.5f, direction);

    if (player.CharacterController.isGrounded && Physics.Raycast(downRay, out RaycastHit hit, 1f, 1<<ground.gameObject.layer)) {
      Vector3 normal = hit.normal.normalized;
      Vector3 tangent = new Vector3(0, -normal.z, normal.y);

      player.transform.forward = tangent;
      player.transform.up = normal;
      Vector3 accel = Vector3.Project(Physics.gravity, tangent);
      player.Velocity += accel*dt;
      player.Velocity = Vector3.Project(player.Velocity, tangent);
      player.CharacterController.Move(player.Velocity*dt);
      player.Velocity = player.CharacterController.velocity;
      Debug.Log($"Grounded");
      player.FrictionParticles.Play();
    } else {
      Debug.Log("Airborne");
      player.Velocity += Physics.gravity * dt;
      player.CharacterController.Move(player.Velocity*dt);
      player.FrictionParticles.Stop();
    }

    smartCamera.LookAt(player.transform);
  }
}