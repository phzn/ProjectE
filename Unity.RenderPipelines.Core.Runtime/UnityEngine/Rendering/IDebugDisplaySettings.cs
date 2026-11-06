using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200006F RID: 111
	public interface IDebugDisplaySettings : IDebugDisplaySettingsQuery
	{
		// Token: 0x06000390 RID: 912
		void Reset();

		// Token: 0x06000391 RID: 913
		void ForEach(Action<IDebugDisplaySettingsData> onExecute);
	}
}
