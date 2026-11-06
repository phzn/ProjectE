using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200036F RID: 879
	[Serializable]
	public class ButtonSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x000966F2 File Offset: 0x000948F2
		// (set) Token: 0x060016DC RID: 5852 RVA: 0x000966FA File Offset: 0x000948FA
		[FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_button_type, FcuLocKey.tooltip_button_type, 0f, 1f)]
		public ButtonComponent ButtonComponent
		{
			get
			{
				return this.buttonComponent;
			}
			set
			{
				base.SetValue<ButtonComponent>(ref this.buttonComponent, value);
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00096709 File Offset: 0x00094909
		[SerializeProperty("unityButtonSettings")]
		public UnityButtonSettings UnityButtonSettings
		{
			get
			{
				return this.monoBeh.Link(ref this.unityButtonSettings);
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x0009671C File Offset: 0x0009491C
		// (set) Token: 0x060016DF RID: 5855 RVA: 0x00096724 File Offset: 0x00094924
		[SerializeProperty("transition")]
		public ButtonTransitionType TransitionType { get; set; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x0009672D File Offset: 0x0009492D
		[SerializeProperty("dabSettings")]
		public DAB_Settings DAB_Settings
		{
			get
			{
				return this.monoBeh.Link(ref this.dabSettings);
			}
		}

		// Token: 0x040018B1 RID: 6321
		[SerializeField]
		private ButtonComponent buttonComponent;

		// Token: 0x040018B2 RID: 6322
		[SerializeField]
		private UnityButtonSettings unityButtonSettings;

		// Token: 0x040018B3 RID: 6323
		[SerializeField]
		private ButtonTransitionType transition;

		// Token: 0x040018B5 RID: 6325
		[SerializeField]
		private DAB_Settings dabSettings;
	}
}
