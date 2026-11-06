using System;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x0200004A RID: 74
	public class EngineEmitter : IProfilerEmitter
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00007171 File Offset: 0x00005371
		public bool IsEnabled
		{
			get
			{
				return Profiler.enabled;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007178 File Offset: 0x00005378
		public void EmitFrameMetaData(Guid id, int tag, Array data)
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000717A File Offset: 0x0000537A
		public void InitialiseCallbacks(Action<float> d)
		{
			ComponentSingleton<MonoBehaviourCallbackHooks>.Instance.OnLateUpdateDelegate += d;
		}
	}
}
