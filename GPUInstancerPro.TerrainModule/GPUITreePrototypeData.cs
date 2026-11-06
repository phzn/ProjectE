using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class GPUITreePrototypeData : GPUIPrototypeData, IGPUIParameterBufferData
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x000057C2 File Offset: 0x000039C2
		public GPUITreePrototypeData()
		{
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000057C2 File Offset: 0x000039C2
		public GPUITreePrototypeData(TreePrototype treePrototype)
		{
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000057DF File Offset: 0x000039DF
		public override bool Initialize(GPUIPrototype prototype)
		{
			return base.Initialize(prototype);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000057E8 File Offset: 0x000039E8
		public override void ReleaseBuffers()
		{
			base.ReleaseBuffers();
			if (this._treeInstanceDataBuffer != null)
			{
				this._treeInstanceDataBuffer.Release();
			}
		}

		// Token: 0x04000078 RID: 120
		[SerializeField]
		public bool isApplyRotation = true;

		// Token: 0x04000079 RID: 121
		[SerializeField]
		public bool isApplyPrefabScale = true;

		// Token: 0x0400007A RID: 122
		[SerializeField]
		public bool isApplyHeight = true;

		// Token: 0x0400007B RID: 123
		internal GraphicsBuffer _treeInstanceDataBuffer;
	}
}
