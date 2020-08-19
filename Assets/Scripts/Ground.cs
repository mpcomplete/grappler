using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Ground : MonoBehaviour {
  public AnimationCurve Curve;
  public int NumSegments = 64;
  public float Thickness = 2f;
  Mesh mesh;

  void Start() {
		Generate();
	}

  [ContextMenu("Generate")]
	private void Generate() {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();

		Vector3[] vertices = new Vector3[3*(NumSegments+1)];
    Vector2[] uv = new Vector2[vertices.Length];
    for (int i = 0; i < NumSegments+1; i++) {
      float t = (float)i / (float)NumSegments;
      vertices[3*i] = new Vector3(-1, Curve.Evaluate(t)*100f, t*100f);
      vertices[3*i+1] = new Vector3(1, Curve.Evaluate(t)*100f, t*100f);
      vertices[3*i+2] = new Vector3(1, Curve.Evaluate(t)*100f - Thickness, t*100f);

      uv[3*i] = new Vector2(1, t);
      uv[3*i+1] = new Vector2(0, t);
      uv[3*i+2] = new Vector2(1, t);
    }
    mesh.vertices = vertices;
    mesh.uv = uv;

		int[] triangles = new int[4*3*NumSegments]; // 4 triangles, 3 verts each
		for (int i = 0; i < NumSegments; i++) {
      int ti = 4*3*i;
      int vi = 3*i;
      // Top face.
      triangles[ti+0] = vi;
      triangles[ti+1] = vi+3;
      triangles[ti+2] = vi+1;
      triangles[ti+3] = vi+1;
      triangles[ti+4] = vi+3;
      triangles[ti+5] = vi+4;
      // Front face.
      triangles[ti+6] = vi+1;
      triangles[ti+7] = vi+4;
      triangles[ti+8] = vi+2;
      triangles[ti+9] = vi+2;
      triangles[ti+10] = vi+4;
      triangles[ti+11] = vi+5;
    }
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    GetComponent<MeshCollider>().sharedMesh = mesh;
  }
}
