using System;

namespace Features.Score
{
	// Token: 0x02000C75 RID: 3189
	public readonly struct RunScoreAwardRequest
	{
		// Token: 0x06005C78 RID: 23672 RVA: 0x002450BC File Offset: 0x002432BC
		public RunScoreAwardRequest(RunScoreReason reason, int points, string label, string detail = null, string groupKey = null, RunScorePresentation presentation = RunScorePresentation.Generic)
		{
			this.Reason = reason;
			this.Points = points;
			this.Label = label ?? string.Empty;
			this.Detail = detail ?? string.Empty;
			this.GroupKey = groupKey ?? string.Empty;
			this.Presentation = presentation;
		}

		// Token: 0x04005DC7 RID: 24007
		public readonly RunScoreReason Reason;

		// Token: 0x04005DC8 RID: 24008
		public readonly int Points;

		// Token: 0x04005DC9 RID: 24009
		public readonly string Label;

		// Token: 0x04005DCA RID: 24010
		public readonly string Detail;

		// Token: 0x04005DCB RID: 24011
		public readonly string GroupKey;

		// Token: 0x04005DCC RID: 24012
		public readonly RunScorePresentation Presentation;
	}
}
