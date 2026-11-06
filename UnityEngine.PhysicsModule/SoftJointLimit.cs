using System;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x02000007 RID: 7
	public struct SoftJointLimit
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002104 File Offset: 0x00000304
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000211C File Offset: 0x0000031C
		public float limit
		{
			get
			{
				return this.m_Limit;
			}
			set
			{
				this.m_Limit = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002128 File Offset: 0x00000328
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002140 File Offset: 0x00000340
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000214C File Offset: 0x0000034C
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002164 File Offset: 0x00000364
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002170 File Offset: 0x00000370
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("Spring has been moved to SoftJointLimitSpring class in Unity 5", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float spring
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000218C File Offset: 0x0000038C
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("Damper has been moved to SoftJointLimitSpring class in Unity 5", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float damper
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000021A4 File Offset: 0x000003A4
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002140 File Offset: 0x00000340
		[Obsolete("Use SoftJointLimit.bounciness instead", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float bouncyness
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

		// Token: 0x04000021 RID: 33
		private float m_Limit;

		// Token: 0x04000022 RID: 34
		private float m_Bounciness;

		// Token: 0x04000023 RID: 35
		private float m_ContactDistance;
	}
}
