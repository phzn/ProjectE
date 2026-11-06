using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C6 RID: 4806
	[Serializable]
	public class NPCReactionEntry
	{
		// Token: 0x040099F6 RID: 39414
		public ReactionType Type;

		// Token: 0x040099F7 RID: 39415
		[Range(0f, 100f)]
		public int Weight = 10;

		// Token: 0x040099F8 RID: 39416
		[Header("Animator Integration")]
		[Tooltip("Maps to ReactionType parameter in Animator (1=TakePhoto, 2=TakeVideo, etc.)")]
		public int AnimatorReactionIndex;

		// Token: 0x040099F9 RID: 39417
		[Tooltip("Trigger name in Animator (default: ReactionTrigger)")]
		public string AnimationTrigger = "ReactionTrigger";

		// Token: 0x040099FA RID: 39418
		[Tooltip("Duration in seconds to play the reaction animation before entering idle state")]
		public float ReactionDuration = 2f;

		// Token: 0x040099FB RID: 39419
		[Header("Props & Movement")]
		[Tooltip("List of GameObject names (children of the NPC) to activate during this reaction")]
		public List<string> PropsToActivate;

		// Token: 0x040099FC RID: 39420
		[Tooltip("If true, the NPC stays in place. If false, it might move (handled by behavior)")]
		public bool IsStationary = true;

		// Token: 0x040099FD RID: 39421
		[Header("Legacy Props (Optional)")]
		public GameObject PropsPrefab;
	}
}
