using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x02000690 RID: 1680
	public interface vICharacter : vIHealthController, vIDamageReceiver
	{
		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002787 RID: 10119
		OnActiveRagdoll onActiveRagdoll { get; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002788 RID: 10120
		Animator animator { get; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002789 RID: 10121
		bool isCrouching { get; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x0600278A RID: 10122
		// (set) Token: 0x0600278B RID: 10123
		bool ragdolled { get; set; }

		// Token: 0x0600278C RID: 10124
		void EnableRagdoll();

		// Token: 0x0600278D RID: 10125
		void ResetRagdoll();
	}
}
