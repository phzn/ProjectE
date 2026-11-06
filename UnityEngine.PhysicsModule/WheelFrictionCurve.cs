using System;

namespace UnityEngine
{
	// Token: 0x02000006 RID: 6
	public struct WheelFrictionCurve
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		public float extremumSlip
		{
			get
			{
				return this.m_ExtremumSlip;
			}
			set
			{
				this.m_ExtremumSlip = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002074 File Offset: 0x00000274
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
		public float extremumValue
		{
			get
			{
				return this.m_ExtremumValue;
			}
			set
			{
				this.m_ExtremumValue = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002098 File Offset: 0x00000298
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020B0 File Offset: 0x000002B0
		public float asymptoteSlip
		{
			get
			{
				return this.m_AsymptoteSlip;
			}
			set
			{
				this.m_AsymptoteSlip = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020BC File Offset: 0x000002BC
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020D4 File Offset: 0x000002D4
		public float asymptoteValue
		{
			get
			{
				return this.m_AsymptoteValue;
			}
			set
			{
				this.m_AsymptoteValue = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020F8 File Offset: 0x000002F8
		public float stiffness
		{
			get
			{
				return this.m_Stiffness;
			}
			set
			{
				this.m_Stiffness = value;
			}
		}

		// Token: 0x0400001C RID: 28
		private float m_ExtremumSlip;

		// Token: 0x0400001D RID: 29
		private float m_ExtremumValue;

		// Token: 0x0400001E RID: 30
		private float m_AsymptoteSlip;

		// Token: 0x0400001F RID: 31
		private float m_AsymptoteValue;

		// Token: 0x04000020 RID: 32
		private float m_Stiffness;
	}
}
