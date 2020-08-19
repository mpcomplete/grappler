using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
  public CharacterController CharacterController;
  public Vector3 Velocity;
  public float Speed = 1f;
  public ParticleSystem FrictionParticles;

  GameObject whip = null;
  Vector3 whipTarget = Vector3.zero;

  public void UseWhip(Vector3 targetPos) {
    Debug.Log($"whipping: {whip}");
    if (whip == null) {
      whip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      whip.layer = gameObject.layer;
      whip.transform.position = transform.position + Vector3.up;
      whipTarget = targetPos;
      StartCoroutine(WhipRoutine());
    }
  }

  IEnumerator WhipRoutine() {
    float t = 0f;
    while (t < .5f) {
      whip.transform.position = Vector3.Lerp(whip.transform.position, whipTarget, 1 - Mathf.Pow(1e-5f, Time.deltaTime));
      t += Time.deltaTime;
      yield return null;
    }
    yield return new WaitForSeconds(.2f);
    Destroy(whip);
    whip = null;
  }
}