using System;

namespace UnityEngine
{
	// Token: 0x0200001E RID: 30
	public struct JointSuspension2D
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00006D84 File Offset: 0x00004F84
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00006D9C File Offset: 0x00004F9C
		public float dampingRatio
		{
			get
			{
				return this.m_DampingRatio;
			}
			set
			{
				this.m_DampingRatio = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00006DA8 File Offset: 0x00004FA8
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00006DC0 File Offset: 0x00004FC0
		public float frequency
		{
			get
			{
				return this.m_Frequency;
			}
			set
			{
				this.m_Frequency = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00006DCC File Offset: 0x00004FCC
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00006DE4 File Offset: 0x00004FE4
		public float angle
		{
			get
			{
				return this.m_Angle;
			}
			set
			{
				this.m_Angle = value;
			}
		}

		// Token: 0x04000085 RID: 133
		private float m_DampingRatio;

		// Token: 0x04000086 RID: 134
		private float m_Frequency;

		// Token: 0x04000087 RID: 135
		private float m_Angle;
	}
}
