using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B3 RID: 435
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class MMFEnumConditionAttribute : PropertyAttribute
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x0002F82E File Offset: 0x0002DA2E
		public bool ContainsBitFlag(int enumValue)
		{
			return this.bitArray.Get(enumValue);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0002F83C File Offset: 0x0002DA3C
		public MMFEnumConditionAttribute(string conditionBoolean, params int[] enumValues)
		{
			this.ConditionEnum = conditionBoolean;
			this.Hidden = true;
			for (int i = 0; i < enumValues.Length; i++)
			{
				this.bitArray.Set(enumValues[i], true);
			}
		}

		// Token: 0x04000C87 RID: 3207
		public string ConditionEnum = "";

		// Token: 0x04000C88 RID: 3208
		public bool Hidden;

		// Token: 0x04000C89 RID: 3209
		private BitArray bitArray = new BitArray(32);
	}
}
