using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public struct Mesh_Extents
	{
		// Token: 0x06000111 RID: 273 RVA: 0x0001731F File Offset: 0x0001551F
		public Mesh_Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00017330 File Offset: 0x00015530
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

		// Token: 0x040000BB RID: 187
		public Vector2 min;

		// Token: 0x040000BC RID: 188
		public Vector2 max;
	}
}
