using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001243 RID: 4675
	public enum PresetReferenceTarget
	{
		// Token: 0x04009593 RID: 38291
		[Tooltip("Use the phase default: NPC transform during NPC phase, Player transform during Emote phase.")]
		Default,
		// Token: 0x04009594 RID: 38292
		[Tooltip("Always use the Player (Ciel) transform, regardless of which phase is active.")]
		Player,
		// Token: 0x04009595 RID: 38293
		[Tooltip("Always use the NPC transform, regardless of which phase is active.")]
		NPC
	}
}
