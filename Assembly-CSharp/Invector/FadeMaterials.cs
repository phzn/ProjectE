using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000448 RID: 1096
	[Serializable]
	public class FadeMaterials
	{
		// Token: 0x04001CAE RID: 7342
		public Renderer renderer;

		// Token: 0x04001CAF RID: 7343
		public Material[] originalMaterials;

		// Token: 0x04001CB0 RID: 7344
		public Material[] fadeMaterials;

		// Token: 0x04001CB1 RID: 7345
		public float[] originalAlpha;
	}
}
