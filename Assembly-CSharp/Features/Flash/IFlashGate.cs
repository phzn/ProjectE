using System;

namespace Features.Flash
{
	// Token: 0x0200120B RID: 4619
	public interface IFlashGate
	{
		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06008EA7 RID: 36519
		bool IsEngaged { get; }

		// Token: 0x06008EA8 RID: 36520
		void SetRemoved();

		// Token: 0x06008EA9 RID: 36521
		void SetRestored();
	}
}
