using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200038A RID: 906
	[FcuPropertyHeader(FcuLocKey.label_unity_text_settings, FcuLocKey.tooltip_unity_text_settings)]
	[Serializable]
	public class UnityTextSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0009778A File Offset: 0x0009598A
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x00097792 File Offset: 0x00095992
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_best_fit, FcuLocKey.tooltip_best_fit, 0f, 1f)]
		public bool BestFit
		{
			get
			{
				return this.bestFit;
			}
			set
			{
				base.SetValue<bool>(ref this.bestFit, value);
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x000977A1 File Offset: 0x000959A1
		// (set) Token: 0x060017C4 RID: 6084 RVA: 0x000977A9 File Offset: 0x000959A9
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_line_spacing, FcuLocKey.tooltip_line_spacing, 0f, 1f)]
		public float FontLineSpacing
		{
			get
			{
				return this.fontLineSpacing;
			}
			set
			{
				base.SetValue<float>(ref this.fontLineSpacing, value);
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x000977B8 File Offset: 0x000959B8
		// (set) Token: 0x060017C6 RID: 6086 RVA: 0x000977C0 File Offset: 0x000959C0
		[FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_horizontal_overflow, FcuLocKey.tooltip_horizontal_overflow, 0f, 1f)]
		public HorizontalWrapMode HorizontalWrapMode
		{
			get
			{
				return this.horizontalWrapMode;
			}
			set
			{
				base.SetValue<HorizontalWrapMode>(ref this.horizontalWrapMode, value);
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060017C7 RID: 6087 RVA: 0x000977CF File Offset: 0x000959CF
		// (set) Token: 0x060017C8 RID: 6088 RVA: 0x000977D7 File Offset: 0x000959D7
		[FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_vertical_overflow, FcuLocKey.tooltip_vertical_overflow, 0f, 1f)]
		public VerticalWrapMode VerticalWrapMode
		{
			get
			{
				return this.verticalWrapMode;
			}
			set
			{
				base.SetValue<VerticalWrapMode>(ref this.verticalWrapMode, value);
			}
		}

		// Token: 0x0400192A RID: 6442
		[SerializeField]
		private bool bestFit = true;

		// Token: 0x0400192B RID: 6443
		[SerializeField]
		private float fontLineSpacing = 1f;

		// Token: 0x0400192C RID: 6444
		[SerializeField]
		private HorizontalWrapMode horizontalWrapMode;

		// Token: 0x0400192D RID: 6445
		[SerializeField]
		private VerticalWrapMode verticalWrapMode;
	}
}
