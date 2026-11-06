using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000010 RID: 16
	internal struct IntRect
	{
		// Token: 0x06000070 RID: 112 RVA: 0x0000354B File Offset: 0x0000174B
		public IntRect(long l, long t, long r, long b)
		{
			this.left = l;
			this.top = t;
			this.right = r;
			this.bottom = b;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000356A File Offset: 0x0000176A
		public IntRect(IntRect ir)
		{
			this.left = ir.left;
			this.top = ir.top;
			this.right = ir.right;
			this.bottom = ir.bottom;
		}

		// Token: 0x04000035 RID: 53
		public long left;

		// Token: 0x04000036 RID: 54
		public long top;

		// Token: 0x04000037 RID: 55
		public long right;

		// Token: 0x04000038 RID: 56
		public long bottom;
	}
}
