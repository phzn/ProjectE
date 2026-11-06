using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public class BlendShapeOverride
	{
		// Token: 0x04000114 RID: 276
		[Tooltip("Nome esatto del blend shape nella mesh")]
		public string blendShapeName;

		// Token: 0x04000115 RID: 277
		[Tooltip("Valore target (0-100)")]
		[Range(0f, 100f)]
		public float targetValue = 100f;
	}
}
