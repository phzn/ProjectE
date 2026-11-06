using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200009E RID: 158
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class MMConditionAttribute : PropertyAttribute
	{
		// Token: 0x0600035F RID: 863 RVA: 0x000116AB File Offset: 0x0000F8AB
		public MMConditionAttribute(string conditionBoolean)
		{
			this.ConditionBoolean = conditionBoolean;
			this.Hidden = false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000116CC File Offset: 0x0000F8CC
		public MMConditionAttribute(string conditionBoolean, bool hideInInspector)
		{
			this.ConditionBoolean = conditionBoolean;
			this.Hidden = hideInInspector;
			this.Negative = false;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000116F4 File Offset: 0x0000F8F4
		public MMConditionAttribute(string conditionBoolean, bool hideInInspector, bool negative)
		{
			this.ConditionBoolean = conditionBoolean;
			this.Hidden = hideInInspector;
			this.Negative = negative;
		}

		// Token: 0x040004CF RID: 1231
		public string ConditionBoolean = "";

		// Token: 0x040004D0 RID: 1232
		public bool Hidden;

		// Token: 0x040004D1 RID: 1233
		public bool Negative;
	}
}
