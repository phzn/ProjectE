using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000ED RID: 237
	public static class RendererExtensions
	{
		// Token: 0x06000500 RID: 1280 RVA: 0x00016F90 File Offset: 0x00015190
		public static bool MMIsVisibleFrom(this Renderer renderer, Camera camera)
		{
			return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
		}
	}
}
