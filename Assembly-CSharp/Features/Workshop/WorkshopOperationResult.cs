using System;

namespace Features.Workshop
{
	// Token: 0x02000B41 RID: 2881
	public readonly struct WorkshopOperationResult
	{
		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x060051CF RID: 20943 RVA: 0x001FB449 File Offset: 0x001F9649
		public bool Success
		{
			get
			{
				return this.ErrorCode == WorkshopErrorCode.None;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x060051D0 RID: 20944 RVA: 0x001FB454 File Offset: 0x001F9654
		public WorkshopErrorCode ErrorCode { get; }

		// Token: 0x060051D1 RID: 20945 RVA: 0x001FB45C File Offset: 0x001F965C
		public WorkshopOperationResult(WorkshopErrorCode errorCode)
		{
			this.ErrorCode = errorCode;
		}
	}
}
