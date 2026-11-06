using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000378 RID: 888
	[FcuPropertyHeader(FcuLocKey.label_pui_settings, FcuLocKey.tooltip_pui_settings)]
	[Serializable]
	public class JoshPuiSettings : BaseImageSettings
	{
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600172B RID: 5931 RVA: 0x00096D96 File Offset: 0x00094F96
		// (set) Token: 0x0600172C RID: 5932 RVA: 0x00096D9E File Offset: 0x00094F9E
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

		// Token: 0x040018DF RID: 6367
		[SerializeField]
		private float falloffDistance = 1f;
	}
}
