using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000070 RID: 112
	public interface IDebugDisplaySettingsData : IDebugDisplaySettingsQuery
	{
		// Token: 0x06000392 RID: 914
		IDebugDisplaySettingsPanelDisposable CreatePanel();
	}
}
