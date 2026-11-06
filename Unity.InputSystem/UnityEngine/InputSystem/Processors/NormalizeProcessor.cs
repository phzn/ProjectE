using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000106 RID: 262
	public class NormalizeProcessor : InputProcessor<float>
	{
		// Token: 0x06000F23 RID: 3875 RVA: 0x0004A238 File Offset: 0x00048438
		public override float Process(float value, InputControl control)
		{
			return NormalizeProcessor.Normalize(value, this.min, this.max, this.zero);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0004A254 File Offset: 0x00048454
		public static float Normalize(float value, float min, float max, float zero)
		{
			if (zero < min)
			{
				zero = min;
			}
			if (Mathf.Approximately(value, min))
			{
				if (min < zero)
				{
					return -1f;
				}
				return 0f;
			}
			else
			{
				float num = (value - min) / (max - min);
				if (min < zero)
				{
					return 2f * num - 1f;
				}
				return num;
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0004A29C File Offset: 0x0004849C
		internal static float Denormalize(float value, float min, float max, float zero)
		{
			if (zero < min)
			{
				zero = min;
			}
			if (min >= zero)
			{
				return min + (max - min) * value;
			}
			if (value < 0f)
			{
				return min + (zero - min) * (value * -1f);
			}
			return zero + (max - zero) * value;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0004A2CE File Offset: 0x000484CE
		public override string ToString()
		{
			return string.Format("Normalize(min={0},max={1},zero={2})", this.min, this.max, this.zero);
		}

		// Token: 0x04000651 RID: 1617
		public float min;

		// Token: 0x04000652 RID: 1618
		public float max;

		// Token: 0x04000653 RID: 1619
		public float zero;
	}
}
