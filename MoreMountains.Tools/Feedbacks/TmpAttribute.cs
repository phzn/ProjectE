using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001BA RID: 442
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class TmpAttribute : PropertyAttribute
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x0002F974 File Offset: 0x0002DB74
		public TmpAttribute(string header)
		{
			this.header = header;
		}

		// Token: 0x04000C94 RID: 3220
		public readonly string header;
	}
}
