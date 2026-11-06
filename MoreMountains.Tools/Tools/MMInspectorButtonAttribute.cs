using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000A7 RID: 167
	[AttributeUsage(AttributeTargets.Field)]
	public class MMInspectorButtonAttribute : PropertyAttribute
	{
		// Token: 0x0600036B RID: 875 RVA: 0x000117CD File Offset: 0x0000F9CD
		public MMInspectorButtonAttribute(string MethodName)
		{
			this.MethodName = MethodName;
		}

		// Token: 0x040004D8 RID: 1240
		public readonly string MethodName;
	}
}
