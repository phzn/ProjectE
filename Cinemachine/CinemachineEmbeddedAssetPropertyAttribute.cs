using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000043 RID: 67
	public sealed class CinemachineEmbeddedAssetPropertyAttribute : PropertyAttribute
	{
		// Token: 0x060002CC RID: 716 RVA: 0x00012DE6 File Offset: 0x00010FE6
		public CinemachineEmbeddedAssetPropertyAttribute(bool warnIfNull = false)
		{
			this.WarnIfNull = warnIfNull;
		}

		// Token: 0x040001F4 RID: 500
		public bool WarnIfNull;
	}
}
