using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000408 RID: 1032
	public interface IVisualElementScheduledItem
	{
		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002102 RID: 8450
		VisualElement element { get; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002103 RID: 8451
		bool isActive { get; }

		// Token: 0x06002104 RID: 8452
		void Resume();

		// Token: 0x06002105 RID: 8453
		void Pause();

		// Token: 0x06002106 RID: 8454
		void ExecuteLater(long delayMs);

		// Token: 0x06002107 RID: 8455
		IVisualElementScheduledItem StartingIn(long delayMs);

		// Token: 0x06002108 RID: 8456
		IVisualElementScheduledItem Every(long intervalMs);

		// Token: 0x06002109 RID: 8457
		IVisualElementScheduledItem Until(Func<bool> stopCondition);

		// Token: 0x0600210A RID: 8458
		IVisualElementScheduledItem ForDuration(long durationMs);
	}
}
