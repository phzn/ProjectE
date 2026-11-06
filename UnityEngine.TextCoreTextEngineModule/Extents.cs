using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000011 RID: 17
	internal struct Extents
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x000067A0 File Offset: 0x000049A0
		public Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000067B4 File Offset: 0x000049B4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Min (",
				this.min.x.ToString("f2"),
				", ",
				this.min.y.ToString("f2"),
				")   Max (",
				this.max.x.ToString("f2"),
				", ",
				this.max.y.ToString("f2"),
				")"
			});
		}

		// Token: 0x0400007A RID: 122
		public Vector2 min;

		// Token: 0x0400007B RID: 123
		public Vector2 max;
	}
}
