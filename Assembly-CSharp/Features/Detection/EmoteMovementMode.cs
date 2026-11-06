using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200127F RID: 4735
	public enum EmoteMovementMode
	{
		// Token: 0x040097AA RID: 38826
		[Tooltip("Player is locked in place during emote")]
		Static,
		// Token: 0x040097AB RID: 38827
		[Tooltip("Player can move freely during emote (ThirdPersonController stays active)")]
		Moving
	}
}
