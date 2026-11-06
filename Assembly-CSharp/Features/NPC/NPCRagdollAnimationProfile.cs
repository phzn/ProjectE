using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EDD RID: 3805
	[CreateAssetMenu(fileName = "NPCRagdollAnimationProfile", menuName = "Project Exhibitionist/NPC/Ragdoll Animation Profile")]
	public sealed class NPCRagdollAnimationProfile : ScriptableObject
	{
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x060071E3 RID: 29155 RVA: 0x002E4CDA File Offset: 0x002E2EDA
		public bool IsValid
		{
			get
			{
				return this.standUpFromBack != null && this.standUpFromBelly != null;
			}
		}

		// Token: 0x040074E3 RID: 29923
		public AnimationClip standUpFromBack;

		// Token: 0x040074E4 RID: 29924
		public AnimationClip standUpFromBelly;
	}
}
