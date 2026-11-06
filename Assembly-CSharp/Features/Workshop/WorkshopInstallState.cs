using System;

namespace Features.Workshop
{
	// Token: 0x02000B43 RID: 2883
	public readonly struct WorkshopInstallState
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x001FB4B3 File Offset: 0x001F96B3
		public bool IsSubscribed { get; }

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x001FB4BB File Offset: 0x001F96BB
		public bool IsInstalled { get; }

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x001FB4C3 File Offset: 0x001F96C3
		public bool IsDownloading { get; }

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x001FB4CB File Offset: 0x001F96CB
		public bool IsDownloadPending { get; }

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x060051DB RID: 20955 RVA: 0x001FB4D3 File Offset: 0x001F96D3
		public bool NeedsUpdate { get; }

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x001FB4DB File Offset: 0x001F96DB
		public float DownloadProgress { get; }

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x060051DD RID: 20957 RVA: 0x001FB4E3 File Offset: 0x001F96E3
		public WorkshopErrorCode ErrorCode { get; }

		// Token: 0x060051DE RID: 20958 RVA: 0x001FB4EC File Offset: 0x001F96EC
		public WorkshopInstallState(bool subscribed, bool installed, bool downloading, bool pending, bool needsUpdate, ulong downloaded = 0UL, ulong total = 0UL, WorkshopErrorCode errorCode = WorkshopErrorCode.None)
		{
			this.IsSubscribed = subscribed;
			this.IsInstalled = installed;
			this.IsDownloading = downloading;
			this.IsDownloadPending = pending;
			this.NeedsUpdate = needsUpdate;
			this.DownloadProgress = ((total == 0UL) ? 0f : ((float)Math.Min(1.0, downloaded / total)));
			this.ErrorCode = errorCode;
		}
	}
}
