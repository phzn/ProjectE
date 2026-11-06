using System;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x0200000D RID: 13
	public struct JointLimits
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002324 File Offset: 0x00000524
		// (set) Token: 0x0600002C RID: 44 RVA: 0x0000233C File Offset: 0x0000053C
		public float min
		{
			get
			{
				return this.m_Min;
			}
			set
			{
				this.m_Min = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002348 File Offset: 0x00000548
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002360 File Offset: 0x00000560
		public float max
		{
			get
			{
				return this.m_Max;
			}
			set
			{
				this.m_Max = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000236C File Offset: 0x0000056C
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002384 File Offset: 0x00000584
		public float bounciness
		{
			get
			{
				return this.m_Bounciness;
			}
			set
			{
				this.m_Bounciness = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002390 File Offset: 0x00000590
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000023A8 File Offset: 0x000005A8
		public float bounceMinVelocity
		{
			get
			{
				return this.m_BounceMinVelocity;
			}
			set
			{
				this.m_BounceMinVelocity = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000023B4 File Offset: 0x000005B4
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000023CC File Offset: 0x000005CC
		public float contactDistance
		{
			get
			{
				return this.m_ContactDistance;
			}
			set
			{
				this.m_ContactDistance = value;
			}
		}

		// Token: 0x04000034 RID: 52
		private float m_Min;

		// Token: 0x04000035 RID: 53
		private float m_Max;

		// Token: 0x04000036 RID: 54
		private float m_Bounciness;

		// Token: 0x04000037 RID: 55
		private float m_BounceMinVelocity;

		// Token: 0x04000038 RID: 56
		private float m_ContactDistance;

		// Token: 0x04000039 RID: 57
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("minBounce and maxBounce are replaced by a single JointLimits.bounciness for both limit ends.", true)]
		public float minBounce;

		// Token: 0x0400003A RID: 58
		[Obsolete("minBounce and maxBounce are replaced by a single JointLimits.bounciness for both limit ends.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float maxBounce;
	}
}
