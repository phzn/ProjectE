using System;
using UnityEngine;

namespace Features.BoobTouch
{
	// Token: 0x020014E3 RID: 5347
	public readonly struct BreastInteractionSizeProfile
	{
		// Token: 0x0600A4B5 RID: 42165 RVA: 0x0044AB1C File Offset: 0x00448D1C
		public BreastInteractionSizeProfile(int boobsType, string label, float interactionScale, float clearanceScale, float acquireRadiusScale, float magnetRadiusScale, float fullGrabMotionScale, float nippleMotionScale, float clothRangeScale, float particleRadiusScale, float supportMotionScale, float cameraDistanceScale, float chestWidthScale, float chestDepthScale, bool flatPressMode = false)
		{
			this.boobsType = Mathf.Clamp(boobsType, 0, 5);
			this.label = label;
			this.interactionScale = Mathf.Max(0.1f, interactionScale);
			this.clearanceScale = Mathf.Max(0.1f, clearanceScale);
			this.acquireRadiusScale = Mathf.Max(0.1f, acquireRadiusScale);
			this.magnetRadiusScale = Mathf.Max(0.1f, magnetRadiusScale);
			this.fullGrabMotionScale = Mathf.Max(0.1f, fullGrabMotionScale);
			this.nippleMotionScale = Mathf.Max(0.1f, nippleMotionScale);
			this.clothRangeScale = Mathf.Max(0.1f, clothRangeScale);
			this.particleRadiusScale = Mathf.Max(0.1f, particleRadiusScale);
			this.supportMotionScale = Mathf.Max(0.1f, supportMotionScale);
			this.cameraDistanceScale = Mathf.Max(0.1f, cameraDistanceScale);
			this.chestWidthScale = Mathf.Max(0.1f, chestWidthScale);
			this.chestDepthScale = Mathf.Max(0.1f, chestDepthScale);
			this.flatPressMode = flatPressMode;
		}

		// Token: 0x0400A8E6 RID: 43238
		public readonly int boobsType;

		// Token: 0x0400A8E7 RID: 43239
		public readonly string label;

		// Token: 0x0400A8E8 RID: 43240
		public readonly float interactionScale;

		// Token: 0x0400A8E9 RID: 43241
		public readonly float clearanceScale;

		// Token: 0x0400A8EA RID: 43242
		public readonly float acquireRadiusScale;

		// Token: 0x0400A8EB RID: 43243
		public readonly float magnetRadiusScale;

		// Token: 0x0400A8EC RID: 43244
		public readonly float fullGrabMotionScale;

		// Token: 0x0400A8ED RID: 43245
		public readonly float nippleMotionScale;

		// Token: 0x0400A8EE RID: 43246
		public readonly float clothRangeScale;

		// Token: 0x0400A8EF RID: 43247
		public readonly float particleRadiusScale;

		// Token: 0x0400A8F0 RID: 43248
		public readonly float supportMotionScale;

		// Token: 0x0400A8F1 RID: 43249
		public readonly float cameraDistanceScale;

		// Token: 0x0400A8F2 RID: 43250
		public readonly float chestWidthScale;

		// Token: 0x0400A8F3 RID: 43251
		public readonly float chestDepthScale;

		// Token: 0x0400A8F4 RID: 43252
		public readonly bool flatPressMode;
	}
}
