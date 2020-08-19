using UnityEngine;

public class SmartCamera : MonoBehaviour {
  public float Distance = 50f;
  public void LookAt(Transform target) {
    Vector3 position = target.transform.position + Vector3.right * Distance;
    Quaternion rotation = Quaternion.LookRotation(-Vector3.right, Vector3.up);

    transform.SetPositionAndRotation(position, rotation);
  }
}