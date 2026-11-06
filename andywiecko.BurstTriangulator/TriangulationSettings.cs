using System;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class TriangulationSettings
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000248E File Offset: 0x0000068E
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002496 File Offset: 0x00000696
		public bool AutoHolesAndBoundary { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000249F File Offset: 0x0000069F
		public RefinementThresholds RefinementThresholds { get; } = new RefinementThresholds();

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024A7 File Offset: 0x000006A7
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000024AF File Offset: 0x000006AF
		public bool RefineMesh { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000024B8 File Offset: 0x000006B8
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000024C0 File Offset: 0x000006C0
		public bool ValidateInput { get; set; } = true;

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024C9 File Offset: 0x000006C9
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000024D1 File Offset: 0x000006D1
		public bool Verbose { get; set; } = true;

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000024DA File Offset: 0x000006DA
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000024E2 File Offset: 0x000006E2
		public bool RestoreBoundary { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000024EB File Offset: 0x000006EB
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000024F3 File Offset: 0x000006F3
		public int SloanMaxIters { get; set; } = 1000000;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000024FC File Offset: 0x000006FC
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002504 File Offset: 0x00000704
		public Preprocessor Preprocessor { get; set; }
	}
}
