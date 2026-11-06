using System;

namespace Features.Score
{
	// Token: 0x02000C76 RID: 3190
	public readonly struct RunScoreAward
	{
		// Token: 0x06005C79 RID: 23673 RVA: 0x00245114 File Offset: 0x00243314
		public RunScoreAward(RunScoreAwardRequest request, int appliedPoints, int totalAfterAward, float unscaledTimestamp)
		{
			this.Reason = request.Reason;
			this.Points = appliedPoints;
			this.TotalAfterAward = totalAfterAward;
			this.Label = request.Label;
			this.Detail = request.Detail;
			this.GroupKey = request.GroupKey;
			this.Presentation = request.Presentation;
			this.UnscaledTimestamp = unscaledTimestamp;
		}

		// Token: 0x04005DCD RID: 24013
		public readonly RunScoreReason Reason;

		// Token: 0x04005DCE RID: 24014
		public readonly int Points;

		// Token: 0x04005DCF RID: 24015
		public readonly int TotalAfterAward;

		// Token: 0x04005DD0 RID: 24016
		public readonly string Label;

		// Token: 0x04005DD1 RID: 24017
		public readonly string Detail;

		// Token: 0x04005DD2 RID: 24018
		public readonly string GroupKey;

		// Token: 0x04005DD3 RID: 24019
		public readonly RunScorePresentation Presentation;

		// Token: 0x04005DD4 RID: 24020
		public readonly float UnscaledTimestamp;
	}
}
