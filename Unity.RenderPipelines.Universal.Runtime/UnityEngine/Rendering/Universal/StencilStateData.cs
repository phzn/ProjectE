using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class StencilStateData
	{
		// Token: 0x040001AC RID: 428
		public bool overrideStencilState;

		// Token: 0x040001AD RID: 429
		public int stencilReference;

		// Token: 0x040001AE RID: 430
		public CompareFunction stencilCompareFunction = CompareFunction.Always;

		// Token: 0x040001AF RID: 431
		public StencilOp passOperation;

		// Token: 0x040001B0 RID: 432
		public StencilOp failOperation;

		// Token: 0x040001B1 RID: 433
		public StencilOp zFailOperation;
	}
}
