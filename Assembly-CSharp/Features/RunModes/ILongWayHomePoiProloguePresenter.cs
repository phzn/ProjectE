using System;

namespace Features.RunModes
{
	// Token: 0x02000CD9 RID: 3289
	public interface ILongWayHomePoiProloguePresenter
	{
		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06005F9C RID: 24476
		bool OwnsDestinationReveal { get; }

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06005F9D RID: 24477
		bool IsPreparedForReveal { get; }

		// Token: 0x06005F9E RID: 24478
		void FailPreparation(string reason);

		// Token: 0x06005F9F RID: 24479
		void PrepareForSceneExit();
	}
}
