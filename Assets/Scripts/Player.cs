using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
  public CharacterController CharacterController;
  public Vector3 Velocity;
  public float Speed = 1f;
  public ParticleSystem FrictionParticles;
  public Whip Whip = null;

  public bool IsWhipStuck { get => WhipRadius > 0f; }
  public float WhipRadius = 0f;
  public Vector3 WhipStuckTo { get => whip.transform.position; }
  Whip whip = null;
  Vector3 whipTarget = Vector3.zero;

  public void UseWhip(Vector3 targetPos) {
    if (!whip) {
      whip = Instantiate(Whip);
      whip.transform.position = transform.position + Vector3.up;
      whipTarget = targetPos;
      StartCoroutine(WhipRoutine());
    } else {
      WhipRadius = 0f;
      Destroy(whip.gameObject);
      whip = null;
    }
  }

  IEnumerator WhipRoutine() {
    float t = 0f;
    while (t < .5f) {
      if (whip.StuckToObject) {
        break;

      }
      whip.transform.position = Vector3.Lerp(whip.transform.position, whipTarget, 1 - Mathf.Pow(1e-5f, Time.deltaTime));
      t += Time.deltaTime;
      yield return null;
    }
    if (whip.StuckToObject) {
      t = 0f;
      float distance = (whip.transform.position - transform.position).magnitude;
      WhipRadius = distance;
      while (t < .2f) {
        WhipRadius = Mathf.Lerp(WhipRadius, distance*.5f, 1 - Mathf.Pow(1e-5f, Time.deltaTime));
        t += Time.deltaTime;
        yield return null;
      }
      yield break;
    }
    WhipRadius = 0f;
    Destroy(whip.gameObject);
    whip = null;
  }
}