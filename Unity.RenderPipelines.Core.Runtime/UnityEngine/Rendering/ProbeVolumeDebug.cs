using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200008D RID: 141
	internal class ProbeVolumeDebug : IDebugData
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x00017690 File Offset: 0x00015890
		public ProbeVolumeDebug()
		{
			this.Init();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000176F0 File Offset: 0x000158F0
		private void Init()
		{
			this.drawProbes = false;
			this.drawBricks = false;
			this.drawCells = false;
			this.realtimeSubdivision = false;
			this.subdivisionCellUpdatePerFrame = 4;
			this.subdivisionDelayInSeconds = 1f;
			this.probeShading = DebugProbeShadingMode.SH;
			this.probeSize = 0.3f;
			this.subdivisionViewCullingDistance = 500f;
			this.probeCullingDistance = 200f;
			this.maxSubdivToVisualize = 7;
			this.minSubdivToVisualize = 0;
			this.exposureCompensation = 0f;
			this.drawVirtualOffsetPush = false;
			this.offsetSize = 0.025f;
			this.freezeStreaming = false;
			this.otherStateIndex = 0;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001778C File Offset: 0x0001598C
		public Action GetReset()
		{
			return delegate
			{
				this.Init();
			};
		}

		// Token: 0x040002F1 RID: 753
		public bool drawProbes;

		// Token: 0x040002F2 RID: 754
		public bool drawBricks;

		// Token: 0x040002F3 RID: 755
		public bool drawCells;

		// Token: 0x040002F4 RID: 756
		public bool realtimeSubdivision;

		// Token: 0x040002F5 RID: 757
		public int subdivisionCellUpdatePerFrame = 4;

		// Token: 0x040002F6 RID: 758
		public float subdivisionDelayInSeconds = 1f;

		// Token: 0x040002F7 RID: 759
		public DebugProbeShadingMode probeShading;

		// Token: 0x040002F8 RID: 760
		public float probeSize = 0.3f;

		// Token: 0x040002F9 RID: 761
		public float subdivisionViewCullingDistance = 500f;

		// Token: 0x040002FA RID: 762
		public float probeCullingDistance = 200f;

		// Token: 0x040002FB RID: 763
		public int maxSubdivToVisualize = 7;

		// Token: 0x040002FC RID: 764
		public int minSubdivToVisualize;

		// Token: 0x040002FD RID: 765
		public float exposureCompensation;

		// Token: 0x040002FE RID: 766
		public bool drawVirtualOffsetPush;

		// Token: 0x040002FF RID: 767
		public float offsetSize = 0.025f;

		// Token: 0x04000300 RID: 768
		public bool freezeStreaming;

		// Token: 0x04000301 RID: 769
		public int otherStateIndex;
	}
}
