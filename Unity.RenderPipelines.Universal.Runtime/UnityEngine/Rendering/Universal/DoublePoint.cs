using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000B RID: 11
	internal struct DoublePoint
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002ED8 File Offset: 0x000010D8
		public DoublePoint(double x = 0.0, double y = 0.0)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002EE8 File Offset: 0x000010E8
		public DoublePoint(DoublePoint dp)
		{
			this.X = dp.X;
			this.Y = dp.Y;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002F02 File Offset: 0x00001102
		public DoublePoint(IntPoint ip)
		{
			this.X = (double)ip.X;
			this.Y = (double)ip.Y;
		}

		// Token: 0x04000023 RID: 35
		public double X;

		// Token: 0x04000024 RID: 36
		public double Y;
	}
}
