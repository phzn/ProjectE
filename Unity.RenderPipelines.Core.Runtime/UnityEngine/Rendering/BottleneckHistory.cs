using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x0200006C RID: 108
	internal class BottleneckHistory
	{
		// Token: 0x06000382 RID: 898 RVA: 0x0000FFAA File Offset: 0x0000E1AA
		public BottleneckHistory(int initialCapacity)
		{
			this.m_Bottlenecks.Capacity = initialCapacity;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000FFC9 File Offset: 0x0000E1C9
		internal void DiscardOldSamples(int historySize)
		{
			while (this.m_Bottlenecks.Count >= historySize)
			{
				this.m_Bottlenecks.RemoveAt(0);
			}
			this.m_Bottlenecks.Capacity = historySize;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000FFF4 File Offset: 0x0000E1F4
		internal void AddBottleneckFromAveragedSample(FrameTimeSample frameHistorySampleAverage)
		{
			PerformanceBottleneck performanceBottleneck = BottleneckHistory.DetermineBottleneck(frameHistorySampleAverage);
			this.m_Bottlenecks.Add(performanceBottleneck);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00010014 File Offset: 0x0000E214
		internal void ComputeHistogram()
		{
			BottleneckHistogram bottleneckHistogram = default(BottleneckHistogram);
			for (int i = 0; i < this.m_Bottlenecks.Count; i++)
			{
				switch (this.m_Bottlenecks[i])
				{
				case PerformanceBottleneck.PresentLimited:
					bottleneckHistogram.PresentLimited += 1f;
					break;
				case PerformanceBottleneck.CPU:
					bottleneckHistogram.CPU += 1f;
					break;
				case PerformanceBottleneck.GPU:
					bottleneckHistogram.GPU += 1f;
					break;
				case PerformanceBottleneck.Balanced:
					bottleneckHistogram.Balanced += 1f;
					break;
				}
			}
			bottleneckHistogram.Balanced /= (float)this.m_Bottlenecks.Count;
			bottleneckHistogram.CPU /= (float)this.m_Bottlenecks.Count;
			bottleneckHistogram.GPU /= (float)this.m_Bottlenecks.Count;
			bottleneckHistogram.PresentLimited /= (float)this.m_Bottlenecks.Count;
			this.Histogram = bottleneckHistogram;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00010110 File Offset: 0x0000E310
		private static PerformanceBottleneck DetermineBottleneck(FrameTimeSample s)
		{
			if (s.GPUFrameTime == 0f || s.MainThreadCPUFrameTime == 0f)
			{
				return PerformanceBottleneck.Indeterminate;
			}
			float num = 0.8f * s.FullFrameTime;
			if (s.GPUFrameTime > num && s.MainThreadCPUFrameTime < num && s.RenderThreadCPUFrameTime < num)
			{
				return PerformanceBottleneck.GPU;
			}
			if (s.GPUFrameTime < num && (s.MainThreadCPUFrameTime > num || s.RenderThreadCPUFrameTime > num))
			{
				return PerformanceBottleneck.CPU;
			}
			if (s.MainThreadCPUPresentWaitTime > 0.5f && s.GPUFrameTime < num && s.MainThreadCPUFrameTime < num && s.RenderThreadCPUFrameTime < num)
			{
				return PerformanceBottleneck.PresentLimited;
			}
			return PerformanceBottleneck.Balanced;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000101AB File Offset: 0x0000E3AB
		internal void Clear()
		{
			this.m_Bottlenecks.Clear();
			this.Histogram = default(BottleneckHistogram);
		}

		// Token: 0x040001FC RID: 508
		private List<PerformanceBottleneck> m_Bottlenecks = new List<PerformanceBottleneck>();

		// Token: 0x040001FD RID: 509
		internal BottleneckHistogram Histogram;
	}
}
