using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	public struct Vector3Bool
	{
		// Token: 0x06000309 RID: 777 RVA: 0x0000A3B8 File Offset: 0x000085B8
		public Vector3Bool(bool val)
		{
			this.z = val;
			this.y = val;
			this.x = val;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000A3DE File Offset: 0x000085DE
		public Vector3Bool(bool x, bool y, bool z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x04000141 RID: 321
		public bool x;

		// Token: 0x04000142 RID: 322
		public bool y;

		// Token: 0x04000143 RID: 323
		public bool z;
	}
}
