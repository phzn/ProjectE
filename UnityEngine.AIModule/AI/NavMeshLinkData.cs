using System;

namespace UnityEngine.AI
{
	// Token: 0x02000014 RID: 20
	public struct NavMeshLinkData
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00002D40 File Offset: 0x00000F40
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00002D58 File Offset: 0x00000F58
		public Vector3 startPosition
		{
			get
			{
				return this.m_StartPosition;
			}
			set
			{
				this.m_StartPosition = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00002D64 File Offset: 0x00000F64
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00002D7C File Offset: 0x00000F7C
		public Vector3 endPosition
		{
			get
			{
				return this.m_EndPosition;
			}
			set
			{
				this.m_EndPosition = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00002D88 File Offset: 0x00000F88
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public float costModifier
		{
			get
			{
				return this.m_CostModifier;
			}
			set
			{
				this.m_CostModifier = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00002DAC File Offset: 0x00000FAC
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00002DC7 File Offset: 0x00000FC7
		public bool bidirectional
		{
			get
			{
				return this.m_Bidirectional != 0;
			}
			set
			{
				this.m_Bidirectional = (value ? 1 : 0);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00002DD8 File Offset: 0x00000FD8
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00002DF0 File Offset: 0x00000FF0
		public float width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00002DFC File Offset: 0x00000FFC
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00002E14 File Offset: 0x00001014
		public int area
		{
			get
			{
				return this.m_Area;
			}
			set
			{
				this.m_Area = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00002E20 File Offset: 0x00001020
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00002E38 File Offset: 0x00001038
		public int agentTypeID
		{
			get
			{
				return this.m_AgentTypeID;
			}
			set
			{
				this.m_AgentTypeID = value;
			}
		}

		// Token: 0x04000031 RID: 49
		private Vector3 m_StartPosition;

		// Token: 0x04000032 RID: 50
		private Vector3 m_EndPosition;

		// Token: 0x04000033 RID: 51
		private float m_CostModifier;

		// Token: 0x04000034 RID: 52
		private int m_Bidirectional;

		// Token: 0x04000035 RID: 53
		private float m_Width;

		// Token: 0x04000036 RID: 54
		private int m_Area;

		// Token: 0x04000037 RID: 55
		private int m_AgentTypeID;
	}
}
