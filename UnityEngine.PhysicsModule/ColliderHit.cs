using System;

namespace UnityEngine
{
	// Token: 0x02000042 RID: 66
	public struct ColliderHit
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00006BF5 File Offset: 0x00004DF5
		public int instanceID
		{
			get
			{
				return this.m_ColliderInstanceID;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00006BFD File Offset: 0x00004DFD
		public Collider collider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.instanceID) as Collider;
			}
		}

		// Token: 0x04000106 RID: 262
		private int m_ColliderInstanceID;
	}
}
