using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006FD RID: 1789
	[Serializable]
	public class SkateBlendShapeOverride
	{
		// Token: 0x04002D3A RID: 11578
		[Tooltip("Nome esatto del blend shape nella mesh")]
		public string blendShapeName;

		// Token: 0x04002D3B RID: 11579
		[Tooltip("Valore target (0-100)")]
		[Range(0f, 100f)]
		public float targetValue = 100f;
	}
}
