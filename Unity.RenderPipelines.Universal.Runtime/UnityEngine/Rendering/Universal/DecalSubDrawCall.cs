using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200006A RID: 106
	internal struct DecalSubDrawCall
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x000169E9 File Offset: 0x00014BE9
		public int count
		{
			get
			{
				return this.end - this.start;
			}
		}

		// Token: 0x040002EF RID: 751
		public int start;

		// Token: 0x040002F0 RID: 752
		public int end;
	}
}
