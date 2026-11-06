using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000041 RID: 65
	internal static class SetPropertyUtility
	{
		// Token: 0x06000328 RID: 808 RVA: 0x00022A74 File Offset: 0x00020C74
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00022AC3 File Offset: 0x00020CC3
		public static bool SetEquatableStruct<T>(ref T currentValue, T newValue) where T : IEquatable<T>
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00022ADE File Offset: 0x00020CDE
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00022B00 File Offset: 0x00020D00
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
