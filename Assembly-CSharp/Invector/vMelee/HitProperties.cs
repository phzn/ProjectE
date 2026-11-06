using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x02000511 RID: 1297
	[Serializable]
	public class HitProperties
	{
		// Token: 0x04002029 RID: 8233
		[Tooltip("Tag to receive Damage")]
		public List<string> hitDamageTags = new List<string> { "Enemy" };

		// Token: 0x0400202A RID: 8234
		[Tooltip("Trigger a HitRecoil animation if the character attacks a obstacle")]
		public bool useRecoil = true;

		// Token: 0x0400202B RID: 8235
		public bool drawRecoilGizmos;

		// Token: 0x0400202C RID: 8236
		[Range(0f, 180f)]
		public float recoilRange = 90f;

		// Token: 0x0400202D RID: 8237
		[Tooltip("layer to Recoil Damage")]
		public LayerMask hitRecoilLayer = 1;
	}
}
