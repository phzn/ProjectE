using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000A6 RID: 166
	public class MMInformationAttribute : PropertyAttribute
	{
		// Token: 0x0600036A RID: 874 RVA: 0x000117C5 File Offset: 0x0000F9C5
		public MMInformationAttribute(string message, MMInformationAttribute.InformationType type, bool messageAfterProperty)
		{
		}

		// Token: 0x020003F7 RID: 1015
		public enum InformationType
		{
			// Token: 0x04001EA7 RID: 7847
			Error,
			// Token: 0x04001EA8 RID: 7848
			Info,
			// Token: 0x04001EA9 RID: 7849
			None,
			// Token: 0x04001EAA RID: 7850
			Warning
		}
	}
}
