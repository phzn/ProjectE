using System;

namespace UnityEngine
{
	// Token: 0x02000008 RID: 8
	public struct SoftJointLimitSpring
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000021BC File Offset: 0x000003BC
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000021D4 File Offset: 0x000003D4
		public float spring
		{
			get
			{
				return this.m_Spring;
			}
			set
			{
				this.m_Spring = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000021E0 File Offset: 0x000003E0
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000021F8 File Offset: 0x000003F8
		public float damper
		{
			get
			{
				return this.m_Damper;
			}
			set
			{
				this.m_Damper = value;
			}
		}

		// Token: 0x04000024 RID: 36
		private float m_Spring;

		// Token: 0x04000025 RID: 37
		private float m_Damper;
	}
}
