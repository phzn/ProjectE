using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004B2 RID: 1202
	public static class Easing
	{
		// Token: 0x0600254A RID: 9546 RVA: 0x0009E018 File Offset: 0x0009C218
		public static float Step(float t)
		{
			return (float)((t < 0.5f) ? 0 : 1);
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x0009E038 File Offset: 0x0009C238
		public static float Linear(float t)
		{
			return t;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x0009E04C File Offset: 0x0009C24C
		public static float InSine(float t)
		{
			return Mathf.Sin(1.5707964f * (t - 1f)) + 1f;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x0009E078 File Offset: 0x0009C278
		public static float OutSine(float t)
		{
			return Mathf.Sin(t * 1.5707964f);
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x0009E098 File Offset: 0x0009C298
		public static float InOutSine(float t)
		{
			return (Mathf.Sin(3.1415927f * (t - 0.5f)) + 1f) * 0.5f;
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x0009E0C8 File Offset: 0x0009C2C8
		public static float InQuad(float t)
		{
			return t * t;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x0009E0E0 File Offset: 0x0009C2E0
		public static float OutQuad(float t)
		{
			return t * (2f - t);
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x0009E0FC File Offset: 0x0009C2FC
		public static float InOutQuad(float t)
		{
			t *= 2f;
			bool flag = t < 1f;
			float num;
			if (flag)
			{
				num = t * t * 0.5f;
			}
			else
			{
				num = -0.5f * ((t - 1f) * (t - 3f) - 1f);
			}
			return num;
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x0009E14C File Offset: 0x0009C34C
		public static float InCubic(float t)
		{
			return Easing.InPower(t, 3);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x0009E168 File Offset: 0x0009C368
		public static float OutCubic(float t)
		{
			return Easing.OutPower(t, 3);
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x0009E184 File Offset: 0x0009C384
		public static float InOutCubic(float t)
		{
			return Easing.InOutPower(t, 3);
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x0009E1A0 File Offset: 0x0009C3A0
		public static float InPower(float t, int power)
		{
			return Mathf.Pow(t, (float)power);
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x0009E1BC File Offset: 0x0009C3BC
		public static float OutPower(float t, int power)
		{
			int num = ((power % 2 == 0) ? (-1) : 1);
			return (float)num * (Mathf.Pow(t - 1f, (float)power) + (float)num);
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x0009E1F0 File Offset: 0x0009C3F0
		public static float InOutPower(float t, int power)
		{
			t *= 2f;
			bool flag = t < 1f;
			float num;
			if (flag)
			{
				num = Easing.InPower(t, power) * 0.5f;
			}
			else
			{
				int num2 = ((power % 2 == 0) ? (-1) : 1);
				num = (float)num2 * 0.5f * (Mathf.Pow(t - 2f, (float)power) + (float)(num2 * 2));
			}
			return num;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x0009E250 File Offset: 0x0009C450
		public static float InBounce(float t)
		{
			return 1f - Easing.OutBounce(1f - t);
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x0009E274 File Offset: 0x0009C474
		public static float OutBounce(float t)
		{
			bool flag = t < 0.36363637f;
			float num;
			if (flag)
			{
				num = 7.5625f * t * t;
			}
			else
			{
				bool flag2 = t < 0.72727275f;
				if (flag2)
				{
					float num2;
					t = (num2 = t - 0.54545456f);
					num = 7.5625f * num2 * t + 0.75f;
				}
				else
				{
					bool flag3 = t < 0.90909094f;
					if (flag3)
					{
						float num3;
						t = (num3 = t - 0.8181818f);
						num = 7.5625f * num3 * t + 0.9375f;
					}
					else
					{
						float num4;
						t = (num4 = t - 0.95454544f);
						num = 7.5625f * num4 * t + 0.984375f;
					}
				}
			}
			return num;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x0009E314 File Offset: 0x0009C514
		public static float InOutBounce(float t)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InBounce(t * 2f) * 0.5f;
			}
			else
			{
				num = Easing.OutBounce((t - 0.5f) * 2f) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x0009E368 File Offset: 0x0009C568
		public static float InElastic(float t)
		{
			bool flag = t == 0f;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = t == 1f;
				if (flag2)
				{
					num = 1f;
				}
				else
				{
					float num2 = 0.3f;
					float num3 = num2 / 4f;
					float num4 = Mathf.Pow(2f, 10f * (t -= 1f));
					num = -(num4 * Mathf.Sin((t - num3) * 6.2831855f / num2));
				}
			}
			return num;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x0009E3E4 File Offset: 0x0009C5E4
		public static float OutElastic(float t)
		{
			bool flag = t == 0f;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = t == 1f;
				if (flag2)
				{
					num = 1f;
				}
				else
				{
					float num2 = 0.3f;
					float num3 = num2 / 4f;
					num = Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - num3) * 6.2831855f / num2) + 1f;
				}
			}
			return num;
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x0009E458 File Offset: 0x0009C658
		public static float InOutElastic(float t)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InElastic(t * 2f) * 0.5f;
			}
			else
			{
				num = Easing.OutElastic((t - 0.5f) * 2f) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x0009E4AC File Offset: 0x0009C6AC
		public static float InBack(float t)
		{
			float num = 1.70158f;
			return t * t * ((num + 1f) * t - num);
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x0009E4D4 File Offset: 0x0009C6D4
		public static float OutBack(float t)
		{
			return 1f - Easing.InBack(1f - t);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x0009E4F8 File Offset: 0x0009C6F8
		public static float InOutBack(float t)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InBack(t * 2f) * 0.5f;
			}
			else
			{
				num = Easing.OutBack((t - 0.5f) * 2f) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x0009E54C File Offset: 0x0009C74C
		public static float InBack(float t, float s)
		{
			return t * t * ((s + 1f) * t - s);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x0009E570 File Offset: 0x0009C770
		public static float OutBack(float t, float s)
		{
			return 1f - Easing.InBack(1f - t, s);
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x0009E598 File Offset: 0x0009C798
		public static float InOutBack(float t, float s)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InBack(t * 2f, s) * 0.5f;
			}
			else
			{
				num = Easing.OutBack((t - 0.5f) * 2f, s) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x0009E5F0 File Offset: 0x0009C7F0
		public static float InCirc(float t)
		{
			return -(Mathf.Sqrt(1f - t * t) - 1f);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x0009E618 File Offset: 0x0009C818
		public static float OutCirc(float t)
		{
			t -= 1f;
			return Mathf.Sqrt(1f - t * t);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x0009E644 File Offset: 0x0009C844
		public static float InOutCirc(float t)
		{
			t *= 2f;
			bool flag = t < 1f;
			float num;
			if (flag)
			{
				num = -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
			}
			else
			{
				t -= 2f;
				num = 0.5f * (Mathf.Sqrt(1f - t * t) + 1f);
			}
			return num;
		}

		// Token: 0x04001231 RID: 4657
		private const float HalfPi = 1.5707964f;
	}
}
