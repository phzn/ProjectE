using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200046A RID: 1130
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class vReadOnlyAttribute : PropertyAttribute
	{
		// Token: 0x06001BF3 RID: 7155 RVA: 0x000AF7BE File Offset: 0x000AD9BE
		public vReadOnlyAttribute(bool justInPlayMode = true)
		{
			this.justInPlayMode = justInPlayMode;
		}

		// Token: 0x04001D57 RID: 7511
		public readonly bool justInPlayMode;
	}
}
