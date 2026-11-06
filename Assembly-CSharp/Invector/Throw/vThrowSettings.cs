using System;
using UnityEngine;

namespace Invector.Throw
{
	// Token: 0x02000503 RID: 1283
	[CreateAssetMenu(menuName = "Invector/Throw/New ThrowSettings")]
	public class vThrowSettings : ScriptableObject
	{
		// Token: 0x04001FC7 RID: 8135
		[Tooltip("Align End point UP to surface normal direction")]
		public bool alignToSurfaceNormal = true;

		// Token: 0x04001FC8 RID: 8136
		[Tooltip("End point UP always face to the view direction when don't has a surface")]
		public bool alignToViewWhenNotHit = true;

		// Token: 0x04001FC9 RID: 8137
		public bool disableEndPointWhenNotHit = true;

		// Token: 0x04001FCA RID: 8138
		[Tooltip("End point alignment smooth")]
		public float alignmentSmooth = 20f;

		// Token: 0x04001FCB RID: 8139
		[Min(0.001f)]
		public float metersPerSeconds = 0.1f;

		// Token: 0x04001FCC RID: 8140
		[vMinMax(0.0001f, 10f)]
		public Vector2 minMaxTime = new Vector2(0.1f, 1f);

		// Token: 0x04001FCD RID: 8141
		public float maxDistance = 100f;

		// Token: 0x04001FCE RID: 8142
		public float maxVelocity = 10f;

		// Token: 0x04001FCF RID: 8143
		public float throwMaxForce = 15f;

		// Token: 0x04001FD0 RID: 8144
		public float throwDelayTime = 0.25f;

		// Token: 0x04001FD1 RID: 8145
		[Min(0.001f)]
		public float lineStepPerTime = 0.1f;

		// Token: 0x04001FD2 RID: 8146
		public float maxLineLength = 100f;

		// Token: 0x04001FD3 RID: 8147
		public float exitThrowModeDelay = 0.5f;

		// Token: 0x04001FD4 RID: 8148
		public string cameraStateStanding = "ThrowStanding";

		// Token: 0x04001FD5 RID: 8149
		public string cameraStateCrouching = "ThrowCrouching";

		// Token: 0x04001FD6 RID: 8150
		public string throwAnimation = "ThrowObject";

		// Token: 0x04001FD7 RID: 8151
		public string holdingAnimation = "HoldingObject";

		// Token: 0x04001FD8 RID: 8152
		public string cancelAnimation = "CancelThrow";
	}
}
