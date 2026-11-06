using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000100 RID: 256
	public class ClampProcessor : InputProcessor<float>
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x00049F83 File Offset: 0x00048183
		public override float Process(float value, InputControl control)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00049F97 File Offset: 0x00048197
		public override string ToString()
		{
			return string.Format("Clamp(min={0},max={1})", this.min, this.max);
		}

		// Token: 0x0400064A RID: 1610
		public float min;

		// Token: 0x0400064B RID: 1611
		public float max;
	}
}
