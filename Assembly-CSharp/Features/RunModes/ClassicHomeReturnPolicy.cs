using System;

namespace Features.RunModes
{
	// Token: 0x02000CA2 RID: 3234
	public static class ClassicHomeReturnPolicy
	{
		// Token: 0x06005DE9 RID: 24041 RVA: 0x0025207B File Offset: 0x0025027B
		public static int CalculateBonus(float peakMeters)
		{
			if (float.IsNaN(peakMeters) || float.IsInfinity(peakMeters) || peakMeters < 100f)
			{
				return 0;
			}
			return (int)Math.Min(2147483646.0, Math.Floor((double)peakMeters * 1.0));
		}

		// Token: 0x04005EF1 RID: 24305
		public const float MinimumDistanceMeters = 100f;

		// Token: 0x04005EF2 RID: 24306
		public const int PointsPerMeter = 1;

		// Token: 0x04005EF3 RID: 24307
		public const float FadeDuration = 0.65f;

		// Token: 0x04005EF4 RID: 24308
		public const float RecapDelay = 0.85f;
	}
}
