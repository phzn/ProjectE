using System;

namespace UnityEngine
{
	// Token: 0x0200000A RID: 10
	public struct AccelerationEvent
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002248 File Offset: 0x00000448
		public Vector3 acceleration
		{
			get
			{
				return new Vector3(this.x, this.y, this.z);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002274 File Offset: 0x00000474
		public float deltaTime
		{
			get
			{
				return this.m_TimeDelta;
			}
		}

		// Token: 0x04000036 RID: 54
		internal float x;

		// Token: 0x04000037 RID: 55
		internal float y;

		// Token: 0x04000038 RID: 56
		internal float z;

		// Token: 0x04000039 RID: 57
		internal float m_TimeDelta;
	}
}
