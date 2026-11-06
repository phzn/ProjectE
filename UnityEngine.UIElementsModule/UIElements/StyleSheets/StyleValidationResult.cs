using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x020004A6 RID: 1190
	internal struct StyleValidationResult
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x0009CB14 File Offset: 0x0009AD14
		public bool success
		{
			get
			{
				return this.status == StyleValidationStatus.Ok;
			}
		}

		// Token: 0x040011E2 RID: 4578
		public StyleValidationStatus status;

		// Token: 0x040011E3 RID: 4579
		public string message;

		// Token: 0x040011E4 RID: 4580
		public string errorValue;

		// Token: 0x040011E5 RID: 4581
		public string hint;
	}
}
