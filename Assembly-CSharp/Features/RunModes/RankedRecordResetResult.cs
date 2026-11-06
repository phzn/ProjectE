using System;

namespace Features.RunModes
{
	// Token: 0x02000D55 RID: 3413
	public sealed class RankedRecordResetResult
	{
		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06006451 RID: 25681 RVA: 0x00282AFE File Offset: 0x00280CFE
		public bool Success { get; }

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06006452 RID: 25682 RVA: 0x00282B06 File Offset: 0x00280D06
		public string ErrorMessage { get; }

		// Token: 0x06006453 RID: 25683 RVA: 0x00282B0E File Offset: 0x00280D0E
		public RankedRecordResetResult(bool success, string errorMessage = null)
		{
			this.Success = success;
			this.ErrorMessage = errorMessage ?? string.Empty;
		}
	}
}
