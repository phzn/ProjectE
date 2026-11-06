using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D5 RID: 213
	public struct ShadowSliceData
	{
		// Token: 0x060006AD RID: 1709 RVA: 0x00027E44 File Offset: 0x00026044
		public void Clear()
		{
			this.viewMatrix = Matrix4x4.identity;
			this.projectionMatrix = Matrix4x4.identity;
			this.shadowTransform = Matrix4x4.identity;
			this.offsetX = (this.offsetY = 0);
			this.resolution = 1024;
		}

		// Token: 0x04000553 RID: 1363
		public Matrix4x4 viewMatrix;

		// Token: 0x04000554 RID: 1364
		public Matrix4x4 projectionMatrix;

		// Token: 0x04000555 RID: 1365
		public Matrix4x4 shadowTransform;

		// Token: 0x04000556 RID: 1366
		public int offsetX;

		// Token: 0x04000557 RID: 1367
		public int offsetY;

		// Token: 0x04000558 RID: 1368
		public int resolution;

		// Token: 0x04000559 RID: 1369
		public ShadowSplitData splitData;
	}
}
