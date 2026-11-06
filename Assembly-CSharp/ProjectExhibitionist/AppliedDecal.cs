using System;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x02000903 RID: 2307
	[Serializable]
	public class AppliedDecal
	{
		// Token: 0x04003E89 RID: 16009
		[Tooltip("Riferimento al SkinDecal")]
		public SkinDecal decal;

		// Token: 0x04003E8A RID: 16010
		[Tooltip("Colore di tinta")]
		public Color tint = Color.white;

		// Token: 0x04003E8B RID: 16011
		[Tooltip("Opacità")]
		[Range(0f, 1f)]
		public float opacity = 1f;

		// Token: 0x04003E8C RID: 16012
		[Tooltip("Se attivo")]
		public bool isActive = true;
	}
}
