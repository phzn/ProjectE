using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008BC RID: 2236
	[Serializable]
	public class LinkedColorSets
	{
		// Token: 0x04003C35 RID: 15413
		public OutfitType linkedType;

		// Token: 0x04003C36 RID: 15414
		[Range(1f, 9f)]
		public int linkedChannelRange;
	}
}
