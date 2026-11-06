using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000108 RID: 264
	public class NormalizeVector3Processor : InputProcessor<Vector3>
	{
		// Token: 0x06000F2B RID: 3883 RVA: 0x0004A31B File Offset: 0x0004851B
		public override Vector3 Process(Vector3 value, InputControl control)
		{
			return value.normalized;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0004A324 File Offset: 0x00048524
		public override string ToString()
		{
			return "NormalizeVector3()";
		}
	}
}
