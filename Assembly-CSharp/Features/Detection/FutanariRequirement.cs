using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001281 RID: 4737
	public enum FutanariRequirement
	{
		// Token: 0x040097B0 RID: 38832
		[Tooltip("Emote sempre disponibile, indipendente da Futanari Mode")]
		Always,
		// Token: 0x040097B1 RID: 38833
		[Tooltip("Emote disponibile SOLO se Futanari Mode e' attiva")]
		FutanariOnly,
		// Token: 0x040097B2 RID: 38834
		[Tooltip("Emote disponibile SOLO se Futanari Mode e' disattiva")]
		NoFutanari
	}
}
