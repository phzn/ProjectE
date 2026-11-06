using System;

namespace Features.Detection
{
	// Token: 0x020012B8 RID: 4792
	public interface IDetectionUISource
	{
		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x0600949D RID: 38045
		bool VisualActive { get; }

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x0600949E RID: 38046
		bool AudioActive { get; }

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x0600949F RID: 38047
		float DetectionLevel { get; }
	}
}
