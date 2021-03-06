﻿using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour {
  Player player;
  SmartCamera smartCamera;
  Camera mainCamera;
  Ground ground;

  void Start() {
    player = FindObjectOfType<Player>();
    smartCamera = FindObjectOfType<SmartCamera>();
    mainCamera = smartCamera.GetComponent<Camera>();
    ground = FindObjectOfType<Ground>();
    Physics.IgnoreLayerCollision(player.gameObject.layer, player.gameObject.layer, true);
    Physics.IgnoreLayerCollision(player.gameObject.layer, 11, true);
  }

  void Update() {
    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q)) {
      Vector3 clickWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, smartCamera.Distance));
      player.UseWhip(clickWorldPos);
    }
  }

  void FixedUpdate() {
    float dt = Time.fixedDeltaTime;
    Vector3 direction = Physics.gravity.normalized;
    Vector3 position = player.transform.position;
    Ray downRay = new Ray(position+player.transform.up*.25f, -player.transform.up);

    if (player.IsWhipStuck) {
      Vector3 inwards = player.WhipStuckTo - player.transform.position;
      Vector3 tangent = new Vector3(0, -inwards.z, inwards.y);
      Vector3 accel = Vector3.Project(Physics.gravity, tangent);
      player.Velocity += accel*dt;
      player.Velocity = Vector3.Project(player.Velocity, tangent);
      player.CharacterController.Move(player.Velocity*dt);
      Vector3 inwardsNew = player.WhipStuckTo - player.transform.position;
      player.transform.position = player.WhipStuckTo - player.WhipRadius*inwardsNew.normalized;
    } else if (player.CharacterController.isGrounded && Physics.Raycast(downRay, out RaycastHit hit, 1f, 1<<ground.gameObject.layer)) {
      Vector3 normal = hit.normal.normalized;
      Vector3 tangent = new Vector3(0, -normal.z, normal.y);

      player.transform.forward = tangent;
      player.transform.up = normal;
      Vector3 accel = Vector3.Project(Physics.gravity, tangent);
      player.Velocity += accel*dt;
      player.Velocity = Vector3.Project(player.Velocity, tangent);
      player.CharacterController.Move(player.Velocity*dt);
      player.CharacterController.Move(normal*-.1f);
      //Debug.Log($"Grounded");
      player.FrictionParticles.Play();
    } else {
      Debug.Log($"Airborne {player.CharacterController.isGrounded}");
      player.Velocity += Physics.gravity * dt;
      player.CharacterController.Move(player.Velocity*dt);
      player.FrictionParticles.Stop();
    }

    smartCamera.LookAt(player.transform);
  }
}