using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x0200001E RID: 30
	[Serializable]
	public class GPUIRendererData : IGPUIDisposable, IDisposable
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000651F File Offset: 0x0000471F
		public bool IsShadowCasting
		{
			get
			{
				return this.shadowCastingMode > ShadowCastingMode.Off;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000652A File Offset: 0x0000472A
		public bool IsShadowsOnly
		{
			get
			{
				return this.shadowCastingMode == ShadowCastingMode.ShadowsOnly;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006535 File Offset: 0x00004735
		public GPUIRendererData()
		{
			this.transformOffset = Matrix4x4.identity;
			this.rendererMaterials = new Material[0];
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000655C File Offset: 0x0000475C
		public GPUIRendererData(Mesh mesh, Material[] materials, Matrix4x4 transformOffset, int layer, ShadowCastingMode shadowCastingMode, bool receiveShadows, MotionVectorGenerationMode motionVectorGenerationMode, bool isSkinnedMesh, bool doesNotContributeToBounds, uint renderingLayerMask, LightProbeUsage lightProbeUsage, int forceMeshLod = -1, Matrix4x4 boundsOffset = default(Matrix4x4))
		{
			if (transformOffset == Matrix4x4.zero)
			{
				transformOffset = Matrix4x4.identity;
			}
			if (boundsOffset == Matrix4x4.zero)
			{
				boundsOffset = Matrix4x4.identity;
			}
			this.rendererMesh = mesh;
			this.rendererMaterials = materials;
			this.transformOffset = transformOffset;
			this.layer = layer;
			this.shadowCastingMode = shadowCastingMode;
			this.receiveShadows = receiveShadows;
			this.motionVectorGenerationMode = motionVectorGenerationMode;
			this.isSkinnedMesh = isSkinnedMesh;
			this.boundsOffset = boundsOffset;
			this.doesNotContributeToBounds = doesNotContributeToBounds;
			this.renderingLayerMask = renderingLayerMask;
			this.lightProbeUsage = ((lightProbeUsage != LightProbeUsage.Off) ? LightProbeUsage.BlendProbes : LightProbeUsage.Off);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006602 File Offset: 0x00004802
		public void InitializeReplacementMaterials(GPUIMaterialProvider materialProvider)
		{
			this.replacementMaterials = new Material[this.rendererMaterials.Length];
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006618 File Offset: 0x00004818
		public void RemoveReplacementMaterials()
		{
			if (this.replacementMaterials == null)
			{
				return;
			}
			for (int i = 0; i < this.replacementMaterials.Length; i++)
			{
				this.replacementMaterials[i] = null;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000664A File Offset: 0x0000484A
		public Mesh GetMesh()
		{
			if (this.replacementMesh != null)
			{
				return this.replacementMesh;
			}
			return this.rendererMesh;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000024BC File Offset: 0x000006BC
		public void ReleaseBuffers()
		{
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006667 File Offset: 0x00004867
		public void Dispose()
		{
			if (this.replacementMesh != null)
			{
				this.replacementMesh.DestroyGeneric();
			}
		}

		// Token: 0x0400007C RID: 124
		public Mesh rendererMesh;

		// Token: 0x0400007D RID: 125
		public Material[] rendererMaterials;

		// Token: 0x0400007E RID: 126
		public Matrix4x4 transformOffset;

		// Token: 0x0400007F RID: 127
		public int layer;

		// Token: 0x04000080 RID: 128
		public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;

		// Token: 0x04000081 RID: 129
		public bool receiveShadows;

		// Token: 0x04000082 RID: 130
		public MotionVectorGenerationMode motionVectorGenerationMode;

		// Token: 0x04000083 RID: 131
		public bool isSkinnedMesh;

		// Token: 0x04000084 RID: 132
		public Matrix4x4 boundsOffset;

		// Token: 0x04000085 RID: 133
		public bool doesNotContributeToBounds;

		// Token: 0x04000086 RID: 134
		public uint renderingLayerMask;

		// Token: 0x04000087 RID: 135
		public int optionalRendererNo;

		// Token: 0x04000088 RID: 136
		public LightProbeUsage lightProbeUsage;

		// Token: 0x04000089 RID: 137
		[NonSerialized]
		public Material[] replacementMaterials;

		// Token: 0x0400008A RID: 138
		[NonSerialized]
		public Mesh replacementMesh;
	}
}
