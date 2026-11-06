using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200001A RID: 26
	public struct Extents
	{
		// Token: 0x0600010E RID: 270 RVA: 0x0001722D File Offset: 0x0001542D
		public Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00017240 File Offset: 0x00015440
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

		// Token: 0x040000B7 RID: 183
		internal static Extents zero = new Extents(Vector2.zero, Vector2.zero);

		// Token: 0x040000B8 RID: 184
		internal static Extents uninitialized = new Extents(new Vector2(32767f, 32767f), new Vector2(-32767f, -32767f));

		// Token: 0x040000B9 RID: 185
		public Vector2 min;

		// Token: 0x040000BA RID: 186
		public Vector2 max;
	}
}
