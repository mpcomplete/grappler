using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
  public enum GroundState { Free, Grounded }

  public SphereCollider Collider;
  public float Mass = 1f;
  public Vector3 Velocity;
  public GroundState State = GroundState.Free;

  public static void Execute(Mover[] movers, Vector3 externalForces, float dt) {
    const int substepCount = 16;
    const float substepCountf = (float)substepCount;
    int count = movers.Length;
    float substepDt = dt / substepCountf;

    for (int i = 0; i < count; i++) {
      for (int j = 0; j < substepCount; j++) {
        Update(movers[i], externalForces, substepDt);
      }
    }
  }

  static RaycastHit[] hits = new RaycastHit[64];
  static void Update(Mover mover, Vector3 externalForces, float dt) {
    Vector3 currentPosition = mover.transform.position;
    Vector3 predictedVelocity = mover.Velocity + externalForces * dt;
    Vector3 predictedHeading = predictedVelocity.normalized;
    Vector3 predictedPositionDelta = predictedVelocity * dt;
    Vector3 predictedPosition = currentPosition + predictedPositionDelta;

    int collisionCount = Physics.SphereCastNonAlloc(currentPosition, mover.Collider.radius + .1f, predictedHeading, hits, predictedPositionDelta.magnitude);

    mover.Velocity = predictedVelocity;
    mover.transform.position = predictedPosition;
    for (int i = 0; i < collisionCount; i++) {
      if (hits[i].collider == mover.Collider)
        continue;

      Vector3 normal = hits[i].normal;
      Vector3 tangent = new Vector3(0, -normal.z, normal.y);
      Vector3 penetrationVector = hits[i].point - predictedPosition;
      Vector3 correction = normal * (mover.Collider.radius - penetrationVector.magnitude);

      Debug.Log(hits[i].point);
      Debug.DrawRay(hits[i].point, correction, Color.green);
      mover.transform.position += correction;
      mover.Velocity = Vector3.Project(mover.Velocity, tangent);
    }
  }
}