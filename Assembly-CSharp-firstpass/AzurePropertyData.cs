using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public sealed class AzurePropertyData
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009C RID: 156 RVA: 0x0000380E File Offset: 0x00001A0E
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003816 File Offset: 0x00001A16
		public float floatData
		{
			get
			{
				return this.m_floatData;
			}
			set
			{
				this.m_floatData = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000381F File Offset: 0x00001A1F
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003827 File Offset: 0x00001A27
		public Color colorData
		{
			get
			{
				return this.m_colorData;
			}
			set
			{
				this.m_colorData = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003830 File Offset: 0x00001A30
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003838 File Offset: 0x00001A38
		public AnimationCurve curveData
		{
			get
			{
				return this.m_curveData;
			}
			set
			{
				this.m_curveData = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003841 File Offset: 0x00001A41
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003849 File Offset: 0x00001A49
		public Gradient gradientData
		{
			get
			{
				return this.m_gradientData;
			}
			set
			{
				this.m_gradientData = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003852 File Offset: 0x00001A52
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000385A File Offset: 0x00001A5A
		public Vector3 vector3Data
		{
			get
			{
				return this.m_vector3Data;
			}
			set
			{
				this.m_vector3Data = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003863 File Offset: 0x00001A63
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000386B File Offset: 0x00001A6B
		public AzureWeatherProperty weatherPropertyOwner
		{
			get
			{
				return this.m_weatherPropertyOwner;
			}
			set
			{
				this.m_weatherPropertyOwner = value;
			}
		}

		// Token: 0x04000083 RID: 131
		[SerializeField]
		private float m_floatData;

		// Token: 0x04000084 RID: 132
		[SerializeField]
		private Color m_colorData;

		// Token: 0x04000085 RID: 133
		[SerializeField]
		private AnimationCurve m_curveData;

		// Token: 0x04000086 RID: 134
		[SerializeField]
		private Gradient m_gradientData;

		// Token: 0x04000087 RID: 135
		[SerializeField]
		private Vector3 m_vector3Data;

		// Token: 0x04000088 RID: 136
		[SerializeField]
		private AzureWeatherProperty m_weatherPropertyOwner;
	}
}
