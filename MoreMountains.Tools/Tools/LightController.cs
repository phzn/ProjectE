using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000115 RID: 277
	[AddComponentMenu("More Mountains/Tools/Property Controllers/LightController")]
	public class LightController : MonoBehaviour
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0001E400 File Offset: 0x0001C600
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001E408 File Offset: 0x0001C608
		protected virtual void Initialization()
		{
			if (this.TargetLight == null)
			{
				this.TargetLight = base.gameObject.GetComponent<Light>();
			}
			if (this.TargetLight != null)
			{
				this.TargetLight.range = this.Range;
				this.TargetLight.color = this.LightColor;
			}
			if (this.TargetLights.Count > 0)
			{
				foreach (Light light in this.TargetLights)
				{
					if (light != null)
					{
						light.range = this.Range;
						light.color = this.LightColor;
					}
				}
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001E4D4 File Offset: 0x0001C6D4
		protected virtual void Update()
		{
			this.ApplyLightSettings();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001E4DC File Offset: 0x0001C6DC
		protected virtual void ApplyLightSettings()
		{
			if (this.TargetLight != null)
			{
				this.TargetLight.intensity = this.Intensity * this.Multiplier;
				this.TargetLight.range = this.Range;
				this.TargetLight.color = this.LightColor;
			}
			if (this.TargetLights.Count > 0)
			{
				foreach (Light light in this.TargetLights)
				{
					if (light != null)
					{
						light.intensity = this.Intensity * this.Multiplier;
						light.range = this.Range;
						light.color = this.LightColor;
					}
				}
			}
		}

		// Token: 0x04000767 RID: 1895
		[Header("Binding")]
		[MMInformation("Use this component to control the properties of one or more lights at runtime. Plays well with a FloatController. This component will try to auto set the TargetLight if there's a Light component on this object.", MMInformationAttribute.InformationType.Info, false)]
		public Light TargetLight;

		// Token: 0x04000768 RID: 1896
		public List<Light> TargetLights;

		// Token: 0x04000769 RID: 1897
		[Header("Light Settings")]
		public float Intensity = 1f;

		// Token: 0x0400076A RID: 1898
		public float Multiplier = 1f;

		// Token: 0x0400076B RID: 1899
		public float Range = 1f;

		// Token: 0x0400076C RID: 1900
		[Header("Color")]
		public Color LightColor;
	}
}
