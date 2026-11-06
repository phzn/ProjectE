using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000104 RID: 260
	public class InvertVector2Processor : InputProcessor<Vector2>
	{
		// Token: 0x06000F1D RID: 3869 RVA: 0x0004A12C File Offset: 0x0004832C
		public override Vector2 Process(Vector2 value, InputControl control)
		{
			if (this.invertX)
			{
				value.x *= -1f;
			}
			if (this.invertY)
			{
				value.y *= -1f;
			}
			return value;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0004A15F File Offset: 0x0004835F
		public override string ToString()
		{
			return string.Format("InvertVector2(invertX={0},invertY={1})", this.invertX, this.invertY);
		}

		// Token: 0x0400064C RID: 1612
		public bool invertX = true;

		// Token: 0x0400064D RID: 1613
		public bool invertY = true;
	}
}
