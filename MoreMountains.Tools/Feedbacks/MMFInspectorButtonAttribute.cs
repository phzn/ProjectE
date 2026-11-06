using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B2 RID: 434
	[AttributeUsage(AttributeTargets.Field)]
	public class MMFInspectorButtonAttribute : PropertyAttribute
	{
		// Token: 0x06000A85 RID: 2693 RVA: 0x0002F81F File Offset: 0x0002DA1F
		public MMFInspectorButtonAttribute(string MethodName)
		{
			this.MethodName = MethodName;
		}

		// Token: 0x04000C86 RID: 3206
		public readonly string MethodName;
	}
}
