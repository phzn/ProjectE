using System;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x02000035 RID: 53
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[BurstRuntime.PreserveAttribute]
	internal sealed class BurstTargetCpuAttribute : Attribute
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x00006701 File Offset: 0x00004901
		public BurstTargetCpuAttribute(BurstTargetCpu TargetCpu)
		{
			this.TargetCpu = TargetCpu;
		}

		// Token: 0x04000180 RID: 384
		public readonly BurstTargetCpu TargetCpu;
	}
}
