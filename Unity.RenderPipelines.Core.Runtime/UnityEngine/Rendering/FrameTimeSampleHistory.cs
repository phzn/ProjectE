using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x0200006E RID: 110
	internal class FrameTimeSampleHistory
	{
		// Token: 0x06000389 RID: 905 RVA: 0x000101F0 File Offset: 0x0000E3F0
		public FrameTimeSampleHistory(int initialCapacity)
		{
			this.m_Samples.Capacity = initialCapacity;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001020F File Offset: 0x0000E40F
		internal void Add(FrameTimeSample sample)
		{
			this.m_Samples.Add(sample);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00010220 File Offset: 0x0000E420
		internal void ComputeAggregateValues()
		{
			FrameTimeSample frameTimeSample = default(FrameTimeSample);
			FrameTimeSample frameTimeSample2 = new FrameTimeSample(float.MaxValue);
			FrameTimeSample frameTimeSample3 = new FrameTimeSample(float.MinValue);
			FrameTimeSample frameTimeSample4 = default(FrameTimeSample);
			for (int i = 0; i < this.m_Samples.Count; i++)
			{
				FrameTimeSample frameTimeSample5 = this.m_Samples[i];
				FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample2, frameTimeSample5, FrameTimeSampleHistory.s_SampleValueMin);
				FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample3, frameTimeSample5, FrameTimeSampleHistory.s_SampleValueMax);
				FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample, frameTimeSample5, FrameTimeSampleHistory.s_SampleValueAdd);
				FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample4, frameTimeSample5, FrameTimeSampleHistory.s_SampleValueCountValid);
			}
			FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample2, frameTimeSample4, FrameTimeSampleHistory.s_SampleValueEnsureValid);
			FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample3, frameTimeSample4, FrameTimeSampleHistory.s_SampleValueEnsureValid);
			FrameTimeSampleHistory.<ComputeAggregateValues>g__ForEachSampleMember|12_0(ref frameTimeSample, frameTimeSample4, FrameTimeSampleHistory.s_SampleValueDivide);
			this.SampleAverage = frameTimeSample;
			this.SampleMin = frameTimeSample2;
			this.SampleMax = frameTimeSample3;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000102F2 File Offset: 0x0000E4F2
		internal void DiscardOldSamples(int sampleHistorySize)
		{
			while (this.m_Samples.Count >= sampleHistorySize)
			{
				this.m_Samples.RemoveAt(0);
			}
			this.m_Samples.Capacity = sampleHistorySize;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001031C File Offset: 0x0000E51C
		internal void Clear()
		{
			this.m_Samples.Clear();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000103B8 File Offset: 0x0000E5B8
		[CompilerGenerated]
		internal static void <ComputeAggregateValues>g__ForEachSampleMember|12_0(ref FrameTimeSample aggregate, FrameTimeSample sample, Func<float, float, float> func)
		{
			aggregate.FramesPerSecond = func(aggregate.FramesPerSecond, sample.FramesPerSecond);
			aggregate.FullFrameTime = func(aggregate.FullFrameTime, sample.FullFrameTime);
			aggregate.MainThreadCPUFrameTime = func(aggregate.MainThreadCPUFrameTime, sample.MainThreadCPUFrameTime);
			aggregate.MainThreadCPUPresentWaitTime = func(aggregate.MainThreadCPUPresentWaitTime, sample.MainThreadCPUPresentWaitTime);
			aggregate.RenderThreadCPUFrameTime = func(aggregate.RenderThreadCPUFrameTime, sample.RenderThreadCPUFrameTime);
			aggregate.GPUFrameTime = func(aggregate.GPUFrameTime, sample.GPUFrameTime);
		}

		// Token: 0x04000204 RID: 516
		private List<FrameTimeSample> m_Samples = new List<FrameTimeSample>();

		// Token: 0x04000205 RID: 517
		internal FrameTimeSample SampleAverage;

		// Token: 0x04000206 RID: 518
		internal FrameTimeSample SampleMin;

		// Token: 0x04000207 RID: 519
		internal FrameTimeSample SampleMax;

		// Token: 0x04000208 RID: 520
		private static Func<float, float, float> s_SampleValueAdd = (float value, float other) => value + other;

		// Token: 0x04000209 RID: 521
		private static Func<float, float, float> s_SampleValueMin = delegate(float value, float other)
		{
			if (other <= 0f)
			{
				return value;
			}
			return Mathf.Min(value, other);
		};

		// Token: 0x0400020A RID: 522
		private static Func<float, float, float> s_SampleValueMax = (float value, float other) => Mathf.Max(value, other);

		// Token: 0x0400020B RID: 523
		private static Func<float, float, float> s_SampleValueCountValid = delegate(float value, float other)
		{
			if (other <= 0f)
			{
				return value;
			}
			return value + 1f;
		};

		// Token: 0x0400020C RID: 524
		private static Func<float, float, float> s_SampleValueEnsureValid = delegate(float value, float other)
		{
			if (other <= 0f)
			{
				return 0f;
			}
			return value;
		};

		// Token: 0x0400020D RID: 525
		private static Func<float, float, float> s_SampleValueDivide = delegate(float value, float other)
		{
			if (other <= 0f)
			{
				return 0f;
			}
			return value / other;
		};
	}
}
