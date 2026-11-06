using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000F RID: 15
	internal struct IntPoint
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000033E1 File Offset: 0x000015E1
		public IntPoint(long X, long Y)
		{
			this.X = X;
			this.Y = Y;
			this.NX = 0.0;
			this.NY = 0.0;
			this.N = -1L;
			this.D = 0L;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000341F File Offset: 0x0000161F
		public IntPoint(double x, double y)
		{
			this.X = (long)x;
			this.Y = (long)y;
			this.NX = 0.0;
			this.NY = 0.0;
			this.N = -1L;
			this.D = 0L;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003460 File Offset: 0x00001660
		public IntPoint(IntPoint pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
			this.NX = pt.NX;
			this.NY = pt.NY;
			this.N = pt.N;
			this.D = pt.D;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000034B5 File Offset: 0x000016B5
		public static bool operator ==(IntPoint a, IntPoint b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000034D5 File Offset: 0x000016D5
		public static bool operator !=(IntPoint a, IntPoint b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000034F8 File Offset: 0x000016F8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is IntPoint)
			{
				IntPoint intPoint = (IntPoint)obj;
				return this.X == intPoint.X && this.Y == intPoint.Y;
			}
			return false;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003539 File Offset: 0x00001739
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400002F RID: 47
		public long N;

		// Token: 0x04000030 RID: 48
		public long X;

		// Token: 0x04000031 RID: 49
		public long Y;

		// Token: 0x04000032 RID: 50
		public long D;

		// Token: 0x04000033 RID: 51
		public double NX;

		// Token: 0x04000034 RID: 52
		public double NY;
	}
}
