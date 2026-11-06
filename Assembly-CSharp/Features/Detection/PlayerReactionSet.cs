using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012D6 RID: 4822
	[Serializable]
	public class PlayerReactionSet
	{
		// Token: 0x04009A9B RID: 39579
		[Range(1f, 100f)]
		[Tooltip("Higher weight = more likely to be selected")]
		public int weight = 10;

		// Token: 0x04009A9C RID: 39580
		public PlayerReactionEntry reaction;
	}
}
