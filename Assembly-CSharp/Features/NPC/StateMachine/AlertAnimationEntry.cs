using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F07 RID: 3847
	[Serializable]
	public class AlertAnimationEntry
	{
		// Token: 0x040076BC RID: 30396
		[Tooltip("Display name for this animation (for debugging/editor)")]
		public string displayName = "Animation";

		// Token: 0x040076BD RID: 30397
		[Header("Selection")]
		[Tooltip("Weight for random selection (higher = more likely)")]
		[Range(1f, 100f)]
		public int weight = 10;

		// Token: 0x040076BE RID: 30398
		[Header("Animator")]
		[Tooltip("Index value to set on the animator parameter (e.g., SurprisedIndex or SearchingType)")]
		public int animationIndex;

		// Token: 0x040076BF RID: 30399
		[Header("Props")]
		[Tooltip("List of prop names (children of NPC) to activate during this animation")]
		public List<string> propsToActivate = new List<string>();

		// Token: 0x040076C0 RID: 30400
		[Tooltip("If true, props will be deactivated when exiting the state")]
		public bool deactivatePropsOnExit = true;
	}
}
