using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Splines
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	internal class SplinePathRef
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000C31D File Offset: 0x0000A51D
		public SplinePathRef()
		{
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C325 File Offset: 0x0000A525
		public SplinePathRef(IEnumerable<SplinePathRef.SliceRef> slices)
		{
			this.Splines = slices.ToArray<SplinePathRef.SliceRef>();
		}

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		public SplinePathRef.SliceRef[] Splines;

		// Token: 0x0200006D RID: 109
		[Serializable]
		public class SliceRef
		{
			// Token: 0x06000379 RID: 889 RVA: 0x00010D5B File Offset: 0x0000EF5B
			public SliceRef(int splineIndex, SplineRange range)
			{
				this.Index = splineIndex;
				this.Range = range;
			}

			// Token: 0x04000168 RID: 360
			[SerializeField]
			public int Index;

			// Token: 0x04000169 RID: 361
			[SerializeField]
			public SplineRange Range;
		}
	}
}
