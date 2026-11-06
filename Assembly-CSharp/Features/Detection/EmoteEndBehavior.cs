using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001280 RID: 4736
	public enum EmoteEndBehavior
	{
		// Token: 0x040097AD RID: 38829
		[Tooltip("Emote plays once then ends automatically after Duration")]
		TimedEnd,
		// Token: 0x040097AE RID: 38830
		[Tooltip("Emote loops (or holds last frame) until player manually ends it")]
		ManualEnd
	}
}
