using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000135 RID: 309
	internal static class InputArrayExtensions
	{
		// Token: 0x06001135 RID: 4405 RVA: 0x000524FC File Offset: 0x000506FC
		public static int IndexOfReference<TValue>(this InlinedArray<TValue> array, TValue value) where TValue : class
		{
			for (int i = 0; i < array.length; i++)
			{
				if (array[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00052534 File Offset: 0x00050734
		public static bool Contains<TValue>(this InlinedArray<TValue> array, TValue value)
		{
			for (int i = 0; i < array.length; i++)
			{
				TValue tvalue = array[i];
				if (tvalue.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00052573 File Offset: 0x00050773
		public static bool ContainsReference<TValue>(this InlinedArray<TValue> array, TValue value) where TValue : class
		{
			return array.IndexOfReference(value) != -1;
		}
	}
}
