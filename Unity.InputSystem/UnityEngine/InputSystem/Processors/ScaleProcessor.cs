using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000109 RID: 265
	public class ScaleProcessor : InputProcessor<float>
	{
		// Token: 0x06000F2E RID: 3886 RVA: 0x0004A333 File Offset: 0x00048533
		public override float Process(float value, InputControl control)
		{
			return value * this.factor;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0004A33D File Offset: 0x0004853D
		public override string ToString()
		{
			return string.Format("Scale(factor={0})", this.factor);
		}

		// Token: 0x04000654 RID: 1620
		[Tooltip("Scale factor to multiply incoming float values by.")]
		public float factor = 1f;
	}
}
