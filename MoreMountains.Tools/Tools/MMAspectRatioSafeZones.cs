using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200001A RID: 26
	[AddComponentMenu("More Mountains/Tools/Camera/MMAspectRatioSafeZones")]
	public class MMAspectRatioSafeZones : MonoBehaviour
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00003278 File Offset: 0x00001478
		public virtual void AutoSetup()
		{
			this.Ratios.Clear();
			this.Ratios.Add(new Ratio(true, new Vector2(16f, 9f), MMColors.DeepSkyBlue));
			this.Ratios.Add(new Ratio(true, new Vector2(16f, 10f), MMColors.GreenYellow));
			this.Ratios.Add(new Ratio(true, new Vector2(4f, 3f), MMColors.HotPink));
		}

		// Token: 0x04000089 RID: 137
		[Header("Center")]
		public bool DrawCenterCrosshair = true;

		// Token: 0x0400008A RID: 138
		public float CenterCrosshairSize = 1f;

		// Token: 0x0400008B RID: 139
		public Color CenterCrosshairColor = MMColors.Wheat;

		// Token: 0x0400008C RID: 140
		[Header("Ratios")]
		public bool DrawRatios = true;

		// Token: 0x0400008D RID: 141
		public float CameraSize = 5f;

		// Token: 0x0400008E RID: 142
		public float UnsafeZonesOpacity = 0.2f;

		// Token: 0x0400008F RID: 143
		public List<Ratio> Ratios;

		// Token: 0x04000090 RID: 144
		[MMInspectorButton("AutoSetup")]
		public bool AutoSetupButton;
	}
}
