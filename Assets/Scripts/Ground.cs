using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Ground : MonoBehaviour {
  public AnimationCurve Curve;
  public int NumBricks;
	Mesh mesh;

  void Start() {
		Generate();
	}

	private void Generate() {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();

		Vector3[] vertices = new Vector3[2*(NumBricks+1)];
    Vector2[] uv = new Vector2[vertices.Length];
    for (int i = 0; i < NumBricks+1; i++) {
      float t = (float)i / (float)NumBricks;
			vertices[2*i] = new Vector3(-1, Curve.Evaluate(t)*100f, t*100f);
			vertices[2*i+1] = new Vector3(1, Curve.Evaluate(t)*100f, t*100f);

      uv[2*i] = new Vector2(0, t);
      uv[2*i+1] = new Vector2(1, t);
    }
		mesh.vertices = vertices;
    mesh.uv = uv;

		int[] triangles = new int[6*NumBricks];
		for (int i = 0; i < NumBricks; i++) {
      int ti = 6*i;
      int vi = 2*i;
      triangles[ti] = vi;
      triangles[ti+4] = triangles[ti+1] = vi+2;
      triangles[ti+3] = triangles[ti+2] = vi+1;
      triangles[ti+5] = vi+3;
    }
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    GetComponent<MeshCollider>().sharedMesh = mesh;
  }
}
