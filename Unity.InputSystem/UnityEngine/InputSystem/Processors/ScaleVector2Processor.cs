using System;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x0200010A RID: 266
	public class ScaleVector2Processor : InputProcessor<Vector2>
	{
		// Token: 0x06000F31 RID: 3889 RVA: 0x0004A367 File Offset: 0x00048567
		public override Vector2 Process(Vector2 value, InputControl control)
		{
			return new Vector2(value.x * this.x, value.y * this.y);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0004A388 File Offset: 0x00048588
		public override string ToString()
		{
			return string.Format("ScaleVector2(x={0},y={1})", this.x, this.y);
		}

		// Token: 0x04000655 RID: 1621
		[Tooltip("Scale factor to multiply the incoming Vector2's X component by.")]
		public float x = 1f;

		// Token: 0x04000656 RID: 1622
		[Tooltip("Scale factor to multiply the incoming Vector2's Y component by.")]
		public float y = 1f;
	}
}
