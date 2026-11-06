using System;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200000B RID: 11
	[IgnoredByDeepProfiler]
	[NativeHeader("Modules/UI/Canvas.h")]
	[StaticAccessor("UI::SystemProfilerApi", StaticAccessorType.DoubleColon)]
	public static class UISystemProfilerApi
	{
		// Token: 0x06000099 RID: 153
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginSample(UISystemProfilerApi.SampleType type);

		// Token: 0x0600009A RID: 154
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndSample(UISystemProfilerApi.SampleType type);

		// Token: 0x0600009B RID: 155
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void AddMarker(string name, Object obj);

		// Token: 0x0200000C RID: 12
		public enum SampleType
		{
			// Token: 0x04000017 RID: 23
			Layout,
			// Token: 0x04000018 RID: 24
			Render
		}
	}
}
