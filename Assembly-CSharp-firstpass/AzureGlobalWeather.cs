using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public sealed class AzureGlobalWeather
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003033 File Offset: 0x00001233
		// (set) Token: 0x06000071 RID: 113 RVA: 0x0000303B File Offset: 0x0000123B
		public AzureWeatherPreset weatherPreset
		{
			get
			{
				return this.m_weatherPreset;
			}
			set
			{
				this.m_weatherPreset = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003044 File Offset: 0x00001244
		// (set) Token: 0x06000073 RID: 115 RVA: 0x0000304C File Offset: 0x0000124C
		public float transitionTime
		{
			get
			{
				return this.m_transitionTime;
			}
			set
			{
				this.m_transitionTime = value;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003055 File Offset: 0x00001255
		public AzureGlobalWeather(float transitionTime)
		{
			this.m_transitionTime = transitionTime;
		}

		// Token: 0x0400006E RID: 110
		[SerializeField]
		private AzureWeatherPreset m_weatherPreset;

		// Token: 0x0400006F RID: 111
		[SerializeField]
		private float m_transitionTime = 10f;
	}
}
