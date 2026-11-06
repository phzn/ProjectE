using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000107 RID: 263
	public static class MeshUtility
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x0002D61C File Offset: 0x0002B81C
		public static Mesh GetSharedMesh(Renderer ren)
		{
			if (ren == null)
			{
				return null;
			}
			if (ren is SkinnedMeshRenderer)
			{
				return (ren as SkinnedMeshRenderer).sharedMesh;
			}
			MeshFilter meshFilter;
			if (!ren.TryGetComponent<MeshFilter>(out meshFilter))
			{
				Debug.LogError("Not found MeshFilter!");
				return null;
			}
			return meshFilter.sharedMesh;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0002D664 File Offset: 0x0002B864
		public static bool SetMesh(Renderer ren, Mesh mesh, Transform[] skinBones = null)
		{
			if (ren is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = ren as SkinnedMeshRenderer;
				skinnedMeshRenderer.sharedMesh = mesh;
				if (skinBones != null && skinBones.Length != 0)
				{
					skinnedMeshRenderer.bones = skinBones;
				}
			}
			else
			{
				MeshFilter meshFilter;
				if (!ren.TryGetComponent<MeshFilter>(out meshFilter))
				{
					Debug.LogError("Not found MeshFilter!");
					return false;
				}
				meshFilter.mesh = mesh;
			}
			return true;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
		public static int GetTransformCount(Renderer ren)
		{
			int num = 0;
			if (ren)
			{
				num++;
				if (ren is SkinnedMeshRenderer)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = ren as SkinnedMeshRenderer;
					num++;
					int num2 = num;
					Transform[] bones = skinnedMeshRenderer.bones;
					num = num2 + ((bones != null) ? bones.Length : 0);
				}
			}
			return num;
		}
	}
}
