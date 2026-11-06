using System;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x0200041C RID: 1052
	internal interface IVisualTreeUpdater : IDisposable
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002182 RID: 8578
		// (set) Token: 0x06002183 RID: 8579
		BaseVisualElementPanel panel { get; set; }

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002184 RID: 8580
		ProfilerMarker profilerMarker { get; }

		// Token: 0x06002185 RID: 8581
		void Update();

		// Token: 0x06002186 RID: 8582
		void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType);
	}
}
