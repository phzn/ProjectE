using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000374 RID: 884
	[FcuPropertyHeader(FcuLocKey.label_procedural_ui_settings, FcuLocKey.tooltip_procedural_ui_settings)]
	[Serializable]
	public class DttPuiSettings : BaseImageSettings
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x000969E0 File Offset: 0x00094BE0
		// (set) Token: 0x06001707 RID: 5895 RVA: 0x000969E8 File Offset: 0x00094BE8
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

		// Token: 0x040018C9 RID: 6345
		[SerializeField]
		private float falloffDistance = 1f;
	}
}
