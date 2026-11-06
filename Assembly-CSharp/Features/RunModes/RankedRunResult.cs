using System;

namespace Features.RunModes
{
	// Token: 0x02000D52 RID: 3410
	public readonly struct RankedRunResult
	{
		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x0600643A RID: 25658 RVA: 0x00282851 File Offset: 0x00280A51
		public int DistanceMeters { get; }

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x0600643B RID: 25659 RVA: 0x00282859 File Offset: 0x00280A59
		public int Score { get; }

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x0600643C RID: 25660 RVA: 0x00282861 File Offset: 0x00280A61
		public bool IsValid
		{
			get
			{
				return this.DistanceMeters > 0 && this.DistanceMeters < int.MaxValue && this.Score >= 0 && this.Score < int.MaxValue;
			}
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x00282891 File Offset: 0x00280A91
		public RankedRunResult(int distanceMeters, int score)
		{
			this.DistanceMeters = distanceMeters;
			this.Score = score;
		}

		// Token: 0x0600643E RID: 25662 RVA: 0x002828A4 File Offset: 0x00280AA4
		public static bool TryCreate(float distanceMeters, int score, out RankedRunResult result)
		{
			result = default(RankedRunResult);
			if (float.IsNaN(distanceMeters) || float.IsInfinity(distanceMeters) || distanceMeters < 0f || (double)distanceMeters >= 2147483647.0 || score < 0 || score == 2147483647)
			{
				return false;
			}
			double num = Math.Round((double)distanceMeters, MidpointRounding.ToEven);
			if (num <= 0.0 || num >= 2147483647.0)
			{
				return false;
			}
			result = new RankedRunResult((int)num, score);
			return true;
		}
	}
}
