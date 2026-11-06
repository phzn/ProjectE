using System;
using UnityEngine;

namespace ProjectExhibitionist.Lighting
{
	// Token: 0x020009D0 RID: 2512
	[DisallowMultipleComponent]
	[AddComponentMenu("")]
	public sealed class AtmosphereLampLayout : MonoBehaviour
	{
		// Token: 0x0400463F RID: 17983
		public const int CurrentOrientationVersion = 2;

		// Token: 0x04004640 RID: 17984
		[HideInInspector]
		public int orientationVersion;
	}
}
