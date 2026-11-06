using System;

namespace UnityEngine.Splines
{
	// Token: 0x0200002E RID: 46
	[Serializable]
	public struct SplineKnotIndex : IEquatable<SplineKnotIndex>
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000AD64 File Offset: 0x00008F64
		public SplineKnotIndex(int spline, int knot)
		{
			this.Spline = spline;
			this.Knot = knot;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AD74 File Offset: 0x00008F74
		public static bool operator ==(SplineKnotIndex indexA, SplineKnotIndex indexB)
		{
			return indexA.Equals(indexB);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000AD7E File Offset: 0x00008F7E
		public static bool operator !=(SplineKnotIndex indexA, SplineKnotIndex indexB)
		{
			return !indexA.Equals(indexB);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000AD8B File Offset: 0x00008F8B
		public bool Equals(SplineKnotIndex otherIndex)
		{
			return this.Spline == otherIndex.Spline && this.Knot == otherIndex.Knot;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000ADAC File Offset: 0x00008FAC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is SplineKnotIndex)
			{
				SplineKnotIndex splineKnotIndex = (SplineKnotIndex)obj;
				return this.Equals(splineKnotIndex);
			}
			return false;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000ADD6 File Offset: 0x00008FD6
		public bool IsValid()
		{
			return this.Spline >= 0 && this.Knot >= 0;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000ADEF File Offset: 0x00008FEF
		public override int GetHashCode()
		{
			return (this.Spline * 397) ^ this.Knot;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000AE04 File Offset: 0x00009004
		public override string ToString()
		{
			return string.Format("{{{0}, {1}}}", this.Spline, this.Knot);
		}

		// Token: 0x040000B8 RID: 184
		public static SplineKnotIndex Invalid = new SplineKnotIndex(-1, -1);

		// Token: 0x040000B9 RID: 185
		public int Spline;

		// Token: 0x040000BA RID: 186
		public int Knot;
	}
}
