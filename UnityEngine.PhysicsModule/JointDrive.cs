using System;

namespace UnityEngine
{
	// Token: 0x02000009 RID: 9
	public struct JointDrive
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002204 File Offset: 0x00000404
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000221C File Offset: 0x0000041C
		public float positionSpring
		{
			get
			{
				return this.m_PositionSpring;
			}
			set
			{
				this.m_PositionSpring = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002228 File Offset: 0x00000428
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002240 File Offset: 0x00000440
		public float positionDamper
		{
			get
			{
				return this.m_PositionDamper;
			}
			set
			{
				this.m_PositionDamper = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000224C File Offset: 0x0000044C
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002264 File Offset: 0x00000464
		public float maximumForce
		{
			get
			{
				return this.m_MaximumForce;
			}
			set
			{
				this.m_MaximumForce = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002270 File Offset: 0x00000470
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000228B File Offset: 0x0000048B
		public bool useAcceleration
		{
			get
			{
				return this.m_UseAcceleration == 1;
			}
			set
			{
				this.m_UseAcceleration = (value ? 1 : 0);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000229C File Offset: 0x0000049C
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002187 File Offset: 0x00000387
		[Obsolete("JointDriveMode is obsolete")]
		public JointDriveMode mode
		{
			get
			{
				return JointDriveMode.None;
			}
			set
			{
			}
		}

		// Token: 0x04000026 RID: 38
		private float m_PositionSpring;

		// Token: 0x04000027 RID: 39
		private float m_PositionDamper;

		// Token: 0x04000028 RID: 40
		private float m_MaximumForce;

		// Token: 0x04000029 RID: 41
		private int m_UseAcceleration;
	}
}
