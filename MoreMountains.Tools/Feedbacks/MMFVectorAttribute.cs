using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B7 RID: 439
	public class MMFVectorAttribute : PropertyAttribute
	{
		// Token: 0x06000A8D RID: 2701 RVA: 0x0002F91A File Offset: 0x0002DB1A
		public MMFVectorAttribute(params string[] labels)
		{
			this.Labels = labels;
		}

		// Token: 0x04000C8D RID: 3213
		public readonly string[] Labels;
	}
}
