using UnityEngine;

public class Whip : MonoBehaviour {
  public GameObject StuckToObject = null;

  void OnCollisionEnter(Collision collision) {
    if (!StuckToObject)
      StuckToObject = collision.gameObject;
  }
}