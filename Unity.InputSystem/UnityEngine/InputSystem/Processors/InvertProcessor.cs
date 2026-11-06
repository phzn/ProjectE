using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000103 RID: 259
	public class InvertProcessor : InputProcessor<float>
	{
		// Token: 0x06000F1A RID: 3866 RVA: 0x0004A114 File Offset: 0x00048314
		public override float Process(float value, InputControl control)
		{
			return value * -1f;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0004A11D File Offset: 0x0004831D
		public override string ToString()
		{
			return "Invert()";
		}
	}
}
