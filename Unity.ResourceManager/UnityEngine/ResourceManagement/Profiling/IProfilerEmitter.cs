using System;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x02000050 RID: 80
	internal interface IProfilerEmitter
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001E1 RID: 481
		bool IsEnabled { get; }

		// Token: 0x060001E2 RID: 482
		void EmitFrameMetaData(Guid id, int tag, Array data);

		// Token: 0x060001E3 RID: 483
		void InitialiseCallbacks(Action<float> onLateUpdateDelegate);
	}
}
