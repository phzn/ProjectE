using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B4 RID: 436
	public class MMFInformationAttribute : PropertyAttribute
	{
		// Token: 0x06000A88 RID: 2696 RVA: 0x0002F892 File Offset: 0x0002DA92
		public MMFInformationAttribute(string message, MMFInformationAttribute.InformationType type, bool messageAfterProperty)
		{
		}

		// Token: 0x0200048E RID: 1166
		public enum InformationType
		{
			// Token: 0x04002140 RID: 8512
			Error,
			// Token: 0x04002141 RID: 8513
			Info,
			// Token: 0x04002142 RID: 8514
			None,
			// Token: 0x04002143 RID: 8515
			Warning
		}
	}
}
