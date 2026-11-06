using System;
using UnityEngine;

namespace Features.Suspicion
{
	// Token: 0x02000C36 RID: 3126
	[DisallowMultipleComponent]
	public class SuspicionSource : MonoBehaviour
	{
		// Token: 0x04005B0C RID: 23308
		[Tooltip("Positive values raise wanted by one tier when this NPC captures the player.\nSet 1 for NPCs or reporters, 0 for police. Legacy positive weights also count as one tier.")]
		[Min(0f)]
		public float suspicionDelta = 1f;
	}
}
