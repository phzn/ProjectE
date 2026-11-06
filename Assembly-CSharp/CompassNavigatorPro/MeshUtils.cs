using System;
using System.Collections.Generic;
using UnityEngine;

namespace CompassNavigatorPro
{
	// Token: 0x020002A7 RID: 679
	public static class MeshUtils
	{
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x0007CEA4 File Offset: 0x0007B0A4
		public static Mesh quadMesh
		{
			get
			{
				if (MeshUtils._quadMesh != null)
				{
					return MeshUtils._quadMesh;
				}
				MeshUtils._quadMesh = new Mesh();
				MeshUtils._quadMesh.SetVertices(new List<Vector3>
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(1f, 1f, 0f)
				});
				MeshUtils._quadMesh.SetUVs(0, new List<Vector2>
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 1f),
					new Vector2(1f, 0f),
					new Vector2(1f, 1f)
				});
				MeshUtils._quadMesh.SetIndices(new int[] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, false);
				MeshUtils._quadMesh.UploadMeshData(true);
				return MeshUtils._quadMesh;
			}
		}

		// Token: 0x040014B3 RID: 5299
		private static Mesh _quadMesh;
	}
}
