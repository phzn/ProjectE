using System;
using System.Collections.Generic;

namespace Features.Workshop
{
	// Token: 0x02000B40 RID: 2880
	public sealed class WorkshopPage
	{
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x060051C7 RID: 20935 RVA: 0x001FB39F File Offset: 0x001F959F
		public WorkshopScope Scope { get; }

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x060051C8 RID: 20936 RVA: 0x001FB3A7 File Offset: 0x001F95A7
		public int Page { get; }

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x060051C9 RID: 20937 RVA: 0x001FB3AF File Offset: 0x001F95AF
		public int TotalCount { get; }

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060051CA RID: 20938 RVA: 0x001FB3B7 File Offset: 0x001F95B7
		public bool HasNextPage
		{
			get
			{
				return this.State == WorkshopPageState.Ready && ((long)this.Page + 1L) * 50L < (long)this.TotalCount;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060051CB RID: 20939 RVA: 0x001FB3DB File Offset: 0x001F95DB
		public IReadOnlyList<WorkshopItem> Items { get; }

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x060051CC RID: 20940 RVA: 0x001FB3E3 File Offset: 0x001F95E3
		public WorkshopPageState State { get; }

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x060051CD RID: 20941 RVA: 0x001FB3EB File Offset: 0x001F95EB
		public WorkshopErrorCode ErrorCode { get; }

		// Token: 0x060051CE RID: 20942 RVA: 0x001FB3F4 File Offset: 0x001F95F4
		public WorkshopPage(WorkshopScope scope, int page, WorkshopPageState state, IReadOnlyList<WorkshopItem> items = null, int totalCount = 0, WorkshopErrorCode errorCode = WorkshopErrorCode.None)
		{
			this.Scope = scope;
			this.Page = Math.Max(0, page);
			this.State = state;
			this.Items = items ?? Array.Empty<WorkshopItem>();
			this.TotalCount = Math.Max(0, totalCount);
			this.ErrorCode = errorCode;
		}
	}
}
