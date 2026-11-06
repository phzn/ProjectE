using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E4 RID: 228
	public static class MMCameraExtensions
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x00016B5C File Offset: 0x00014D5C
		public static float MMCameraWorldSpaceWidth(this Camera camera, float depth = 0f)
		{
			if (camera.orthographic)
			{
				return camera.aspect * camera.orthographicSize * 2f;
			}
			float num = camera.fieldOfView * 0.017453292f;
			return camera.aspect * depth * Mathf.Tan(num);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00016BA4 File Offset: 0x00014DA4
		public static float MMCameraWorldSpaceHeight(this Camera camera, float depth = 0f)
		{
			if (camera.orthographic)
			{
				return camera.orthographicSize * 2f;
			}
			float num = camera.fieldOfView * 0.017453292f;
			return depth * Mathf.Tan(num);
		}
	}
}
