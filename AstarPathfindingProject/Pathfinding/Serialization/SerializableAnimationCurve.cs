using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000251 RID: 593
	[Preserve]
	internal class SerializableAnimationCurve
	{
		// Token: 0x04000B17 RID: 2839
		public WrapMode preWrapMode;

		// Token: 0x04000B18 RID: 2840
		public WrapMode postWrapMode;

		// Token: 0x04000B19 RID: 2841
		public Keyframe[] keys;
	}
}
