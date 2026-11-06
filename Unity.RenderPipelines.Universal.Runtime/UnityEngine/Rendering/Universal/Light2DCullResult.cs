using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000029 RID: 41
	internal class Light2DCullResult : ILight2DCullResult
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000C64F File Offset: 0x0000A84F
		public List<Light2D> visibleLights
		{
			get
			{
				return this.m_VisibleLights;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000C657 File Offset: 0x0000A857
		public List<ShadowCasterGroup2D> visibleShadows
		{
			get
			{
				return this.m_VisibleShadows;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000C65F File Offset: 0x0000A85F
		public bool IsSceneLit()
		{
			return Light2DManager.lights.Count > 0;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000C670 File Offset: 0x0000A870
		public LightStats GetLightStatsByLayer(int layer)
		{
			LightStats lightStats = default(LightStats);
			foreach (Light2D light2D in this.visibleLights)
			{
				if (light2D.IsLitLayer(layer))
				{
					lightStats.totalLights++;
					if (light2D.normalMapQuality != Light2D.NormalMapQuality.Disabled)
					{
						lightStats.totalNormalMapUsage++;
					}
					if (light2D.volumeIntensity > 0f)
					{
						lightStats.totalVolumetricUsage++;
					}
					lightStats.blendStylesUsed |= 1U << light2D.blendStyleIndex;
					if (light2D.lightType != Light2D.LightType.Global)
					{
						lightStats.blendStylesWithLights |= 1U << light2D.blendStyleIndex;
					}
				}
			}
			return lightStats;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000C744 File Offset: 0x0000A944
		public void SetupCulling(ref ScriptableCullingParameters cullingParameters, Camera camera)
		{
			this.m_VisibleLights.Clear();
			foreach (Light2D light2D in Light2DManager.lights)
			{
				if ((camera.cullingMask & (1 << light2D.gameObject.layer)) != 0)
				{
					if (light2D.lightType == Light2D.LightType.Global)
					{
						this.m_VisibleLights.Add(light2D);
					}
					else
					{
						Vector3 position = light2D.boundingSphere.position;
						bool flag = false;
						for (int i = 0; i < cullingParameters.cullingPlaneCount; i++)
						{
							Plane cullingPlane = cullingParameters.GetCullingPlane(i);
							if (math.dot(position, cullingPlane.normal) + cullingPlane.distance < -light2D.boundingSphere.radius)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.m_VisibleLights.Add(light2D);
						}
					}
				}
			}
			this.m_VisibleLights.Sort((Light2D l1, Light2D l2) => l1.lightOrder - l2.lightOrder);
			this.m_VisibleShadows.Clear();
			if (ShadowCasterGroup2DManager.shadowCasterGroups != null)
			{
				foreach (ShadowCasterGroup2D shadowCasterGroup2D in ShadowCasterGroup2DManager.shadowCasterGroups)
				{
					List<ShadowCaster2D> shadowCasters = shadowCasterGroup2D.GetShadowCasters();
					if (shadowCasters != null)
					{
						foreach (ShadowCaster2D shadowCaster2D in shadowCasters)
						{
							foreach (Light2D light2D2 in this.m_VisibleLights)
							{
								if (shadowCaster2D.IsLit(light2D2) && !this.m_VisibleShadows.Contains(shadowCasterGroup2D))
								{
									this.m_VisibleShadows.Add(shadowCasterGroup2D);
									break;
								}
							}
							if (this.m_VisibleShadows.Contains(shadowCasterGroup2D))
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x040000E5 RID: 229
		private List<Light2D> m_VisibleLights = new List<Light2D>();

		// Token: 0x040000E6 RID: 230
		private List<ShadowCasterGroup2D> m_VisibleShadows = new List<ShadowCasterGroup2D>();
	}
}
