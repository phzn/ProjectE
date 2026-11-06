using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200001E RID: 30
	internal class OutRec
	{
		// Token: 0x0400006F RID: 111
		internal int Idx;

		// Token: 0x04000070 RID: 112
		internal bool IsHole;

		// Token: 0x04000071 RID: 113
		internal bool IsOpen;

		// Token: 0x04000072 RID: 114
		internal OutRec FirstLeft;

		// Token: 0x04000073 RID: 115
		internal OutPt Pts;

		// Token: 0x04000074 RID: 116
		internal OutPt BottomPt;

		// Token: 0x04000075 RID: 117
		internal PolyNode PolyNode;
	}
}
