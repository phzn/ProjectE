using System;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Editor
{
	// Token: 0x020000AF RID: 175
	internal struct SampleFrequencyCalculator
	{
		// Token: 0x06000CE5 RID: 3301 RVA: 0x00043ECE File Offset: 0x000420CE
		public SampleFrequencyCalculator(float targetFrequency, double realtimeSinceStartup)
		{
			this.targetFrequency = targetFrequency;
			this.m_SampleCount = 0;
			this.frequency = 0f;
			this.m_LastUpdateTime = realtimeSinceStartup;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00043EF0 File Offset: 0x000420F0
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x00043EF8 File Offset: 0x000420F8
		public float targetFrequency { readonly get; private set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00043F01 File Offset: 0x00042101
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x00043F09 File Offset: 0x00042109
		public float frequency { readonly get; private set; }

		// Token: 0x06000CEA RID: 3306 RVA: 0x00043F12 File Offset: 0x00042112
		public void ProcessSample(InputEventPtr eventPtr)
		{
			if (eventPtr != null)
			{
				this.m_SampleCount++;
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00043F31 File Offset: 0x00042131
		public bool Update()
		{
			return this.Update(Time.realtimeSinceStartupAsDouble);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00043F40 File Offset: 0x00042140
		public bool Update(double realtimeSinceStartup)
		{
			double num = realtimeSinceStartup - this.m_LastUpdateTime;
			if (num < 1.0)
			{
				return false;
			}
			this.m_LastUpdateTime = realtimeSinceStartup;
			this.frequency = (float)((double)this.m_SampleCount / num);
			this.m_SampleCount = 0;
			return true;
		}

		// Token: 0x040004F0 RID: 1264
		private double m_LastUpdateTime;

		// Token: 0x040004F1 RID: 1265
		private int m_SampleCount;
	}
}
