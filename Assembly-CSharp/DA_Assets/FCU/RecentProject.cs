using System;

namespace DA_Assets.FCU
{
	// Token: 0x02000329 RID: 809
	[Serializable]
	public struct RecentProject
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x0008E226 File Offset: 0x0008C426
		// (set) Token: 0x060015ED RID: 5613 RVA: 0x0008E22E File Offset: 0x0008C42E
		public string Url { readonly get; set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x0008E237 File Offset: 0x0008C437
		// (set) Token: 0x060015EF RID: 5615 RVA: 0x0008E23F File Offset: 0x0008C43F
		public string Name { readonly get; set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x0008E248 File Offset: 0x0008C448
		// (set) Token: 0x060015F1 RID: 5617 RVA: 0x0008E250 File Offset: 0x0008C450
		public DateTime DateTime { readonly get; set; }
	}
}
