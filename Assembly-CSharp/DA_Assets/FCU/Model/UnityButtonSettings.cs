using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000372 RID: 882
	[FcuPropertyHeader(FcuLocKey.label_button_settings, FcuLocKey.tooltip_button_settings)]
	[Serializable]
	public class UnityButtonSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060016EC RID: 5868 RVA: 0x000967B1 File Offset: 0x000949B1
		// (set) Token: 0x060016ED RID: 5869 RVA: 0x000967B9 File Offset: 0x000949B9
		[FcuInspectorProperty(ComponentType.ColorField, FcuLocKey.label_normal_color, FcuLocKey.tooltip_normal_color, 0f, 1f)]
		public Color NormalColor
		{
			get
			{
				return this.normalColor;
			}
			set
			{
				base.SetValue<Color>(ref this.normalColor, value);
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x000967C8 File Offset: 0x000949C8
		// (set) Token: 0x060016EF RID: 5871 RVA: 0x000967D0 File Offset: 0x000949D0
		[FcuInspectorProperty(ComponentType.ColorField, FcuLocKey.label_highlighted_color, FcuLocKey.tooltip_highlighted_color, 0f, 1f)]
		public Color HighlightedColor
		{
			get
			{
				return this.highlightedColor;
			}
			set
			{
				base.SetValue<Color>(ref this.highlightedColor, value);
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x000967DF File Offset: 0x000949DF
		// (set) Token: 0x060016F1 RID: 5873 RVA: 0x000967E7 File Offset: 0x000949E7
		[FcuInspectorProperty(ComponentType.ColorField, FcuLocKey.label_pressed_color, FcuLocKey.tooltip_pressed_color, 0f, 1f)]
		public Color PressedColor
		{
			get
			{
				return this.pressedColor;
			}
			set
			{
				base.SetValue<Color>(ref this.pressedColor, value);
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x000967F6 File Offset: 0x000949F6
		// (set) Token: 0x060016F3 RID: 5875 RVA: 0x000967FE File Offset: 0x000949FE
		[FcuInspectorProperty(ComponentType.ColorField, FcuLocKey.label_selected_color, FcuLocKey.tooltip_selected_color, 0f, 1f)]
		public Color SelectedColor
		{
			get
			{
				return this.selectedColor;
			}
			set
			{
				base.SetValue<Color>(ref this.selectedColor, value);
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0009680D File Offset: 0x00094A0D
		// (set) Token: 0x060016F5 RID: 5877 RVA: 0x00096815 File Offset: 0x00094A15
		[FcuInspectorProperty(ComponentType.ColorField, FcuLocKey.label_disabled_color, FcuLocKey.tooltip_disabled_color, 0f, 1f)]
		public Color DisabledColor
		{
			get
			{
				return this.disabledColor;
			}
			set
			{
				base.SetValue<Color>(ref this.disabledColor, value);
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060016F6 RID: 5878 RVA: 0x00096824 File Offset: 0x00094A24
		// (set) Token: 0x060016F7 RID: 5879 RVA: 0x0009682C File Offset: 0x00094A2C
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_color_multiplier, FcuLocKey.tooltip_color_multiplier, 0f, 1f)]
		public float ColorMultiplier
		{
			get
			{
				return this.colorMultiplier;
			}
			set
			{
				base.SetValue<float>(ref this.colorMultiplier, value);
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x0009683B File Offset: 0x00094A3B
		// (set) Token: 0x060016F9 RID: 5881 RVA: 0x00096843 File Offset: 0x00094A43
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_fade_duration, FcuLocKey.tooltip_fade_duration, 0f, 1f)]
		public float FadeDuration
		{
			get
			{
				return this.fadeDuration;
			}
			set
			{
				base.SetValue<float>(ref this.fadeDuration, value);
			}
		}

		// Token: 0x040018BD RID: 6333
		[SerializeField]
		private Color normalColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x040018BE RID: 6334
		[SerializeField]
		private Color highlightedColor = new Color32(245, 245, 245, byte.MaxValue);

		// Token: 0x040018BF RID: 6335
		[SerializeField]
		private Color pressedColor = new Color32(200, 200, 200, byte.MaxValue);

		// Token: 0x040018C0 RID: 6336
		[SerializeField]
		private Color selectedColor = new Color32(245, 245, 245, byte.MaxValue);

		// Token: 0x040018C1 RID: 6337
		[SerializeField]
		private Color disabledColor = new Color32(200, 200, 200, 128);

		// Token: 0x040018C2 RID: 6338
		[SerializeField]
		private float colorMultiplier = 1f;

		// Token: 0x040018C3 RID: 6339
		[SerializeField]
		private float fadeDuration = 0.1f;
	}
}
