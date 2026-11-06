using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000107 RID: 263
	public class NormalizeVector2Processor : InputProcessor<Vector2>
	{
		// Token: 0x06000F28 RID: 3880 RVA: 0x0004A303 File Offset: 0x00048503
		public override Vector2 Process(Vector2 value, InputControl control)
		{
			return value.normalized;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0004A30C File Offset: 0x0004850C
		public override string ToString()
		{
			return "NormalizeVector2()";
		}
	}
}
