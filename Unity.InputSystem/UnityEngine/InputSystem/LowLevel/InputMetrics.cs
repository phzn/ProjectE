using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F2 RID: 242
	[Serializable]
	public struct InputMetrics
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00047481 File Offset: 0x00045681
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x00047489 File Offset: 0x00045689
		public int maxNumDevices { readonly get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00047492 File Offset: 0x00045692
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x0004749A File Offset: 0x0004569A
		public int currentNumDevices { readonly get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x000474A3 File Offset: 0x000456A3
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x000474AB File Offset: 0x000456AB
		public int maxStateSizeInBytes { readonly get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x000474B4 File Offset: 0x000456B4
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x000474BC File Offset: 0x000456BC
		public int currentStateSizeInBytes { readonly get; set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x000474C5 File Offset: 0x000456C5
		// (set) Token: 0x06000E68 RID: 3688 RVA: 0x000474CD File Offset: 0x000456CD
		public int currentControlCount { readonly get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x000474D6 File Offset: 0x000456D6
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x000474DE File Offset: 0x000456DE
		public int currentLayoutCount { readonly get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x000474E7 File Offset: 0x000456E7
		// (set) Token: 0x06000E6C RID: 3692 RVA: 0x000474EF File Offset: 0x000456EF
		public int totalEventBytes { readonly get; set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x000474F8 File Offset: 0x000456F8
		// (set) Token: 0x06000E6E RID: 3694 RVA: 0x00047500 File Offset: 0x00045700
		public int totalEventCount { readonly get; set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00047509 File Offset: 0x00045709
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x00047511 File Offset: 0x00045711
		public int totalUpdateCount { readonly get; set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0004751A File Offset: 0x0004571A
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x00047522 File Offset: 0x00045722
		public double totalEventProcessingTime { readonly get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0004752B File Offset: 0x0004572B
		// (set) Token: 0x06000E74 RID: 3700 RVA: 0x00047533 File Offset: 0x00045733
		public double totalEventLagTime { readonly get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0004753C File Offset: 0x0004573C
		public float averageEventBytesPerFrame
		{
			get
			{
				return (float)this.totalEventBytes / (float)this.totalUpdateCount;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0004754D File Offset: 0x0004574D
		public double averageProcessingTimePerEvent
		{
			get
			{
				return this.totalEventProcessingTime / (double)this.totalEventCount;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0004755D File Offset: 0x0004575D
		public double averageLagTimePerEvent
		{
			get
			{
				return this.totalEventLagTime / (double)this.totalEventCount;
			}
		}
	}
}
