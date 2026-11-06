using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F4 RID: 244
	public class MMArray : MonoBehaviour
	{
		// Token: 0x0600051C RID: 1308 RVA: 0x00017404 File Offset: 0x00015604
		public static int RoundIntToArray(int value, int[] array)
		{
			int num = 0;
			if (array[num] >= value)
			{
				return array[num];
			}
			int num2 = array.Length - 1;
			if (array[num2] <= value)
			{
				return array[num2];
			}
			while (num2 - num > 1)
			{
				int num3 = (num2 + num) / 2;
				if (array[num3] == value)
				{
					return array[num3];
				}
				if (array[num3] < value)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			if (array[num2] - value <= value - array[num])
			{
				return array[num2];
			}
			return array[num];
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00017464 File Offset: 0x00015664
		public static float RoundFloatToArray(float value, float[] array)
		{
			int num = 0;
			if (array[num] >= value)
			{
				return array[num];
			}
			int num2 = array.Length - 1;
			if (array[num2] <= value)
			{
				return array[num2];
			}
			while (num2 - num > 1)
			{
				int num3 = (num2 + num) / 2;
				if (array[num3] == value)
				{
					return array[num3];
				}
				if (array[num3] < value)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			if (array[num2] - value <= value - array[num])
			{
				return array[num2];
			}
			return array[num];
		}
	}
}
