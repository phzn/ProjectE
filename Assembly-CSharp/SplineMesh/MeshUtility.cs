using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x0200019E RID: 414
	public class MeshUtility
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x000576BC File Offset: 0x000558BC
		public static int[] GetReversedTriangles(Mesh mesh)
		{
			int[] array = mesh.triangles.ToArray<int>();
			int num = array.Length / 3;
			for (int i = 0; i < num; i++)
			{
				int num2 = array[i * 3];
				array[i * 3] = array[i * 3 + 1];
				array[i * 3 + 1] = num2;
			}
			return array;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00057704 File Offset: 0x00055904
		public static void Update(Mesh mesh, Mesh source, IEnumerable<int> triangles = null, IEnumerable<Vector3> vertices = null, IEnumerable<Vector3> normals = null, IEnumerable<Vector2> uv = null, IEnumerable<Vector2> uv2 = null, IEnumerable<Vector2> uv3 = null, IEnumerable<Vector2> uv4 = null, IEnumerable<Vector2> uv5 = null, IEnumerable<Vector2> uv6 = null, IEnumerable<Vector2> uv7 = null, IEnumerable<Vector2> uv8 = null)
		{
			mesh.hideFlags = source.hideFlags;
			mesh.indexFormat = source.indexFormat;
			mesh.triangles = new int[0];
			mesh.vertices = ((vertices == null) ? source.vertices : vertices.ToArray<Vector3>());
			mesh.normals = ((normals == null) ? source.normals : normals.ToArray<Vector3>());
			mesh.uv = ((uv == null) ? source.uv : uv.ToArray<Vector2>());
			mesh.uv2 = ((uv2 == null) ? source.uv2 : uv2.ToArray<Vector2>());
			mesh.uv3 = ((uv3 == null) ? source.uv3 : uv3.ToArray<Vector2>());
			mesh.uv4 = ((uv4 == null) ? source.uv4 : uv4.ToArray<Vector2>());
			mesh.uv5 = ((uv5 == null) ? source.uv5 : uv5.ToArray<Vector2>());
			mesh.uv6 = ((uv6 == null) ? source.uv6 : uv6.ToArray<Vector2>());
			mesh.uv7 = ((uv7 == null) ? source.uv7 : uv7.ToArray<Vector2>());
			mesh.uv8 = ((uv8 == null) ? source.uv8 : uv8.ToArray<Vector2>());
			mesh.triangles = ((triangles == null) ? source.triangles : triangles.ToArray<int>());
			mesh.RecalculateBounds();
			mesh.RecalculateTangents();
		}
	}
}
