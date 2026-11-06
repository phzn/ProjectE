using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200037D RID: 893
	[FcuPropertyHeader(FcuLocKey.label_svg_image_settings, FcuLocKey.tooltip_svg_image_settings)]
	[Serializable]
	public class SvgImageSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00096E97 File Offset: 0x00095097
		// (set) Token: 0x06001741 RID: 5953 RVA: 0x00096E9F File Offset: 0x0009509F
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_raycast_target, FcuLocKey.tooltip_raycast_target, 0f, 1f)]
		public bool RaycastTarget
		{
			get
			{
				return this.raycastTarget;
			}
			set
			{
				base.SetValue<bool>(ref this.raycastTarget, value);
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00096EAE File Offset: 0x000950AE
		// (set) Token: 0x06001743 RID: 5955 RVA: 0x00096EB6 File Offset: 0x000950B6
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_preserve_aspect, FcuLocKey.tooltip_preserve_aspect, 0f, 1f)]
		public bool PreserveAspect
		{
			get
			{
				return this.preserveAspect;
			}
			set
			{
				base.SetValue<bool>(ref this.preserveAspect, value);
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00096EC5 File Offset: 0x000950C5
		// (set) Token: 0x06001745 RID: 5957 RVA: 0x00096ECD File Offset: 0x000950CD
		[FcuInspectorProperty(ComponentType.Vector4Field, FcuLocKey.label_raycast_padding, FcuLocKey.tooltip_raycast_padding, 0f, 1f)]
		public Vector4 RaycastPadding
		{
			get
			{
				return this.raycastPadding;
			}
			set
			{
				base.SetValue<Vector4>(ref this.raycastPadding, value);
			}
		}

		// Token: 0x040018E7 RID: 6375
		[SerializeField]
		private bool raycastTarget = true;

		// Token: 0x040018E8 RID: 6376
		[SerializeField]
		private bool preserveAspect = true;

		// Token: 0x040018E9 RID: 6377
		[SerializeField]
		private Vector4 raycastPadding = new Vector4(0f, 0f, 0f, 0f);
	}
}
