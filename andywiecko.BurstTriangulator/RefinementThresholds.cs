using System;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class RefinementThresholds
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002449 File Offset: 0x00000649
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002451 File Offset: 0x00000651
		public float Area { get; set; } = 1f;

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000245A File Offset: 0x0000065A
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002462 File Offset: 0x00000662
		public float Angle { get; set; } = math.radians(5f);
	}
}
