using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace AutoLOD.MeshDecimator
{
	// Token: 0x02001540 RID: 5440
	public class AutoLODMeshUtility
	{
		// Token: 0x0600A86B RID: 43115 RVA: 0x004647C0 File Offset: 0x004629C0
		public static void Smooth2FlatShading(Mesh mesh)
		{
			Vector3[] vertices = mesh.vertices;
			Vector2[] uv = mesh.uv;
			Vector2[] uv2 = mesh.uv2;
			Vector2[] uv3 = mesh.uv3;
			Vector2[] uv4 = mesh.uv4;
			Color[] colors = mesh.colors;
			BoneWeight[] boneWeights = mesh.boneWeights;
			bool flag = uv.Length != 0;
			bool flag2 = uv2.Length != 0;
			bool flag3 = uv3.Length != 0;
			bool flag4 = uv4.Length != 0;
			bool flag5 = colors.Length != 0;
			bool flag6 = boneWeights.Length != 0;
			List<Vector3> list = new List<Vector3>();
			List<int>[] array = new List<int>[mesh.subMeshCount];
			List<Vector2> list2 = new List<Vector2>();
			List<Vector2> list3 = new List<Vector2>();
			List<Vector2> list4 = new List<Vector2>();
			List<Vector2> list5 = new List<Vector2>();
			List<Color> list6 = new List<Color>();
			List<BoneWeight> list7 = new List<BoneWeight>();
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				int[] triangles = mesh.GetTriangles(i);
				array[i] = new List<int>();
				foreach (int num in triangles)
				{
					list.Add(vertices[num]);
					array[i].Add(list.Count - 1);
					if (flag)
					{
						list2.Add(uv[num]);
					}
					if (flag2)
					{
						list3.Add(uv2[num]);
					}
					if (flag3)
					{
						list4.Add(uv3[num]);
					}
					if (flag4)
					{
						list5.Add(uv4[num]);
					}
					if (flag5)
					{
						list6.Add(colors[num]);
					}
					if (flag6)
					{
						list7.Add(boneWeights[num]);
					}
				}
			}
			if (list.Count > 65535)
			{
				mesh.indexFormat = IndexFormat.UInt32;
			}
			mesh.vertices = list.ToArray();
			if (flag)
			{
				mesh.uv = list2.ToArray();
			}
			if (flag2)
			{
				mesh.uv2 = list3.ToArray();
			}
			if (flag3)
			{
				mesh.uv3 = list4.ToArray();
			}
			if (flag4)
			{
				mesh.uv4 = list5.ToArray();
			}
			if (flag5)
			{
				mesh.colors = list6.ToArray();
			}
			if (flag6)
			{
				mesh.boneWeights = list7.ToArray();
			}
			for (int k = 0; k < mesh.subMeshCount; k++)
			{
				mesh.SetTriangles(array[k].ToArray(), k);
			}
			mesh.RecalculateNormals();
		}
	}
}
