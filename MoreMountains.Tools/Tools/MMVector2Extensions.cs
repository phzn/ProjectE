using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F1 RID: 241
	public static class MMVector2Extensions
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x00017200 File Offset: 0x00015400
		public static Vector2 MMRotate(this Vector2 vector, float angleInDegrees)
		{
			float num = Mathf.Sin(angleInDegrees * 0.017453292f);
			float num2 = Mathf.Cos(angleInDegrees * 0.017453292f);
			float x = vector.x;
			float y = vector.y;
			vector.x = num2 * x - num * y;
			vector.y = num * x + num2 * y;
			return vector;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00017252 File Offset: 0x00015452
		public static Vector2 MMSetX(this Vector2 vector, float newValue)
		{
			vector.x = newValue;
			return vector;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001725D File Offset: 0x0001545D
		public static Vector2 MMSetY(this Vector2 vector, float newValue)
		{
			vector.y = newValue;
			return vector;
		}
	}
}
