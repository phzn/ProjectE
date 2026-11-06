using System;

namespace Features.Workshop
{
	// Token: 0x02000B3F RID: 2879
	public sealed class WorkshopItem
	{
		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x060051C1 RID: 20929 RVA: 0x001FB327 File Offset: 0x001F9527
		public ulong Id { get; }

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060051C2 RID: 20930 RVA: 0x001FB32F File Offset: 0x001F952F
		public string Title { get; }

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060051C3 RID: 20931 RVA: 0x001FB337 File Offset: 0x001F9537
		public string Description { get; }

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x060051C4 RID: 20932 RVA: 0x001FB33F File Offset: 0x001F953F
		public string PreviewUrl { get; }

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x060051C5 RID: 20933 RVA: 0x001FB347 File Offset: 0x001F9547
		public ulong OwnerId { get; }

		// Token: 0x060051C6 RID: 20934 RVA: 0x001FB350 File Offset: 0x001F9550
		public WorkshopItem(ulong id, string title, string description, string previewUrl, ulong ownerId = 0UL)
		{
			this.Id = id;
			this.Title = title ?? string.Empty;
			this.Description = description ?? string.Empty;
			this.PreviewUrl = WorkshopRules.SafePreviewUrl(previewUrl);
			this.OwnerId = ownerId;
		}
	}
}
