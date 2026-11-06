using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200125D RID: 4701
	public enum IKReferenceMode
	{
		// Token: 0x0400966A RID: 38506
		[Tooltip("Offset relative to the global reference bone (default: Hips)")]
		GlobalReferenceBone,
		// Token: 0x0400966B RID: 38507
		[Tooltip("Offset relative to a custom bone assigned per-limb")]
		CustomBone
	}
}
