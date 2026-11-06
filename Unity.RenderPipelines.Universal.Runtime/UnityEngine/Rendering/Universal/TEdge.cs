using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000018 RID: 24
	internal class TEdge
	{
		// Token: 0x04000051 RID: 81
		internal IntPoint Bot;

		// Token: 0x04000052 RID: 82
		internal IntPoint Curr;

		// Token: 0x04000053 RID: 83
		internal IntPoint Top;

		// Token: 0x04000054 RID: 84
		internal IntPoint Delta;

		// Token: 0x04000055 RID: 85
		internal double Dx;

		// Token: 0x04000056 RID: 86
		internal PolyType PolyTyp;

		// Token: 0x04000057 RID: 87
		internal EdgeSide Side;

		// Token: 0x04000058 RID: 88
		internal int WindDelta;

		// Token: 0x04000059 RID: 89
		internal int WindCnt;

		// Token: 0x0400005A RID: 90
		internal int WindCnt2;

		// Token: 0x0400005B RID: 91
		internal int OutIdx;

		// Token: 0x0400005C RID: 92
		internal TEdge Next;

		// Token: 0x0400005D RID: 93
		internal TEdge Prev;

		// Token: 0x0400005E RID: 94
		internal TEdge NextInLML;

		// Token: 0x0400005F RID: 95
		internal TEdge NextInAEL;

		// Token: 0x04000060 RID: 96
		internal TEdge PrevInAEL;

		// Token: 0x04000061 RID: 97
		internal TEdge NextInSEL;

		// Token: 0x04000062 RID: 98
		internal TEdge PrevInSEL;
	}
}
