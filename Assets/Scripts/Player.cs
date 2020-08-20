﻿using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
  public CharacterController CharacterController;
  public Vector3 Velocity;
  public float Speed = 1f;
  public ParticleSystem FrictionParticles;
  public Whip Whip = null;

  public bool IsWhipStuck = false;
  Whip whip = null;
  Vector3 whipTarget = Vector3.zero;

  public void UseWhip(Vector3 targetPos) {
    if (!whip) {
      whip = Instantiate(Whip);
      whip.transform.position = transform.position + Vector3.up;
      whipTarget = targetPos;
      StartCoroutine(WhipRoutine());
    }
  }

  IEnumerator WhipRoutine() {
    float t = 0f;
    while (t < .5f) {
      if (!whip.StuckToObject) {
        whip.transform.position = Vector3.Lerp(whip.transform.position, whipTarget, 1 - Mathf.Pow(1e-5f, Time.deltaTime));
      }
      t += Time.deltaTime;
      yield return null;
    }

    if (whip.StuckToObject) {
      IsWhipStuck = true;
    }
    t = 0f;
    while (t < .5f) {
      if (whip.StuckToObject) {
        transform.position = Vector3.Lerp(transform.position, whip.transform.position, 1 - Mathf.Pow(1e-5f, Time.deltaTime));
      }
      t += Time.deltaTime;
      yield return null;
    }
    IsWhipStuck = false;
    Destroy(whip.gameObject);
    whip = null;
  }
}