using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000009 RID: 9
	internal class XROcclusionMesh
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000031A8 File Offset: 0x000013A8
		internal XROcclusionMesh(XRPass xrPass)
		{
			this.m_Pass = xrPass;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000031B7 File Offset: 0x000013B7
		internal void SetMaterial(Material mat)
		{
			this.m_Material = mat;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000031C0 File Offset: 0x000013C0
		internal bool hasValidOcclusionMesh
		{
			get
			{
				if (!this.IsOcclusionMeshSupported())
				{
					return false;
				}
				if (this.m_Pass.singlePassEnabled)
				{
					return this.m_CombinedMesh != null;
				}
				return this.m_Pass.GetOcclusionMesh(0) != null;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000031F8 File Offset: 0x000013F8
		internal void RenderOcclusionMesh(CommandBuffer cmd, float occlusionMeshScale, bool yFlip = false)
		{
			if (this.IsOcclusionMeshSupported())
			{
				using (new ProfilingScope(cmd, XROcclusionMesh.k_OcclusionMeshProfilingSampler))
				{
					if (this.m_Pass.singlePassEnabled)
					{
						if (this.m_CombinedMesh != null && SystemInfo.supportsMultiview)
						{
							cmd.EnableShaderKeyword("XR_OCCLUSION_MESH_COMBINED");
							Vector3 vector = new Vector3(occlusionMeshScale, yFlip ? occlusionMeshScale : (-occlusionMeshScale), 1f);
							cmd.DrawMesh(this.m_CombinedMesh, Matrix4x4.Scale(vector), this.m_Material);
							cmd.DisableShaderKeyword("XR_OCCLUSION_MESH_COMBINED");
						}
						else if (this.m_CombinedMesh != null && SystemInfo.supportsRenderTargetArrayIndexFromVertexShader)
						{
							this.m_Pass.StopSinglePass(cmd);
							cmd.EnableShaderKeyword("XR_OCCLUSION_MESH_COMBINED");
							Vector3 vector2 = new Vector3(occlusionMeshScale, yFlip ? occlusionMeshScale : (-occlusionMeshScale), 1f);
							cmd.DrawMesh(this.m_CombinedMesh, Matrix4x4.Scale(vector2), this.m_Material);
							cmd.DisableShaderKeyword("XR_OCCLUSION_MESH_COMBINED");
							this.m_Pass.StartSinglePass(cmd);
						}
					}
					else
					{
						Mesh occlusionMesh = this.m_Pass.GetOcclusionMesh(0);
						if (occlusionMesh != null)
						{
							cmd.DrawMesh(occlusionMesh, Matrix4x4.identity, this.m_Material);
						}
					}
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003358 File Offset: 0x00001558
		internal void UpdateCombinedMesh()
		{
			int num;
			if (this.IsOcclusionMeshSupported() && this.m_Pass.singlePassEnabled && this.TryGetOcclusionMeshCombinedHashCode(out num))
			{
				if (this.m_CombinedMesh == null || num != this.m_CombinedMeshHashCode)
				{
					this.CreateOcclusionMeshCombined();
					this.m_CombinedMeshHashCode = num;
					return;
				}
			}
			else
			{
				this.m_CombinedMesh = null;
				this.m_CombinedMeshHashCode = 0;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000033B7 File Offset: 0x000015B7
		private bool IsOcclusionMeshSupported()
		{
			return this.m_Pass.enabled && this.m_Material != null;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000033D4 File Offset: 0x000015D4
		private bool TryGetOcclusionMeshCombinedHashCode(out int hashCode)
		{
			hashCode = 17;
			for (int i = 0; i < this.m_Pass.viewCount; i++)
			{
				Mesh occlusionMesh = this.m_Pass.GetOcclusionMesh(i);
				if (!(occlusionMesh != null))
				{
					hashCode = 0;
					return false;
				}
				hashCode = hashCode * 23 + occlusionMesh.GetHashCode();
			}
			return true;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003428 File Offset: 0x00001628
		private void CreateOcclusionMeshCombined()
		{
			CoreUtils.Destroy(this.m_CombinedMesh);
			this.m_CombinedMesh = new Mesh();
			this.m_CombinedMesh.indexFormat = IndexFormat.UInt16;
			int num = 0;
			uint num2 = 0U;
			for (int i = 0; i < this.m_Pass.viewCount; i++)
			{
				Mesh occlusionMesh = this.m_Pass.GetOcclusionMesh(i);
				num += occlusionMesh.vertexCount;
				num2 += occlusionMesh.GetIndexCount(0);
			}
			Vector3[] array = new Vector3[num];
			ushort[] array2 = new ushort[num2];
			int num3 = 0;
			int num4 = 0;
			for (int j = 0; j < this.m_Pass.viewCount; j++)
			{
				Mesh occlusionMesh2 = this.m_Pass.GetOcclusionMesh(j);
				int[] indices = occlusionMesh2.GetIndices(0);
				occlusionMesh2.vertices.CopyTo(array, num3);
				for (int k = 0; k < occlusionMesh2.vertices.Length; k++)
				{
					array[num3 + k].z = (float)j;
				}
				for (int l = 0; l < indices.Length; l++)
				{
					int num5 = num3 + indices[l];
					array2[num4 + l] = (ushort)num5;
				}
				num3 += occlusionMesh2.vertexCount;
				num4 += indices.Length;
			}
			this.m_CombinedMesh.vertices = array;
			this.m_CombinedMesh.SetIndices(array2, MeshTopology.Triangles, 0, true, 0);
		}

		// Token: 0x0400002A RID: 42
		private XRPass m_Pass;

		// Token: 0x0400002B RID: 43
		private Mesh m_CombinedMesh;

		// Token: 0x0400002C RID: 44
		private Material m_Material;

		// Token: 0x0400002D RID: 45
		private int m_CombinedMeshHashCode;

		// Token: 0x0400002E RID: 46
		private static readonly ProfilingSampler k_OcclusionMeshProfilingSampler = new ProfilingSampler("XR Occlusion Mesh");
	}
}
