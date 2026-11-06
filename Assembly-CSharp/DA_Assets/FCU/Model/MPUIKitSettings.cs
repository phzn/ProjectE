using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000379 RID: 889
	[FcuPropertyHeader(FcuLocKey.label_mpuikit_settings, FcuLocKey.tooltip_mpuikit_settings)]
	[Serializable]
	public class MPUIKitSettings : BaseImageSettings
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x00096DC0 File Offset: 0x00094FC0
		// (set) Token: 0x0600172F RID: 5935 RVA: 0x00096DC8 File Offset: 0x00094FC8
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_pui_falloff_distance, FcuLocKey.tooltip_pui_falloff_distance, 0f, 1f)]
		public float FalloffDistance
		{
			get
			{
				return this.falloffDistance;
			}
			set
			{
				base.SetValue<float>(ref this.falloffDistance, value);
			}
		}

		// Token: 0x040018E0 RID: 6368
		[SerializeField]
		private float falloffDistance = 0.5f;
	}
}
