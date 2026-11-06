using System;

namespace UnityEngine.Splines
{
	// Token: 0x02000033 RID: 51
	internal struct SplineModificationData
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000C2F6 File Offset: 0x0000A4F6
		public SplineModificationData(Spline spline, SplineModification modification, int knotIndex, float prevCurveLength, float nextCurveLength)
		{
			this.Spline = spline;
			this.Modification = modification;
			this.KnotIndex = knotIndex;
			this.PrevCurveLength = prevCurveLength;
			this.NextCurveLength = nextCurveLength;
		}

		// Token: 0x040000D2 RID: 210
		public readonly Spline Spline;

		// Token: 0x040000D3 RID: 211
		public readonly SplineModification Modification;

		// Token: 0x040000D4 RID: 212
		public readonly int KnotIndex;

		// Token: 0x040000D5 RID: 213
		public readonly float PrevCurveLength;

		// Token: 0x040000D6 RID: 214
		public readonly float NextCurveLength;
	}
}
