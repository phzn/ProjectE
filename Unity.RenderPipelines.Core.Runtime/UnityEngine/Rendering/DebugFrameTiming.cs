using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering
{
	// Token: 0x02000060 RID: 96
	public class DebugFrameTiming
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000D00D File Offset: 0x0000B20D
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000D015 File Offset: 0x0000B215
		public int bottleneckHistorySize { get; set; } = 60;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000D01E File Offset: 0x0000B21E
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000D026 File Offset: 0x0000B226
		public int sampleHistorySize { get; set; } = 30;

		// Token: 0x06000309 RID: 777 RVA: 0x0000D030 File Offset: 0x0000B230
		public DebugFrameTiming()
		{
			this.m_FrameHistory = new FrameTimeSampleHistory(this.sampleHistorySize);
			this.m_BottleneckHistory = new BottleneckHistory(this.bottleneckHistorySize);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D084 File Offset: 0x0000B284
		public void UpdateFrameTiming()
		{
			this.m_Timing[0] = default(FrameTiming);
			this.m_Sample = default(FrameTimeSample);
			FrameTimingManager.CaptureFrameTimings();
			FrameTimingManager.GetLatestTimings(1U, this.m_Timing);
			if (this.m_Timing.Length != 0)
			{
				this.m_Sample.FullFrameTime = (float)this.m_Timing.First<FrameTiming>().cpuFrameTime;
				this.m_Sample.FramesPerSecond = ((this.m_Sample.FullFrameTime > 0f) ? (1000f / this.m_Sample.FullFrameTime) : 0f);
				this.m_Sample.MainThreadCPUFrameTime = (float)this.m_Timing.First<FrameTiming>().cpuMainThreadFrameTime;
				this.m_Sample.MainThreadCPUPresentWaitTime = (float)this.m_Timing.First<FrameTiming>().cpuMainThreadPresentWaitTime;
				this.m_Sample.RenderThreadCPUFrameTime = (float)this.m_Timing.First<FrameTiming>().cpuRenderThreadFrameTime;
				this.m_Sample.GPUFrameTime = (float)this.m_Timing.First<FrameTiming>().gpuFrameTime;
			}
			this.m_FrameHistory.DiscardOldSamples(this.sampleHistorySize);
			this.m_FrameHistory.Add(this.m_Sample);
			this.m_FrameHistory.ComputeAggregateValues();
			this.m_BottleneckHistory.DiscardOldSamples(this.bottleneckHistorySize);
			this.m_BottleneckHistory.AddBottleneckFromAveragedSample(this.m_FrameHistory.SampleAverage);
			this.m_BottleneckHistory.ComputeHistogram();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public void RegisterDebugUI(List<DebugUI.Widget> list)
		{
			list.Add(new DebugUI.Foldout
			{
				displayName = "Frame Stats",
				opened = true,
				columnLabels = new string[] { "Avg", "Min", "Max" },
				children = 
				{
					new DebugUI.ValueTuple
					{
						displayName = "Frame Rate (FPS)",
						values = new DebugUI.Value[]
						{
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F1}",
								getter = () => this.m_FrameHistory.SampleAverage.FramesPerSecond
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F1}",
								getter = () => this.m_FrameHistory.SampleMin.FramesPerSecond
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F1}",
								getter = () => this.m_FrameHistory.SampleMax.FramesPerSecond
							}
						}
					},
					new DebugUI.ValueTuple
					{
						displayName = "Frame Time",
						values = new DebugUI.Value[]
						{
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleAverage.FullFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMin.FullFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMax.FullFrameTime
							}
						}
					},
					new DebugUI.ValueTuple
					{
						displayName = "CPU Main Thread Frame",
						values = new DebugUI.Value[]
						{
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleAverage.MainThreadCPUFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMin.MainThreadCPUFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMax.MainThreadCPUFrameTime
							}
						}
					},
					new DebugUI.ValueTuple
					{
						displayName = "CPU Render Thread Frame",
						values = new DebugUI.Value[]
						{
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleAverage.RenderThreadCPUFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMin.RenderThreadCPUFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMax.RenderThreadCPUFrameTime
							}
						}
					},
					new DebugUI.ValueTuple
					{
						displayName = "CPU Present Wait",
						values = new DebugUI.Value[]
						{
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleAverage.MainThreadCPUPresentWaitTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMin.MainThreadCPUPresentWaitTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMax.MainThreadCPUPresentWaitTime
							}
						}
					},
					new DebugUI.ValueTuple
					{
						displayName = "GPU Frame",
						values = new DebugUI.Value[]
						{
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleAverage.GPUFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMin.GPUFrameTime
							},
							new DebugUI.Value
							{
								refreshRate = 0.2f,
								formatString = "{0:F2}ms",
								getter = () => this.m_FrameHistory.SampleMax.GPUFrameTime
							}
						}
					}
				}
			});
			list.Add(new DebugUI.Foldout
			{
				displayName = "Bottlenecks",
				children = 
				{
					new DebugUI.ProgressBarValue
					{
						displayName = "CPU",
						getter = () => this.m_BottleneckHistory.Histogram.CPU
					},
					new DebugUI.ProgressBarValue
					{
						displayName = "GPU",
						getter = () => this.m_BottleneckHistory.Histogram.GPU
					},
					new DebugUI.ProgressBarValue
					{
						displayName = "Present limited",
						getter = () => this.m_BottleneckHistory.Histogram.PresentLimited
					},
					new DebugUI.ProgressBarValue
					{
						displayName = "Balanced",
						getter = () => this.m_BottleneckHistory.Histogram.Balanced
					}
				}
			});
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000D760 File Offset: 0x0000B960
		internal void Reset()
		{
			this.m_BottleneckHistory.Clear();
			this.m_FrameHistory.Clear();
		}

		// Token: 0x040001AC RID: 428
		private const string k_FpsFormatString = "{0:F1}";

		// Token: 0x040001AD RID: 429
		private const string k_MsFormatString = "{0:F2}ms";

		// Token: 0x040001AE RID: 430
		private const float k_RefreshRate = 0.2f;

		// Token: 0x040001AF RID: 431
		internal FrameTimeSampleHistory m_FrameHistory;

		// Token: 0x040001B0 RID: 432
		internal BottleneckHistory m_BottleneckHistory;

		// Token: 0x040001B3 RID: 435
		private FrameTiming[] m_Timing = new FrameTiming[1];

		// Token: 0x040001B4 RID: 436
		private FrameTimeSample m_Sample;
	}
}
