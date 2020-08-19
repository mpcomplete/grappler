using UnityEngine;

public class Player : MonoBehaviour {
  public CharacterController CharacterController;
  public Vector3 Velocity;
  public float Speed = 1f;
  public ParticleSystem FrictionParticles;
}