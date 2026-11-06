using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000D2 RID: 210
	public class TextField : TextInputBaseField<string>
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001B197 File Offset: 0x00019397
		private TextField.TextInput textInput
		{
			get
			{
				return (TextField.TextInput)base.textInputBase;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001B1A4 File Offset: 0x000193A4
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001B1C1 File Offset: 0x000193C1
		public bool multiline
		{
			get
			{
				return this.textInput.multiline;
			}
			set
			{
				this.textInput.multiline = value;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001B1D1 File Offset: 0x000193D1
		public TextField()
			: this(null)
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001B1DC File Offset: 0x000193DC
		public TextField(int maxLength, bool multiline, bool isPasswordField, char maskChar)
			: this(null, maxLength, multiline, isPasswordField, maskChar)
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001B1EC File Offset: 0x000193EC
		public TextField(string label)
			: this(label, -1, false, false, '*')
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001B1FC File Offset: 0x000193FC
		public TextField(string label, int maxLength, bool multiline, bool isPasswordField, char maskChar)
			: base(label, maxLength, maskChar, new TextField.TextInput())
		{
			base.AddToClassList(TextField.ussClassName);
			base.labelElement.AddToClassList(TextField.labelUssClassName);
			base.visualInput.AddToClassList(TextField.inputUssClassName);
			base.pickingMode = PickingMode.Ignore;
			this.SetValueWithoutNotify("");
			this.multiline = multiline;
			base.isPasswordField = isPasswordField;
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001B270 File Offset: 0x00019470
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001B288 File Offset: 0x00019488
		public override string value
		{
			get
			{
				return base.value;
			}
			set
			{
				base.value = value;
				base.textEdition.UpdateText(base.rawValue);
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001B2A5 File Offset: 0x000194A5
		public override void SetValueWithoutNotify(string newValue)
		{
			base.SetValueWithoutNotify(newValue);
			((INotifyValueChanged<string>)this.textInput.textElement).SetValueWithoutNotify(base.rawValue);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001B2C7 File Offset: 0x000194C7
		internal override void UpdateTextFromValue()
		{
			this.SetValueWithoutNotify(base.rawValue);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001B2D8 File Offset: 0x000194D8
		[EventInterest(new Type[] { typeof(BlurEvent) })]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			bool flag;
			if (base.isDelayed)
			{
				long? num = ((evt != null) ? new long?(evt.eventTypeId) : null);
				long num2 = EventBase<BlurEvent>.TypeId();
				flag = (num.GetValueOrDefault() == num2) & (num != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.value = base.text;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001B340 File Offset: 0x00019540
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			string fullHierarchicalViewDataKey = base.GetFullHierarchicalViewDataKey();
			base.OverwriteFromViewData(this, fullHierarchicalViewDataKey);
			base.text = base.rawValue;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001B372 File Offset: 0x00019572
		protected override string ValueToString(string value)
		{
			return value;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001B372 File Offset: 0x00019572
		protected override string StringToValue(string str)
		{
			return str;
		}

		// Token: 0x0400031E RID: 798
		public new static readonly string ussClassName = "unity-text-field";

		// Token: 0x0400031F RID: 799
		public new static readonly string labelUssClassName = TextField.ussClassName + "__label";

		// Token: 0x04000320 RID: 800
		public new static readonly string inputUssClassName = TextField.ussClassName + "__input";

		// Token: 0x020000D3 RID: 211
		public new class UxmlFactory : UxmlFactory<TextField, TextField.UxmlTraits>
		{
		}

		// Token: 0x020000D4 RID: 212
		public new class UxmlTraits : TextInputBaseField<string>.UxmlTraits
		{
			// Token: 0x06000717 RID: 1815 RVA: 0x0001B3B4 File Offset: 0x000195B4
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				TextField textField = (TextField)ve;
				textField.multiline = this.m_Multiline.GetValueFromBag(bag, cc);
				base.Init(ve, bag, cc);
				string empty = string.Empty;
				bool flag = TextField.UxmlTraits.k_Value.TryGetValueFromBag(bag, cc, ref empty);
				if (flag)
				{
					textField.SetValueWithoutNotify(empty);
				}
			}

			// Token: 0x04000321 RID: 801
			private static readonly UxmlStringAttributeDescription k_Value = new UxmlStringAttributeDescription
			{
				name = "value",
				obsoleteNames = new string[] { "text" }
			};

			// Token: 0x04000322 RID: 802
			private UxmlBoolAttributeDescription m_Multiline = new UxmlBoolAttributeDescription
			{
				name = "multiline"
			};
		}

		// Token: 0x020000D5 RID: 213
		private class TextInput : TextInputBaseField<string>.TextInputBase
		{
			// Token: 0x17000135 RID: 309
			// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001B466 File Offset: 0x00019666
			private TextField parentTextField
			{
				get
				{
					return (TextField)base.parent;
				}
			}

			// Token: 0x17000136 RID: 310
			// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001B474 File Offset: 0x00019674
			// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001B494 File Offset: 0x00019694
			public bool multiline
			{
				get
				{
					return base.textEdition.multiline;
				}
				set
				{
					bool flag = base.textEdition.multiline == value;
					if (!flag)
					{
						base.textEdition.multiline = value;
						if (value)
						{
							base.SetMultiline();
						}
						else
						{
							base.text = base.text.Replace("\n", "");
							base.SetSingleLine();
						}
					}
				}
			}

			// Token: 0x17000137 RID: 311
			// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001B4F8 File Offset: 0x000196F8
			public override bool isPasswordField
			{
				set
				{
					base.isPasswordField = value;
					if (value)
					{
						this.multiline = false;
					}
				}
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x0001B372 File Offset: 0x00019572
			protected override string StringToValue(string str)
			{
				return str;
			}
		}
	}
}
