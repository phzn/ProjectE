using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200002A RID: 42
	internal static class Light2DManager
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000C99E File Offset: 0x0000AB9E
		public static List<Light2D> lights { get; } = new List<Light2D>();

		// Token: 0x0600017E RID: 382 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
		internal static void Initialize()
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000C9A7 File Offset: 0x0000ABA7
		internal static void Dispose()
		{
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000C9A9 File Offset: 0x0000ABA9
		public static void RegisterLight(Light2D light)
		{
			Light2DManager.lights.Add(light);
			Light2DManager.ErrorIfDuplicateGlobalLight(light);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000C9BC File Offset: 0x0000ABBC
		public static void DeregisterLight(Light2D light)
		{
			Light2DManager.lights.Remove(light);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		public static void ErrorIfDuplicateGlobalLight(Light2D light)
		{
			if (light.lightType != Light2D.LightType.Global)
			{
				return;
			}
			foreach (int num in light.affectedSortingLayers)
			{
				if (Light2DManager.ContainsDuplicateGlobalLight(num, light.blendStyleIndex))
				{
					Debug.LogError("More than one global light on layer " + SortingLayer.IDToName(num) + " for light blend style index " + light.blendStyleIndex.ToString());
				}
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000CA34 File Offset: 0x0000AC34
		public static bool GetGlobalColor(int sortingLayerIndex, int blendStyleIndex, out Color color)
		{
			bool flag = false;
			color = Color.black;
			foreach (Light2D light2D in Light2DManager.lights)
			{
				if (light2D.lightType == Light2D.LightType.Global && light2D.blendStyleIndex == blendStyleIndex && light2D.IsLitLayer(sortingLayerIndex))
				{
					if (true)
					{
						color = light2D.color * light2D.intensity;
						return true;
					}
					if (!flag)
					{
						color = light2D.color * light2D.intensity;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		private static bool ContainsDuplicateGlobalLight(int sortingLayerIndex, int blendStyleIndex)
		{
			int num = 0;
			foreach (Light2D light2D in Light2DManager.lights)
			{
				if (light2D.lightType == Light2D.LightType.Global && light2D.blendStyleIndex == blendStyleIndex && light2D.IsLitLayer(sortingLayerIndex))
				{
					if (num > 0)
					{
						return true;
					}
					num++;
				}
			}
			return false;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000CB5C File Offset: 0x0000AD5C
		public static SortingLayer[] GetCachedSortingLayer()
		{
			if (Light2DManager.s_SortingLayers == null)
			{
				Light2DManager.s_SortingLayers = SortingLayer.layers;
			}
			return Light2DManager.s_SortingLayers;
		}

		// Token: 0x040000E7 RID: 231
		private static SortingLayer[] s_SortingLayers;
	}
}
