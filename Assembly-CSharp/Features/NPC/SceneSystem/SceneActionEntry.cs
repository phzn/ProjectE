using System;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F25 RID: 3877
	[Serializable]
	public class SceneActionEntry
	{
		// Token: 0x0400774C RID: 30540
		[Tooltip("Optional label for this step (for debugging/editor readability)")]
		public string label;

		// Token: 0x0400774D RID: 30541
		[Tooltip("The action to execute")]
		public SceneActionSO action;

		// Token: 0x0400774E RID: 30542
		[Tooltip("Target NPC slot ID (leave empty for non-NPC actions like Wait)")]
		public string targetSlotId;

		// Token: 0x0400774F RID: 30543
		[Tooltip("If true, this action runs in parallel with the next one(s) until a non-parallel action is found")]
		public bool parallelWithNext;
	}
}
