using System;
using Unity.Profiling;
using UnityEngine.Profiling;

namespace UnityEngine.Rendering
{
	// Token: 0x02000078 RID: 120
	[IgnoredByDeepProfiler]
	public class ProfilingSampler
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00010520 File Offset: 0x0000E720
		public static ProfilingSampler Get<TEnum>(TEnum marker) where TEnum : Enum
		{
			TProfilingSampler<TEnum> tprofilingSampler;
			TProfilingSampler<TEnum>.samples.TryGetValue(marker, out tprofilingSampler);
			return tprofilingSampler;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001053C File Offset: 0x0000E73C
		public ProfilingSampler(string name)
		{
			this.sampler = CustomSampler.Create(name, true);
			this.inlineSampler = CustomSampler.Create("Inl_" + name, false);
			this.name = name;
			this.m_Recorder = this.sampler.GetRecorder();
			this.m_Recorder.enabled = false;
			this.m_InlineRecorder = this.inlineSampler.GetRecorder();
			this.m_InlineRecorder.enabled = false;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000105B4 File Offset: 0x0000E7B4
		public void Begin(CommandBuffer cmd)
		{
			if (cmd != null)
			{
				if (this.sampler != null && this.sampler.isValid)
				{
					cmd.BeginSample(this.sampler);
					return;
				}
				cmd.BeginSample(this.name);
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x000105E7 File Offset: 0x0000E7E7
		public void End(CommandBuffer cmd)
		{
			if (cmd != null)
			{
				if (this.sampler != null && this.sampler.isValid)
				{
					cmd.EndSample(this.sampler);
					return;
				}
				cmd.EndSample(this.name);
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001061A File Offset: 0x0000E81A
		internal bool IsValid()
		{
			return this.sampler != null && this.inlineSampler != null;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001062F File Offset: 0x0000E82F
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00010637 File Offset: 0x0000E837
		internal CustomSampler sampler { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00010640 File Offset: 0x0000E840
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00010648 File Offset: 0x0000E848
		internal CustomSampler inlineSampler { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00010651 File Offset: 0x0000E851
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00010659 File Offset: 0x0000E859
		public string name { get; private set; }

		// Token: 0x1700008B RID: 139
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00010662 File Offset: 0x0000E862
		public bool enableRecording
		{
			set
			{
				this.m_Recorder.enabled = value;
				this.m_InlineRecorder.enabled = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001067C File Offset: 0x0000E87C
		public float gpuElapsedTime
		{
			get
			{
				if (!this.m_Recorder.enabled)
				{
					return 0f;
				}
				return (float)this.m_Recorder.gpuElapsedNanoseconds / 1000000f;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x000106A3 File Offset: 0x0000E8A3
		public int gpuSampleCount
		{
			get
			{
				if (!this.m_Recorder.enabled)
				{
					return 0;
				}
				return this.m_Recorder.gpuSampleBlockCount;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000106BF File Offset: 0x0000E8BF
		public float cpuElapsedTime
		{
			get
			{
				if (!this.m_Recorder.enabled)
				{
					return 0f;
				}
				return (float)this.m_Recorder.elapsedNanoseconds / 1000000f;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000106E6 File Offset: 0x0000E8E6
		public int cpuSampleCount
		{
			get
			{
				if (!this.m_Recorder.enabled)
				{
					return 0;
				}
				return this.m_Recorder.sampleBlockCount;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00010702 File Offset: 0x0000E902
		public float inlineCpuElapsedTime
		{
			get
			{
				if (!this.m_InlineRecorder.enabled)
				{
					return 0f;
				}
				return (float)this.m_InlineRecorder.elapsedNanoseconds / 1000000f;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00010729 File Offset: 0x0000E929
		public int inlineCpuSampleCount
		{
			get
			{
				if (!this.m_InlineRecorder.enabled)
				{
					return 0;
				}
				return this.m_InlineRecorder.sampleBlockCount;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00010745 File Offset: 0x0000E945
		private ProfilingSampler()
		{
		}

		// Token: 0x04000213 RID: 531
		private Recorder m_Recorder;

		// Token: 0x04000214 RID: 532
		private Recorder m_InlineRecorder;
	}
}
