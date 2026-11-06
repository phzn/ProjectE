using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x0200019D RID: 413
	public static class CameraUtility
	{
		// Token: 0x06000CC5 RID: 3269 RVA: 0x00057660 File Offset: 0x00055860
		public static bool IsOnScreen(Vector3 position)
		{
			Vector3 vector = Camera.current.WorldToViewportPoint(position);
			return vector.z > 0f && vector.x > 0f && vector.y > 0f && vector.x < 1f && vector.y < 1f;
		}
	}
}
