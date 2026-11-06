using System;

namespace Features.Achievements
{
	// Token: 0x0200153B RID: 5435
	public interface IAchievementBackend
	{
		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x0600A83E RID: 43070
		string PlatformName { get; }

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x0600A83F RID: 43071
		bool IsAvailable { get; }

		// Token: 0x0600A840 RID: 43072
		bool TryUnlock(string apiName);
	}
}
