using System;
using DA_Assets.DAI;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000373 RID: 883
	[Serializable]
	public class BaseImageSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x00096931 File Offset: 0x00094B31
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x00096939 File Offset: 0x00094B39
		[FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_image_type, FcuLocKey.tooltip_image_type, 0f, 1f)]
		public Image.Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				base.SetValue<Image.Type>(ref this.type, value);
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00096948 File Offset: 0x00094B48
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x00096950 File Offset: 0x00094B50
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

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x0009695F File Offset: 0x00094B5F
		// (set) Token: 0x06001700 RID: 5888 RVA: 0x00096967 File Offset: 0x00094B67
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

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x00096976 File Offset: 0x00094B76
		// (set) Token: 0x06001702 RID: 5890 RVA: 0x0009697E File Offset: 0x00094B7E
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

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x0009698D File Offset: 0x00094B8D
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x00096995 File Offset: 0x00094B95
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_maskable, FcuLocKey.tooltip_maskable, 0f, 1f)]
		public bool Maskable
		{
			get
			{
				return this.maskable;
			}
			set
			{
				base.SetValue<bool>(ref this.maskable, value);
			}
		}

		// Token: 0x040018C4 RID: 6340
		[SerializeField]
		private Image.Type type;

		// Token: 0x040018C5 RID: 6341
		[SerializeField]
		private bool raycastTarget = true;

		// Token: 0x040018C6 RID: 6342
		[SerializeField]
		private bool preserveAspect = true;

		// Token: 0x040018C7 RID: 6343
		[SerializeField]
		private Vector4 raycastPadding = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x040018C8 RID: 6344
		[SerializeField]
		private bool maskable = true;
	}
}
