using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000105 RID: 261
	public class InvertVector3Processor : InputProcessor<Vector3>
	{
		// Token: 0x06000F20 RID: 3872 RVA: 0x0004A198 File Offset: 0x00048398
		public override Vector3 Process(Vector3 value, InputControl control)
		{
			if (this.invertX)
			{
				value.x *= -1f;
			}
			if (this.invertY)
			{
				value.y *= -1f;
			}
			if (this.invertZ)
			{
				value.z *= -1f;
			}
			return value;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0004A1EE File Offset: 0x000483EE
		public override string ToString()
		{
			return string.Format("InvertVector3(invertX={0},invertY={1},invertZ={2})", this.invertX, this.invertY, this.invertZ);
		}

		// Token: 0x0400064E RID: 1614
		public bool invertX = true;

		// Token: 0x0400064F RID: 1615
		public bool invertY = true;

		// Token: 0x04000650 RID: 1616
		public bool invertZ = true;
	}
}
