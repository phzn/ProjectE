using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C3 RID: 4803
	[Serializable]
	public class NPCFacialExpressionEntry
	{
		// Token: 0x040099DE RID: 39390
		[Tooltip("The reaction type this expression applies to")]
		public ReactionType reactionType;

		// Token: 0x040099DF RID: 39391
		[Tooltip("Animator parameter value for the facial expression (FaceExpressionIndex)")]
		public int expressionIndex;

		// Token: 0x040099E0 RID: 39392
		[Tooltip("Optional: Duration to hold the expression (0 = use reaction duration)")]
		public float expressionDuration;
	}
}
