using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000195 RID: 405
	public class MMLoot<T>
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0002C9CB File Offset: 0x0002ABCB
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0002C9D3 File Offset: 0x0002ABD3
		public virtual float RangeFrom { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0002C9DC File Offset: 0x0002ABDC
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
		public virtual float RangeTo { get; set; }

		// Token: 0x04000B73 RID: 2931
		public T Loot;

		// Token: 0x04000B74 RID: 2932
		public float Weight = 1f;

		// Token: 0x04000B75 RID: 2933
		[MMReadOnly]
		public float ChancePercentage;
	}
}
