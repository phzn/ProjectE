using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000135 RID: 309
	public class Toggle : BaseBoolField
	{
		// Token: 0x06000A3E RID: 2622 RVA: 0x00028E94 File Offset: 0x00027094
		public Toggle()
			: this(null)
		{
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00028EA0 File Offset: 0x000270A0
		public Toggle(string label)
			: base(label)
		{
			base.AddToClassList(Toggle.ussClassName);
			base.visualInput.AddToClassList(Toggle.inputUssClassName);
			base.labelElement.AddToClassList(Toggle.labelUssClassName);
			this.m_CheckMark.AddToClassList(Toggle.checkmarkUssClassName);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00028EF5 File Offset: 0x000270F5
		protected override void InitLabel()
		{
			base.InitLabel();
			this.m_Label.AddToClassList(Toggle.textUssClassName);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00028F10 File Offset: 0x00027110
		protected override void UpdateMixedValueContent()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				base.visualInput.pseudoStates &= ~PseudoStates.Checked;
				base.pseudoStates &= ~PseudoStates.Checked;
				this.m_CheckMark.AddToClassList(Toggle.mixedValuesUssClassName);
			}
			else
			{
				this.m_CheckMark.RemoveFromClassList(Toggle.mixedValuesUssClassName);
				bool value = this.value;
				if (value)
				{
					base.visualInput.pseudoStates |= PseudoStates.Checked;
					base.pseudoStates |= PseudoStates.Checked;
				}
				else
				{
					base.visualInput.pseudoStates &= ~PseudoStates.Checked;
					base.pseudoStates &= ~PseudoStates.Checked;
				}
			}
		}

		// Token: 0x040004E4 RID: 1252
		public new static readonly string ussClassName = "unity-toggle";

		// Token: 0x040004E5 RID: 1253
		public new static readonly string labelUssClassName = Toggle.ussClassName + "__label";

		// Token: 0x040004E6 RID: 1254
		public new static readonly string inputUssClassName = Toggle.ussClassName + "__input";

		// Token: 0x040004E7 RID: 1255
		[Obsolete]
		public static readonly string noTextVariantUssClassName = Toggle.ussClassName + "--no-text";

		// Token: 0x040004E8 RID: 1256
		public static readonly string checkmarkUssClassName = Toggle.ussClassName + "__checkmark";

		// Token: 0x040004E9 RID: 1257
		public static readonly string textUssClassName = Toggle.ussClassName + "__text";

		// Token: 0x040004EA RID: 1258
		public static readonly string mixedValuesUssClassName = Toggle.ussClassName + "__mixed-values";

		// Token: 0x02000136 RID: 310
		public new class UxmlFactory : UxmlFactory<Toggle, Toggle.UxmlTraits>
		{
		}

		// Token: 0x02000137 RID: 311
		public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription>
		{
			// Token: 0x06000A44 RID: 2628 RVA: 0x00029068 File Offset: 0x00027268
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				((Toggle)ve).text = this.m_Text.GetValueFromBag(bag, cc);
			}

			// Token: 0x040004EB RID: 1259
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};
		}
	}
}
