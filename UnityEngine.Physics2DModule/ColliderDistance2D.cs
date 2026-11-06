using System;

namespace UnityEngine
{
	// Token: 0x02000017 RID: 23
	public struct ColliderDistance2D
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00006560 File Offset: 0x00004760
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00006578 File Offset: 0x00004778
		public Vector2 pointA
		{
			get
			{
				return this.m_PointA;
			}
			set
			{
				this.m_PointA = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00006584 File Offset: 0x00004784
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000659C File Offset: 0x0000479C
		public Vector2 pointB
		{
			get
			{
				return this.m_PointB;
			}
			set
			{
				this.m_PointB = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000065A8 File Offset: 0x000047A8
		public Vector2 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000210 RID: 528 RVA: 0x000065C0 File Offset: 0x000047C0
		// (set) Token: 0x06000211 RID: 529 RVA: 0x000065D8 File Offset: 0x000047D8
		public float distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				this.m_Distance = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000065E4 File Offset: 0x000047E4
		public bool isOverlapped
		{
			get
			{
				return this.m_Distance < 0f;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00006604 File Offset: 0x00004804
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000661F File Offset: 0x0000481F
		public bool isValid
		{
			get
			{
				return this.m_IsValid != 0;
			}
			set
			{
				this.m_IsValid = (value ? 1 : 0);
			}
		}

		// Token: 0x0400005A RID: 90
		private Vector2 m_PointA;

		// Token: 0x0400005B RID: 91
		private Vector2 m_PointB;

		// Token: 0x0400005C RID: 92
		private Vector2 m_Normal;

		// Token: 0x0400005D RID: 93
		private float m_Distance;

		// Token: 0x0400005E RID: 94
		private int m_IsValid;
	}
}
