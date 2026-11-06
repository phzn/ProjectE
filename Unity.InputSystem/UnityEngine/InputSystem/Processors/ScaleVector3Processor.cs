using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x0200010B RID: 267
	public class ScaleVector3Processor : InputProcessor<Vector3>
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x0004A3C8 File Offset: 0x000485C8
		public override Vector3 Process(Vector3 value, InputControl control)
		{
			return new Vector3(value.x * this.x, value.y * this.y, value.z * this.z);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0004A3F6 File Offset: 0x000485F6
		public override string ToString()
		{
			return string.Format("ScaleVector3(x={0},y={1},z={2})", this.x, this.y, this.z);
		}

		// Token: 0x04000657 RID: 1623
		[Tooltip("Scale factor to multiply the incoming Vector3's X component by.")]
		public float x = 1f;

		// Token: 0x04000658 RID: 1624
		[Tooltip("Scale factor to multiply the incoming Vector3's Y component by.")]
		public float y = 1f;

		// Token: 0x04000659 RID: 1625
		[Tooltip("Scale factor to multiply the incoming Vector3's Z component by.")]
		public float z = 1f;
	}
}
