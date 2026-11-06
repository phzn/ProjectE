using System;
using Unity.Mathematics;

namespace Pathfinding.PID
{
	// Token: 0x02000263 RID: 611
	public struct AnglePIDControlOutput2D
	{
		// Token: 0x06000F1B RID: 3867 RVA: 0x0005E0C0 File Offset: 0x0005C2C0
		public AnglePIDControlOutput2D(float currentRotation, float targetRotation, float rotationDelta, float moveDistance)
		{
			float num;
			float num2;
			math.sincos(currentRotation + rotationDelta * 0.5f, out num, out num2);
			this.rotationDelta = rotationDelta;
			this.positionDelta = new float2(num2, num) * moveDistance;
			this.targetRotation = targetRotation;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0005E104 File Offset: 0x0005C304
		public static AnglePIDControlOutput2D WithMovementAtEnd(float currentRotation, float targetRotation, float rotationDelta, float moveDistance)
		{
			float num;
			float num2;
			math.sincos(currentRotation + rotationDelta, out num, out num2);
			return new AnglePIDControlOutput2D
			{
				rotationDelta = rotationDelta,
				targetRotation = targetRotation,
				positionDelta = new float2(num2, num) * moveDistance
			};
		}

		// Token: 0x04000B62 RID: 2914
		public float rotationDelta;

		// Token: 0x04000B63 RID: 2915
		public float targetRotation;

		// Token: 0x04000B64 RID: 2916
		public float2 positionDelta;
	}
}
