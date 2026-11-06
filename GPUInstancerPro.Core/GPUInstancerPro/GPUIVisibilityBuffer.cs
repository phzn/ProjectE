using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000019 RID: 25
	public class GPUIVisibilityBuffer : GPUIDataBuffer<GPUIVisibilityData>
	{
		// Token: 0x060000AF RID: 175 RVA: 0x0000478A File Offset: 0x0000298A
		public GPUIVisibilityBuffer(GPUICameraData cameraData, string name, GraphicsBuffer.Target target = GraphicsBuffer.Target.Structured)
			: base(name, 0, target)
		{
			this.cameraData = cameraData;
		}

		// Token: 0x0400005F RID: 95
		public GPUICameraData cameraData;
	}
}
