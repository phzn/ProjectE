using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x020000FF RID: 255
	public class AxisDeadzoneProcessor : InputProcessor<float>
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x00049ED1 File Offset: 0x000480D1
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

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00049EF1 File Offset: 0x000480F1
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

		// Token: 0x06000F0C RID: 3852 RVA: 0x00049F14 File Offset: 0x00048114
		public override float Process(float value, InputControl control = null)
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

		// Token: 0x06000F0D RID: 3853 RVA: 0x00049F59 File Offset: 0x00048159
		public override string ToString()
		{
			return string.Format("AxisDeadzone(min={0},max={1})", this.minOrDefault, this.maxOrDefault);
		}

		// Token: 0x04000648 RID: 1608
		public float min;

		// Token: 0x04000649 RID: 1609
		public float max;
	}
}
