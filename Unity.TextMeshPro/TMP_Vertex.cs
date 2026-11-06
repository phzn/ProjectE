using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000021 RID: 33
	public struct TMP_Vertex
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000174BB File Offset: 0x000156BB
		public static TMP_Vertex zero
		{
			get
			{
				return TMP_Vertex.k_Zero;
			}
		}

		// Token: 0x0400010B RID: 267
		public Vector3 position;

		// Token: 0x0400010C RID: 268
		public Vector2 uv;

		// Token: 0x0400010D RID: 269
		public Vector2 uv2;

		// Token: 0x0400010E RID: 270
		public Vector2 uv4;

		// Token: 0x0400010F RID: 271
		public Color32 color;

		// Token: 0x04000110 RID: 272
		private static readonly TMP_Vertex k_Zero;
	}
}
