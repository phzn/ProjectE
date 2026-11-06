using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000B0 RID: 176
	public class MMVectorAttribute : PropertyAttribute
	{
		// Token: 0x0600037C RID: 892 RVA: 0x000118A9 File Offset: 0x0000FAA9
		public MMVectorAttribute(params string[] labels)
		{
			this.Labels = labels;
		}

		// Token: 0x040004E1 RID: 1249
		public readonly string[] Labels;
	}
}
