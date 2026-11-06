using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000A2 RID: 162
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class MMEnumConditionAttribute : PropertyAttribute
	{
		// Token: 0x06000365 RID: 869 RVA: 0x00011742 File Offset: 0x0000F942
		public bool ContainsBitFlag(int enumValue)
		{
			return this.bitArray.Get(enumValue);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00011750 File Offset: 0x0000F950
		public MMEnumConditionAttribute(string conditionBoolean, params int[] enumValues)
		{
			this.ConditionEnum = conditionBoolean;
			this.Hidden = true;
			for (int i = 0; i < enumValues.Length; i++)
			{
				this.bitArray.Set(enumValues[i], true);
			}
		}

		// Token: 0x040004D4 RID: 1236
		public string ConditionEnum = "";

		// Token: 0x040004D5 RID: 1237
		public bool Hidden;

		// Token: 0x040004D6 RID: 1238
		private BitArray bitArray = new BitArray(32);
	}
}
