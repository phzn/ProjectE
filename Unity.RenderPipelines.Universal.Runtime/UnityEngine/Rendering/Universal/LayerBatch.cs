using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000030 RID: 48
	internal struct LayerBatch
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000EA54 File Offset: 0x0000CC54
		public unsafe void InitRTIds(int index)
		{
			for (int i = 0; i < 4; i++)
			{
				*((ref this.renderTargetUsed.FixedElementField) + i) = false;
				*((ref this.renderTargetIds.FixedElementField) + (IntPtr)i * 4) = Shader.PropertyToID(string.Format("_LightTexture_{0}_{1}", index, i));
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
		public unsafe RenderTargetIdentifier GetRTId(CommandBuffer cmd, RenderTextureDescriptor desc, int index)
		{
			if (!(*((ref this.renderTargetUsed.FixedElementField) + index)))
			{
				cmd.GetTemporaryRT(*((ref this.renderTargetIds.FixedElementField) + (IntPtr)index * 4), desc, FilterMode.Bilinear);
				*((ref this.renderTargetUsed.FixedElementField) + index) = true;
			}
			return new RenderTargetIdentifier(*((ref this.renderTargetIds.FixedElementField) + (IntPtr)index * 4));
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000EB04 File Offset: 0x0000CD04
		public unsafe void ReleaseRT(CommandBuffer cmd)
		{
			for (int i = 0; i < 4; i++)
			{
				if (*((ref this.renderTargetUsed.FixedElementField) + i))
				{
					cmd.ReleaseTemporaryRT(*((ref this.renderTargetIds.FixedElementField) + (IntPtr)i * 4));
					*((ref this.renderTargetUsed.FixedElementField) + i) = false;
				}
			}
		}

		// Token: 0x04000101 RID: 257
		public int startLayerID;

		// Token: 0x04000102 RID: 258
		public int endLayerValue;

		// Token: 0x04000103 RID: 259
		public SortingLayerRange layerRange;

		// Token: 0x04000104 RID: 260
		public LightStats lightStats;

		// Token: 0x04000105 RID: 261
		public bool useNormals;

		// Token: 0x04000106 RID: 262
		[FixedBuffer(typeof(int), 4)]
		private LayerBatch.<renderTargetIds>e__FixedBuffer renderTargetIds;

		// Token: 0x04000107 RID: 263
		[FixedBuffer(typeof(bool), 4)]
		private LayerBatch.<renderTargetUsed>e__FixedBuffer renderTargetUsed;

		// Token: 0x0200015D RID: 349
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public struct <renderTargetIds>e__FixedBuffer
		{
			// Token: 0x0400099C RID: 2460
			public int FixedElementField;
		}

		// Token: 0x0200015E RID: 350
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <renderTargetUsed>e__FixedBuffer
		{
			// Token: 0x0400099D RID: 2461
			public bool FixedElementField;
		}
	}
}
