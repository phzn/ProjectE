using System;
using UnityEngine;

namespace Features.BoobTouch
{
	// Token: 0x020014E4 RID: 5348
	public static class BreastInteractionSizing
	{
		// Token: 0x0600A4B6 RID: 42166 RVA: 0x0044AC20 File Offset: 0x00448E20
		public static BreastInteractionSizeProfile Resolve(int boobsType)
		{
			switch (Mathf.Clamp(boobsType, 0, 5))
			{
			case 0:
				return new BreastInteractionSizeProfile(0, "Flat", 0.5f, 0.78f, 0.62f, 0.72f, 0.28f, 0.62f, 0.42f, 0.72f, 0.45f, 0.9f, 0.88f, 0.72f, true);
			case 1:
				return new BreastInteractionSizeProfile(1, "Smaller", 0.68f, 0.84f, 0.75f, 0.82f, 0.52f, 0.74f, 0.62f, 0.82f, 0.65f, 0.94f, 0.92f, 0.82f, false);
			case 2:
				return new BreastInteractionSizeProfile(2, "Small", 0.84f, 0.91f, 0.88f, 0.91f, 0.76f, 0.86f, 0.8f, 0.91f, 0.82f, 0.97f, 0.96f, 0.91f, false);
			case 4:
				return new BreastInteractionSizeProfile(4, "Natural", 1.16f, 1.08f, 1.12f, 1.08f, 1.12f, 1.08f, 1.15f, 1.08f, 1.12f, 1.1f, 1.1f, 1.18f, false);
			case 5:
				return new BreastInteractionSizeProfile(5, "Balloon", 1.42f, 1.18f, 1.28f, 1.16f, 1.28f, 1.16f, 1.35f, 1.16f, 1.25f, 1.24f, 1.24f, 1.42f, false);
			}
			return new BreastInteractionSizeProfile(3, "Default", 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, false);
		}

		// Token: 0x0400A8F5 RID: 43253
		public const int SizeCount = 6;

		// Token: 0x0400A8F6 RID: 43254
		public const int DefaultBoobsType = 3;
	}
}
