using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Ground : MonoBehaviour {
  public AnimationCurve Curve;
  public int NumSegments = 64;
  public float Height = 10f;
  public float Length = 100f;
  public float Thickness = 2f;
  Mesh mesh;

  void Start() {
		Generate();
	}

  float PHI = 1.61803398874989484820459f;
  float goldNoise(Vector2 xy, float seed) {
    return (Mathf.Tan(Vector2.Distance(xy*PHI, xy)*seed)*xy.x) % 1f;
  }

  float noise(Vector2 x) {
    Vector2 uv = new Vector2(Mathf.Floor(x.x), Mathf.Floor(x.y));
    Vector2 f = new Vector2(x.x % 1f, x.y % 1f);
    f = f*f*(new Vector2(3f, 3f) - 2.0f*f);

    float r1 = goldNoise(uv + new Vector2(0.5f, 0.5f), 1f);
    float r2 = goldNoise(uv + new Vector2(1.5f, 0.5f), 1f);
    float r3 = goldNoise(uv + new Vector2(0.5f, 1.5f), 1f);
    float r4 = goldNoise(uv + new Vector2(1.5f, 1.5f), 1f);
    return Mathf.Lerp(Mathf.Lerp(r1, r2, f.x), Mathf.Lerp(r3, r4, f.x), f.y);
  }

  float fbm(Vector2 p) {
    float f = 0f;
    f += 0.500000f*noise(p); p = p*2.02f;
    f += 0.250000f*noise(p); p = p*2.03f;
    f += 0.125000f*noise(p); p = p*2.01f;
    f += 0.062500f*noise(p); p = p*2.04f;
    return f/0.9375f;
  }

  [ContextMenu("Generate")]
	private void Generate() {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();

		Vector3[] vertices = new Vector3[3*(NumSegments+1)];
    Vector2[] uv = new Vector2[vertices.Length];
    for (int i = 0; i < NumSegments+1; i++) {
      float t = (float)i / (float)NumSegments;
      float y = (Curve.Evaluate(t) - 1)*Height;
      //float y = (fbm(new Vector2(t*3f, 0.1f)) - 1)*Height;
      vertices[3*i] = new Vector3(-1, y, t*Length);
      vertices[3*i+1] = new Vector3(1, y, t*Length);
      vertices[3*i+2] = new Vector3(1, y - Thickness, t*Length);

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
