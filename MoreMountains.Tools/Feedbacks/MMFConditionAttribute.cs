using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B6 RID: 438
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class MMFConditionAttribute : PropertyAttribute
	{
		// Token: 0x06000A8A RID: 2698 RVA: 0x0002F8A2 File Offset: 0x0002DAA2
		public MMFConditionAttribute(string conditionBoolean)
		{
			this.ConditionBoolean = conditionBoolean;
			this.Hidden = false;
			this.Negative = false;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0002F8CA File Offset: 0x0002DACA
		public MMFConditionAttribute(string conditionBoolean, bool hideInInspector)
		{
			this.ConditionBoolean = conditionBoolean;
			this.Hidden = hideInInspector;
			this.Negative = false;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0002F8F2 File Offset: 0x0002DAF2
		public MMFConditionAttribute(string conditionBoolean, bool hideInInspector, bool negative)
		{
			this.ConditionBoolean = conditionBoolean;
			this.Hidden = hideInInspector;
			this.Negative = negative;
		}

		// Token: 0x04000C8A RID: 3210
		public string ConditionBoolean = "";

		// Token: 0x04000C8B RID: 3211
		public bool Hidden;

		// Token: 0x04000C8C RID: 3212
		public bool Negative;
	}
}
