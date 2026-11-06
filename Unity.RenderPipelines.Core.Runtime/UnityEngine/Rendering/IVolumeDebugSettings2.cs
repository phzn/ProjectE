using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000075 RID: 117
	public interface IVolumeDebugSettings2 : IVolumeDebugSettings
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003A9 RID: 937
		Type targetRenderPipeline { get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003AA RID: 938
		List<ValueTuple<string, Type>> volumeComponentsPathAndType { get; }
	}
}
