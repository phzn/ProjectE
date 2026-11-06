using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000073 RID: 115
	public interface IDebugDisplaySettingsQuery
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000396 RID: 918
		bool AreAnySettingsActive { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000397 RID: 919
		bool IsPostProcessingAllowed { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000398 RID: 920
		bool IsLightingActive { get; }

		// Token: 0x06000399 RID: 921
		bool TryGetScreenClearColor(ref Color color);
	}
}
