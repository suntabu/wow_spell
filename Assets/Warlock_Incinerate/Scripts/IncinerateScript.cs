using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IncinerateScript : MonoBehaviour
{
	public Transform Target;

	private static int SEGMENTS_COUNT = 21;
	private static int WIDTH = 50;
	private static int SPEED = 10;

	private MeshFilter mMeshFilter;
	private Mesh mMesh;
	private Vector3 mMoveDir;


	private bool mIsInit;

	void Start ()
	{
		mMeshFilter = GetComponent<MeshFilter> ();

		if (mMeshFilter != null) {

			InitVertices (mMeshFilter);

			mIsInit = true;
		}
	}

	void Update ()
	{
		if (Target)
			mMoveDir = Target.position - transform.position;

		if (mIsInit)
			UpdateVertices ();
	}

	private void InitVertices (MeshFilter meshFilter)
	{
		var vertices = new Vector3[(SEGMENTS_COUNT + 1) * 2];
		var uvs = new Vector2[vertices.Length];
		var indices = new int[SEGMENTS_COUNT * 2 * 3];


		for (var i = 0; i < vertices.Length; i++) {
			vertices [i] = transform.position;
		}

		for (var i = 0; i < uvs.Length; i++) {
			float x = 0, y = i / (float)SEGMENTS_COUNT;
			if (i % 2 == 0) {
				x = 0;
			} else {
				x = 1;
			}
			uvs [i] = new Vector2 (x, y);
		}

		for (int i = 0; i < SEGMENTS_COUNT; i++) {
			indices [6 * i + 0] = 0 + i * 2;
			indices [6 * i + 1] = 1 + i * 2;
			indices [6 * i + 2] = 2 + i * 2;


			indices [6 * i + 3 + 0] = 2 + i * 2;
			indices [6 * i + 3 + 1] = 1 + i * 2;
			indices [6 * i + 3 + 2] = 3 + i * 2;

		}


		mMesh = meshFilter.mesh = new Mesh ();
		mMesh.vertices = vertices;
		mMesh.uv = uvs;
		mMesh.SetIndices (indices, MeshTopology.Triangles, 0);
		mMesh.RecalculateBounds ();

	}

	private void UpdateVertices ()
	{
		var vertices = mMesh.vertices;

		for (var i = 0; i < vertices.Length; i++) {
			var v = transform.position;
			var segInx = i / 2;
			var ratio = (SEGMENTS_COUNT - segInx * 1f) / SEGMENTS_COUNT;
			
			var seg = mMoveDir.normalized * segInx * WIDTH / 6f;

			var spine = Mathf.Sin (ratio * Mathf.PI * 2.5f + Time.time * 6) * Vector3.left * WIDTH * 1.5f;
			var wave = Vector3.left * WIDTH * Mathf.Min (1f, ratio + 0.5f);
			if (i % 2 == 0) {
				v = transform.position - seg - wave + spine;
			} else {
				v = transform.position - seg + wave + spine;
			}
 
			vertices [i] = v;
		}
		mMesh.vertices = vertices;
	}
}
