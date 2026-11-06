using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200002E RID: 46
	public struct FastMemoryDesc
	{
		// Token: 0x040000F6 RID: 246
		public bool inFastMemory;

		// Token: 0x040000F7 RID: 247
		public FastMemoryFlags flags;

		// Token: 0x040000F8 RID: 248
		public float residencyFraction;
	}
}
