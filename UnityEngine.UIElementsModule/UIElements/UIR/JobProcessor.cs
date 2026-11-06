using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine.Bindings;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000425 RID: 1061
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/Renderer/UIRendererJobProcessor.h")]
	internal static class JobProcessor
	{
		// Token: 0x060021D6 RID: 8662 RVA: 0x0007F8A4 File Offset: 0x0007DAA4
		internal static JobHandle ScheduleNudgeJobs(IntPtr buffer, int jobCount)
		{
			JobHandle jobHandle;
			JobProcessor.ScheduleNudgeJobs_Injected(buffer, jobCount, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0007F8BC File Offset: 0x0007DABC
		internal static JobHandle ScheduleConvertMeshJobs(IntPtr buffer, int jobCount)
		{
			JobHandle jobHandle;
			JobProcessor.ScheduleConvertMeshJobs_Injected(buffer, jobCount, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x0007F8D4 File Offset: 0x0007DAD4
		internal static JobHandle ScheduleCopyClosingMeshJobs(IntPtr buffer, int jobCount)
		{
			JobHandle jobHandle;
			JobProcessor.ScheduleCopyClosingMeshJobs_Injected(buffer, jobCount, out jobHandle);
			return jobHandle;
		}

		// Token: 0x060021D9 RID: 8665
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleNudgeJobs_Injected(IntPtr buffer, int jobCount, out JobHandle ret);

		// Token: 0x060021DA RID: 8666
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleConvertMeshJobs_Injected(IntPtr buffer, int jobCount, out JobHandle ret);

		// Token: 0x060021DB RID: 8667
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleCopyClosingMeshJobs_Injected(IntPtr buffer, int jobCount, out JobHandle ret);
	}
}
