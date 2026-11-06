using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000013 RID: 19
	[AttributeUsage(AttributeTargets.Class)]
	public class TrackColorAttribute : Attribute
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000664F File Offset: 0x0000484F
		public Color color
		{
			get
			{
				return this.m_Color;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006657 File Offset: 0x00004857
		public TrackColorAttribute(float r, float g, float b)
		{
			this.m_Color = new Color(r, g, b);
		}

		// Token: 0x04000085 RID: 133
		private Color m_Color;
	}
}
