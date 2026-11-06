using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace MiragePro.Impostors.Core
{
	// Token: 0x02000277 RID: 631
	public class ImpostorMeshUtility
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x0006D77C File Offset: 0x0006B97C
		public static Mesh CopyMesh(Mesh mesh)
		{
			Mesh mesh2 = new Mesh();
			mesh2.indexFormat = mesh.indexFormat;
			mesh2.subMeshCount = mesh.subMeshCount;
			mesh2.vertices = mesh.vertices;
			mesh2.triangles = mesh.triangles;
			SubMeshDescriptor[] array = new SubMeshDescriptor[mesh.subMeshCount];
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				array[i] = mesh.GetSubMesh(i);
			}
			mesh2.SetSubMeshes(array, MeshUpdateFlags.Default);
			mesh2.uv = mesh.uv;
			mesh2.uv2 = mesh.uv2;
			mesh2.uv3 = mesh.uv3;
			mesh2.uv4 = mesh.uv4;
			mesh2.boneWeights = mesh.boneWeights;
			mesh2.normals = mesh.normals;
			mesh2.colors = mesh.colors;
			mesh2.tangents = mesh.tangents;
			mesh2.bounds = mesh.bounds;
			return mesh2;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0006D860 File Offset: 0x0006BA60
		public static Mesh BuildQuad(float size)
		{
			Mesh mesh = new Mesh();
			Vector3[] array = new Vector3[4];
			float num = size * 0.5f;
			float num2 = size * 0.5f;
			array[0] = new Vector3(-num2, -num, 0f);
			array[1] = new Vector3(-num2, num, 0f);
			array[2] = new Vector3(num2, -num, 0f);
			array[3] = new Vector3(num2, num, 0f);
			Vector2[] array2 = new Vector2[array.Length];
			array2[0] = new Vector2(0f, 0f);
			array2[1] = new Vector2(0f, 1f);
			array2[2] = new Vector2(1f, 0f);
			array2[3] = new Vector2(1f, 1f);
			int[] array3 = new int[] { 0, 1, 2, 3, 2, 1 };
			Vector3[] array4 = new Vector3[array.Length];
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i] = Vector3.back;
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			mesh.normals = array4;
			return mesh;
		}
	}
}
