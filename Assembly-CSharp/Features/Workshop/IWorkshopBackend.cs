using System;

namespace Features.Workshop
{
	// Token: 0x02000B3E RID: 2878
	public interface IWorkshopBackend
	{
		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x060051BC RID: 20924
		bool IsAvailable { get; }

		// Token: 0x060051BD RID: 20925
		IDisposable RequestPage(WorkshopScope scope, int page, string search, Action<WorkshopPage> callback);

		// Token: 0x060051BE RID: 20926
		IDisposable SetSubscribed(ulong id, bool subscribed, Action<WorkshopOperationResult> callback);

		// Token: 0x060051BF RID: 20927
		WorkshopInstallState GetInstallState(ulong id);

		// Token: 0x060051C0 RID: 20928
		bool Download(ulong id);
	}
}
