using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x0200010C RID: 268
	public class StickDeadzoneProcessor : InputProcessor<Vector2>
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0004A44C File Offset: 0x0004864C
		private float minOrDefault
		{
			get
			{
				if (this.min != 0f)
				{
					return this.min;
				}
				return InputSystem.settings.defaultDeadzoneMin;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0004A46C File Offset: 0x0004866C
		private float maxOrDefault
		{
			get
			{
				if (this.max != 0f)
				{
					return this.max;
				}
				return InputSystem.settings.defaultDeadzoneMax;
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0004A48C File Offset: 0x0004868C
		public override Vector2 Process(Vector2 value, InputControl control = null)
		{
			float magnitude = value.magnitude;
			float deadZoneAdjustedValue = this.GetDeadZoneAdjustedValue(magnitude);
			if (deadZoneAdjustedValue == 0f)
			{
				value = Vector2.zero;
			}
			else
			{
				value *= deadZoneAdjustedValue / magnitude;
			}
			return value;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0004A4C8 File Offset: 0x000486C8
		private float GetDeadZoneAdjustedValue(float value)
		{
			float minOrDefault = this.minOrDefault;
			float maxOrDefault = this.maxOrDefault;
			float num = Mathf.Abs(value);
			if (num < minOrDefault)
			{
				return 0f;
			}
			if (num > maxOrDefault)
			{
				return Mathf.Sign(value);
			}
			return Mathf.Sign(value) * ((num - minOrDefault) / (maxOrDefault - minOrDefault));
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0004A50D File Offset: 0x0004870D
		public override string ToString()
		{
			return string.Format("StickDeadzone(min={0},max={1})", this.minOrDefault, this.maxOrDefault);
		}

		// Token: 0x0400065A RID: 1626
		public float min;

		// Token: 0x0400065B RID: 1627
		public float max;
	}
}
