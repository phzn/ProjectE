using System;

namespace Features.CielOS
{
	// Token: 0x02001391 RID: 5009
	public static class CielOSSession
	{
		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x06009B09 RID: 39689 RVA: 0x003F6A6D File Offset: 0x003F4C6D
		// (set) Token: 0x06009B0A RID: 39690 RVA: 0x003F6A74 File Offset: 0x003F4C74
		public static bool ShouldReopenOnReturn { get; private set; }

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x06009B0B RID: 39691 RVA: 0x003F6A7C File Offset: 0x003F4C7C
		// (set) Token: 0x06009B0C RID: 39692 RVA: 0x003F6A83 File Offset: 0x003F4C83
		public static string LastLaunchedAppId { get; private set; }

		// Token: 0x06009B0D RID: 39693 RVA: 0x003F6A8B File Offset: 0x003F4C8B
		public static void MarkOpenBeforeSceneChange(string launchedAppId)
		{
			CielOSSession.ShouldReopenOnReturn = true;
			CielOSSession.LastLaunchedAppId = launchedAppId ?? string.Empty;
		}

		// Token: 0x06009B0E RID: 39694 RVA: 0x003F6AA2 File Offset: 0x003F4CA2
		public static bool ConsumeReopenRequest()
		{
			bool shouldReopenOnReturn = CielOSSession.ShouldReopenOnReturn;
			CielOSSession.ShouldReopenOnReturn = false;
			return shouldReopenOnReturn;
		}

		// Token: 0x06009B0F RID: 39695 RVA: 0x003F6AAF File Offset: 0x003F4CAF
		public static void Clear()
		{
			CielOSSession.ShouldReopenOnReturn = false;
			CielOSSession.LastLaunchedAppId = string.Empty;
		}
	}
}
