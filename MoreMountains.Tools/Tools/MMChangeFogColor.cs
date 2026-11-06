using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200006F RID: 111
	[ExecuteAlways]
	[AddComponentMenu("More Mountains/Tools/Particles/MMChangeFogColor")]
	public class MMChangeFogColor : MonoBehaviour
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0000CA65 File Offset: 0x0000AC65
		protected virtual void SetupFogColor()
		{
			RenderSettings.fogColor = this.FogColor;
			RenderSettings.fog = true;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000CA78 File Offset: 0x0000AC78
		protected virtual void Start()
		{
			this.SetupFogColor();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000CA80 File Offset: 0x0000AC80
		protected virtual void OnValidate()
		{
			this.SetupFogColor();
		}

		// Token: 0x040003C7 RID: 967
		[MMInformation("Adds this class to a UnityStandardAssets.ImageEffects.GlobalFog to change its color", MMInformationAttribute.InformationType.Info, false)]
		public Color FogColor;
	}
}
