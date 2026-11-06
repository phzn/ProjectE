using System;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000087 RID: 135
	internal struct BuddyAllocation
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x0001EBA8 File Offset: 0x0001CDA8
		public BuddyAllocation(int level, int index)
		{
			this.level = level;
			this.index = index;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
		public uint2 index2D
		{
			get
			{
				return SpaceFillingCurves.DecodeMorton2D((uint)this.index);
			}
		}

		// Token: 0x040003C1 RID: 961
		public int level;

		// Token: 0x040003C2 RID: 962
		public int index;
	}
}
