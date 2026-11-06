using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	internal struct MeshExtents
	{
		// Token: 0x06000161 RID: 353 RVA: 0x0001D6A3 File Offset: 0x0001B8A3
		public MeshExtents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001D6B4 File Offset: 0x0001B8B4
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

		// Token: 0x0400024D RID: 589
		public Vector2 min;

		// Token: 0x0400024E RID: 590
		public Vector2 max;
	}
}
