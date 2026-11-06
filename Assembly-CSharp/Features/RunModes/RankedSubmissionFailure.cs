using System;

namespace Features.RunModes
{
	// Token: 0x02000D5A RID: 3418
	public sealed class RankedSubmissionFailure
	{
		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x0600645D RID: 25693 RVA: 0x00282B2D File Offset: 0x00280D2D
		public string RunId { get; }

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x0600645E RID: 25694 RVA: 0x00282B35 File Offset: 0x00280D35
		public string Message { get; }

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x0600645F RID: 25695 RVA: 0x00282B3D File Offset: 0x00280D3D
		public bool RetainedForRetry { get; }

		// Token: 0x06006460 RID: 25696 RVA: 0x00282B45 File Offset: 0x00280D45
		public RankedSubmissionFailure(string runId, string message, bool retainedForRetry)
		{
			this.RunId = runId ?? string.Empty;
			this.Message = message ?? string.Empty;
			this.RetainedForRetry = retainedForRetry;
		}
	}
}
