using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	// Token: 0x02000036 RID: 54
	internal static class SetPropertyUtility
	{
		// Token: 0x060003FC RID: 1020 RVA: 0x00013FD8 File Offset: 0x000121D8
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014027 File Offset: 0x00012227
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014048 File Offset: 0x00012248
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}
}
