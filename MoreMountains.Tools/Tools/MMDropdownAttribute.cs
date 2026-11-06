using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000A1 RID: 161
	public class MMDropdownAttribute : PropertyAttribute
	{
		// Token: 0x06000364 RID: 868 RVA: 0x00011733 File Offset: 0x0000F933
		public MMDropdownAttribute(params object[] dropdownValues)
		{
			this.DropdownValues = dropdownValues;
		}

		// Token: 0x040004D3 RID: 1235
		public readonly object[] DropdownValues;
	}
}
