using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200006D RID: 109
	internal struct FrameTimeSample
	{
		// Token: 0x06000388 RID: 904 RVA: 0x000101C4 File Offset: 0x0000E3C4
		internal FrameTimeSample(float initValue)
		{
			this.FramesPerSecond = initValue;
			this.FullFrameTime = initValue;
			this.MainThreadCPUFrameTime = initValue;
			this.MainThreadCPUPresentWaitTime = initValue;
			this.RenderThreadCPUFrameTime = initValue;
			this.GPUFrameTime = initValue;
		}

		// Token: 0x040001FE RID: 510
		internal float FramesPerSecond;

		// Token: 0x040001FF RID: 511
		internal float FullFrameTime;

		// Token: 0x04000200 RID: 512
		internal float MainThreadCPUFrameTime;

		// Token: 0x04000201 RID: 513
		internal float MainThreadCPUPresentWaitTime;

		// Token: 0x04000202 RID: 514
		internal float RenderThreadCPUFrameTime;

		// Token: 0x04000203 RID: 515
		internal float GPUFrameTime;
	}
}
