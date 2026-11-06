using System;

namespace UnityEngine
{
	// Token: 0x0200001B RID: 27
	public struct JointAngleLimits2D
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00006CAC File Offset: 0x00004EAC
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00006CC4 File Offset: 0x00004EC4
		public float min
		{
			get
			{
				return this.m_LowerAngle;
			}
			set
			{
				this.m_LowerAngle = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00006CD0 File Offset: 0x00004ED0
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00006CE8 File Offset: 0x00004EE8
		public float max
		{
			get
			{
				return this.m_UpperAngle;
			}
			set
			{
				this.m_UpperAngle = value;
			}
		}

		// Token: 0x0400007F RID: 127
		private float m_LowerAngle;

		// Token: 0x04000080 RID: 128
		private float m_UpperAngle;
	}
}
