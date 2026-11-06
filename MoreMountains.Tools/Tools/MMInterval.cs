using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000101 RID: 257
	[Serializable]
	public struct MMInterval<T> where T : struct, IComparable
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x0001B334 File Offset: 0x00019534
		public MMInterval(T lowerBound, T upperBound, MMInterval<T>.MMIntervalType lowerboundIntervalType = MMInterval<T>.MMIntervalType.Inclusive, MMInterval<T>.MMIntervalType upperboundIntervalType = MMInterval<T>.MMIntervalType.Inclusive)
		{
			this = default(MMInterval<T>);
			T t = lowerBound;
			T t2 = upperBound;
			if (t.CompareTo(t2) > 0)
			{
				t = upperBound;
				t2 = lowerBound;
			}
			this.LowerBound = t;
			this.UpperBound = t2;
			this.LowerBoundIntervalType = lowerboundIntervalType;
			this.UpperBoundIntervalType = upperboundIntervalType;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001B384 File Offset: 0x00019584
		public bool Contains(T value)
		{
			bool flag = ((this.LowerBoundIntervalType == MMInterval<T>.MMIntervalType.Exclusive) ? (this.LowerBound.CompareTo(value) < 0) : (this.LowerBound.CompareTo(value) <= 0));
			bool flag2 = ((this.UpperBoundIntervalType == MMInterval<T>.MMIntervalType.Exclusive) ? (this.UpperBound.CompareTo(value) > 0) : (this.UpperBound.CompareTo(value) >= 0));
			return flag && flag2;
		}

		// Token: 0x040006F1 RID: 1777
		[Tooltip("the lower bound of this interval")]
		public T LowerBound;

		// Token: 0x040006F2 RID: 1778
		[Tooltip("the upper bound of this interval")]
		public T UpperBound;

		// Token: 0x040006F3 RID: 1779
		[Tooltip("whether to include or exclude the lower bound in the interval")]
		public MMInterval<T>.MMIntervalType LowerBoundIntervalType;

		// Token: 0x040006F4 RID: 1780
		[Tooltip("whether to include or exclude the upper bound in the interval")]
		public MMInterval<T>.MMIntervalType UpperBoundIntervalType;

		// Token: 0x0200042E RID: 1070
		public enum MMIntervalType
		{
			// Token: 0x04001F78 RID: 8056
			Inclusive,
			// Token: 0x04001F79 RID: 8057
			Exclusive
		}
	}
}
