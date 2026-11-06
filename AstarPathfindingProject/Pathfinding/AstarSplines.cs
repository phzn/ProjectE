using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200004F RID: 79
	public static class AstarSplines
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0000D250 File Offset: 0x0000B450
		public static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
		{
			float num = elapsedTime * elapsedTime;
			float num2 = num * elapsedTime;
			return previous * (-0.5f * num2 + num - 0.5f * elapsedTime) + start * (1.5f * num2 + -2.5f * num + 1f) + end * (-1.5f * num2 + 2f * num + 0.5f * elapsedTime) + next * (0.5f * num2 - 0.5f * num);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000D2DC File Offset: 0x0000B4DC
		public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000D348 File Offset: 0x0000B548
		public static Vector3 CubicBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return 3f * num * num * (p1 - p0) + 6f * num * t * (p2 - p1) + 3f * t * t * (p3 - p2);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		public static Vector3 CubicBezierSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return 6f * num * (p2 - 2f * p1 + p0) + 6f * t * (p3 - 2f * p2 + p1);
		}
	}
}
