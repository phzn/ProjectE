using System;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x020000D6 RID: 214
	public abstract class TextInputBaseField<TValueType> : BaseField<TValueType>
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001B524 File Offset: 0x00019724
		protected internal TextInputBaseField<TValueType>.TextInputBase textInputBase
		{
			get
			{
				return this.m_TextInputBase;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001B52C File Offset: 0x0001972C
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001B539 File Offset: 0x00019739
		public string text
		{
			get
			{
				return this.m_TextInputBase.text;
			}
			protected internal set
			{
				this.m_TextInputBase.text = value;
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000723 RID: 1827 RVA: 0x0001B548 File Offset: 0x00019748
		// (remove) Token: 0x06000724 RID: 1828 RVA: 0x0001B580 File Offset: 0x00019780
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event Action<bool> onIsReadOnlyChanged;

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001B5B5 File Offset: 0x000197B5
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0001B5C2 File Offset: 0x000197C2
		public bool isReadOnly
		{
			get
			{
				return this.textEdition.isReadOnly;
			}
			set
			{
				this.textEdition.isReadOnly = value;
				Action<bool> action = this.onIsReadOnlyChanged;
				if (action != null)
				{
					action(value);
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001B5E5 File Offset: 0x000197E5
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x0001B5F4 File Offset: 0x000197F4
		public bool isPasswordField
		{
			get
			{
				return this.m_TextInputBase.isPasswordField;
			}
			set
			{
				bool flag = this.m_TextInputBase.isPasswordField == value;
				if (!flag)
				{
					this.m_TextInputBase.isPasswordField = value;
					this.m_TextInputBase.IncrementVersion(VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001B634 File Offset: 0x00019834
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001B641 File Offset: 0x00019841
		public bool autoCorrection
		{
			get
			{
				return this.textEdition.autoCorrection;
			}
			set
			{
				this.textEdition.autoCorrection = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001B650 File Offset: 0x00019850
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001B65D File Offset: 0x0001985D
		public bool hideMobileInput
		{
			get
			{
				return this.textEdition.hideMobileInput;
			}
			set
			{
				this.textEdition.hideMobileInput = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001B66C File Offset: 0x0001986C
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001B679 File Offset: 0x00019879
		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				return this.textEdition.keyboardType;
			}
			set
			{
				this.textEdition.keyboardType = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001B688 File Offset: 0x00019888
		public TouchScreenKeyboard touchScreenKeyboard
		{
			get
			{
				return this.textEdition.touchScreenKeyboard;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001B695 File Offset: 0x00019895
		public ITextSelection textSelection
		{
			get
			{
				return this.m_TextInputBase.textElement.selection;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001B6A7 File Offset: 0x000198A7
		public ITextEdition textEdition
		{
			get
			{
				return this.m_TextInputBase.textElement.edition;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001B6B9 File Offset: 0x000198B9
		public Color selectionColor
		{
			get
			{
				return this.textSelection.selectionColor;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001B6C6 File Offset: 0x000198C6
		public Color cursorColor
		{
			get
			{
				return this.textSelection.cursorColor;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001B6D3 File Offset: 0x000198D3
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0001B6E0 File Offset: 0x000198E0
		public int cursorIndex
		{
			get
			{
				return this.textSelection.cursorIndex;
			}
			set
			{
				this.textSelection.cursorIndex = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001B6EF File Offset: 0x000198EF
		public Vector2 cursorPosition
		{
			get
			{
				return this.textSelection.cursorPosition;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001B6FC File Offset: 0x000198FC
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001B709 File Offset: 0x00019909
		public int selectIndex
		{
			get
			{
				return this.textSelection.selectIndex;
			}
			set
			{
				this.textSelection.selectIndex = value;
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001B718 File Offset: 0x00019918
		public void SelectAll()
		{
			this.textSelection.SelectAll();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001B727 File Offset: 0x00019927
		public void SelectNone()
		{
			this.textSelection.SelectNone();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001B736 File Offset: 0x00019936
		public void SelectRange(int cursorIndex, int selectionIndex)
		{
			this.textSelection.SelectRange(cursorIndex, selectionIndex);
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001B747 File Offset: 0x00019947
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0001B754 File Offset: 0x00019954
		public bool selectAllOnFocus
		{
			get
			{
				return this.textSelection.selectAllOnFocus;
			}
			set
			{
				this.textSelection.selectAllOnFocus = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001B763 File Offset: 0x00019963
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001B770 File Offset: 0x00019970
		public bool selectAllOnMouseUp
		{
			get
			{
				return this.textSelection.selectAllOnMouseUp;
			}
			set
			{
				this.textSelection.selectAllOnMouseUp = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001B77F File Offset: 0x0001997F
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0001B78C File Offset: 0x0001998C
		public int maxLength
		{
			get
			{
				return this.textEdition.maxLength;
			}
			set
			{
				this.textEdition.maxLength = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001B79B File Offset: 0x0001999B
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001B7A8 File Offset: 0x000199A8
		public bool doubleClickSelectsWord
		{
			get
			{
				return this.textSelection.doubleClickSelectsWord;
			}
			set
			{
				this.textSelection.doubleClickSelectsWord = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001B7B7 File Offset: 0x000199B7
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001B7C4 File Offset: 0x000199C4
		public bool tripleClickSelectsLine
		{
			get
			{
				return this.textSelection.tripleClickSelectsLine;
			}
			set
			{
				this.textSelection.tripleClickSelectsLine = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001B7D3 File Offset: 0x000199D3
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0001B7E0 File Offset: 0x000199E0
		public bool isDelayed
		{
			get
			{
				return this.textEdition.isDelayed;
			}
			set
			{
				this.textEdition.isDelayed = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001B7EF File Offset: 0x000199EF
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0001B7FC File Offset: 0x000199FC
		public char maskChar
		{
			get
			{
				return this.textEdition.maskChar;
			}
			set
			{
				this.textEdition.maskChar = value;
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001B80C File Offset: 0x00019A0C
		public bool SetVerticalScrollerVisibility(ScrollerVisibility sv)
		{
			return this.textInputBase.SetVerticalScrollerVisibility(sv);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001B82C File Offset: 0x00019A2C
		public Vector2 MeasureTextSize(string textToMeasure, float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			return TextUtilities.MeasureVisualElementTextSize(this.m_TextInputBase.textElement, textToMeasure, width, widthMode, height, heightMode);
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001B855 File Offset: 0x00019A55
		internal bool hasFocus
		{
			get
			{
				return this.textInputBase.textElement.hasFocus;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001B867 File Offset: 0x00019A67
		private protected override bool canSwitchToMixedValue
		{
			get
			{
				return !this.textInputBase.textElement.hasFocus || (this.textInputBase.textElement.hasFocus && this.focusController != null && this.focusController.IsPendingFocus(this));
			}
		}

		// Token: 0x0600074E RID: 1870
		protected abstract string ValueToString(TValueType value);

		// Token: 0x0600074F RID: 1871
		protected abstract TValueType StringToValue(string str);

		// Token: 0x06000750 RID: 1872 RVA: 0x0001B8A7 File Offset: 0x00019AA7
		protected TextInputBaseField(int maxLength, char maskChar, TextInputBaseField<TValueType>.TextInputBase textInputBase)
			: this(null, maxLength, maskChar, textInputBase)
		{
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001B8B8 File Offset: 0x00019AB8
		protected TextInputBaseField(string label, int maxLength, char maskChar, TextInputBaseField<TValueType>.TextInputBase textInputBase)
			: base(label, textInputBase)
		{
			base.tabIndex = 0;
			base.delegatesFocus = true;
			base.labelElement.tabIndex = -1;
			base.AddToClassList(TextInputBaseField<TValueType>.ussClassName);
			base.labelElement.AddToClassList(TextInputBaseField<TValueType>.labelUssClassName);
			base.visualInput.AddToClassList(TextInputBaseField<TValueType>.inputUssClassName);
			base.visualInput.AddToClassList(TextInputBaseField<TValueType>.singleLineInputUssClassName);
			this.m_TextInputBase = textInputBase;
			this.m_TextInputBase.maxLength = maxLength;
			this.m_TextInputBase.maskChar = maskChar;
			base.RegisterCallback<CustomStyleResolvedEvent>(new EventCallback<CustomStyleResolvedEvent>(this.OnFieldCustomStyleResolved), TrickleDown.NoTrickleDown);
			this.m_UpdateTextFromValue = true;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001B969 File Offset: 0x00019B69
		private void OnFieldCustomStyleResolved(CustomStyleResolvedEvent e)
		{
			this.m_TextInputBase.OnInputCustomStyleResolved(e);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001B97C File Offset: 0x00019B7C
		[EventInterest(new Type[]
		{
			typeof(NavigationSubmitEvent),
			typeof(FocusInEvent),
			typeof(FocusEvent),
			typeof(BlurEvent)
		})]
		protected override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			bool isReadOnly = this.textEdition.isReadOnly;
			if (!isReadOnly)
			{
				bool flag = evt.eventTypeId == EventBase<NavigationSubmitEvent>.TypeId() && evt.leafTarget != this.textInputBase.textElement;
				if (flag)
				{
					this.textInputBase.textElement.Focus();
				}
				else
				{
					bool flag2 = evt.eventTypeId == EventBase<FocusInEvent>.TypeId();
					if (flag2)
					{
						bool showMixedValue = base.showMixedValue;
						if (showMixedValue)
						{
							((INotifyValueChanged<string>)this.textInputBase.textElement).SetValueWithoutNotify(null);
						}
						bool flag3 = evt.leafTarget == this || evt.leafTarget == base.labelElement;
						if (flag3)
						{
							this.m_VisualInputTabIndex = this.textInputBase.textElement.tabIndex;
							this.textInputBase.textElement.tabIndex = -1;
						}
					}
					else
					{
						bool flag4 = evt.eventTypeId == EventBase<FocusEvent>.TypeId();
						if (flag4)
						{
							base.delegatesFocus = false;
						}
						else
						{
							bool flag5 = evt.eventTypeId == EventBase<BlurEvent>.TypeId();
							if (flag5)
							{
								bool showMixedValue2 = base.showMixedValue;
								if (showMixedValue2)
								{
									this.UpdateMixedValueContent();
								}
								base.delegatesFocus = true;
								bool flag6 = evt.leafTarget == this || evt.leafTarget == base.labelElement;
								if (flag6)
								{
									this.textInputBase.textElement.tabIndex = this.m_VisualInputTabIndex;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001BAF0 File Offset: 0x00019CF0
		protected override void UpdateMixedValueContent()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				bool updateTextFromValue = this.m_UpdateTextFromValue;
				if (updateTextFromValue)
				{
					((INotifyValueChanged<string>)this.textInputBase.textElement).SetValueWithoutNotify(BaseField<TValueType>.mixedValueString);
				}
				base.AddToClassList(BaseField<TValueType>.mixedValueLabelUssClassName);
				VisualElement visualInput = base.visualInput;
				if (visualInput != null)
				{
					visualInput.AddToClassList(BaseField<TValueType>.mixedValueLabelUssClassName);
				}
			}
			else
			{
				this.UpdateTextFromValue();
				VisualElement visualInput2 = base.visualInput;
				if (visualInput2 != null)
				{
					visualInput2.RemoveFromClassList(BaseField<TValueType>.mixedValueLabelUssClassName);
				}
				base.RemoveFromClassList(BaseField<TValueType>.mixedValueLabelUssClassName);
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001BB7D File Offset: 0x00019D7D
		internal virtual void UpdateValueFromText()
		{
			this.value = this.StringToValue(this.text);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00003CD2 File Offset: 0x00001ED2
		internal virtual void UpdateTextFromValue()
		{
		}

		// Token: 0x04000323 RID: 803
		private static CustomStyleProperty<Color> s_SelectionColorProperty = new CustomStyleProperty<Color>("--unity-selection-color");

		// Token: 0x04000324 RID: 804
		private static CustomStyleProperty<Color> s_CursorColorProperty = new CustomStyleProperty<Color>("--unity-cursor-color");

		// Token: 0x04000325 RID: 805
		private int m_VisualInputTabIndex;

		// Token: 0x04000326 RID: 806
		private TextInputBaseField<TValueType>.TextInputBase m_TextInputBase;

		// Token: 0x04000327 RID: 807
		internal bool m_UpdateTextFromValue;

		// Token: 0x04000328 RID: 808
		internal const int kMaxLengthNone = -1;

		// Token: 0x04000329 RID: 809
		internal const char kMaskCharDefault = '*';

		// Token: 0x0400032A RID: 810
		public new static readonly string ussClassName = "unity-base-text-field";

		// Token: 0x0400032B RID: 811
		public new static readonly string labelUssClassName = TextInputBaseField<TValueType>.ussClassName + "__label";

		// Token: 0x0400032C RID: 812
		public new static readonly string inputUssClassName = TextInputBaseField<TValueType>.ussClassName + "__input";

		// Token: 0x0400032D RID: 813
		internal static readonly string multilineContainerClassName = TextInputBaseField<TValueType>.ussClassName + "__multiline-container";

		// Token: 0x0400032E RID: 814
		public static readonly string singleLineInputUssClassName = TextInputBaseField<TValueType>.inputUssClassName + "--single-line";

		// Token: 0x0400032F RID: 815
		public static readonly string multilineInputUssClassName = TextInputBaseField<TValueType>.inputUssClassName + "--multiline";

		// Token: 0x04000330 RID: 816
		internal static readonly string multilineInputWithScrollViewUssClassName = TextInputBaseField<TValueType>.multilineInputUssClassName + "--scroll-view";

		// Token: 0x04000331 RID: 817
		public static readonly string textInputUssName = "unity-text-input";

		// Token: 0x020000D7 RID: 215
		public new class UxmlTraits : BaseFieldTraits<string, UxmlStringAttributeDescription>
		{
			// Token: 0x06000758 RID: 1880 RVA: 0x0001BC4C File Offset: 0x00019E4C
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)ve;
				textInputBaseField.maxLength = this.m_MaxLength.GetValueFromBag(bag, cc);
				textInputBaseField.isPasswordField = this.m_Password.GetValueFromBag(bag, cc);
				textInputBaseField.isReadOnly = this.m_IsReadOnly.GetValueFromBag(bag, cc);
				textInputBaseField.isDelayed = this.m_IsDelayed.GetValueFromBag(bag, cc);
				textInputBaseField.hideMobileInput = this.m_HideMobileInput.GetValueFromBag(bag, cc);
				textInputBaseField.keyboardType = this.m_KeyboardType.GetValueFromBag(bag, cc);
				textInputBaseField.autoCorrection = this.m_AutoCorrection.GetValueFromBag(bag, cc);
				string valueFromBag = this.m_MaskCharacter.GetValueFromBag(bag, cc);
				textInputBaseField.maskChar = (string.IsNullOrEmpty(valueFromBag) ? '*' : valueFromBag[0]);
			}

			// Token: 0x04000333 RID: 819
			private UxmlIntAttributeDescription m_MaxLength = new UxmlIntAttributeDescription
			{
				name = "max-length",
				obsoleteNames = new string[] { "maxLength" },
				defaultValue = -1
			};

			// Token: 0x04000334 RID: 820
			private UxmlBoolAttributeDescription m_Password = new UxmlBoolAttributeDescription
			{
				name = "password"
			};

			// Token: 0x04000335 RID: 821
			private UxmlStringAttributeDescription m_MaskCharacter = new UxmlStringAttributeDescription
			{
				name = "mask-character",
				obsoleteNames = new string[] { "maskCharacter" },
				defaultValue = '*'.ToString()
			};

			// Token: 0x04000336 RID: 822
			private UxmlBoolAttributeDescription m_IsReadOnly = new UxmlBoolAttributeDescription
			{
				name = "readonly"
			};

			// Token: 0x04000337 RID: 823
			private UxmlBoolAttributeDescription m_IsDelayed = new UxmlBoolAttributeDescription
			{
				name = "is-delayed"
			};

			// Token: 0x04000338 RID: 824
			private UxmlBoolAttributeDescription m_HideMobileInput = new UxmlBoolAttributeDescription
			{
				name = "hide-mobile-input"
			};

			// Token: 0x04000339 RID: 825
			private UxmlEnumAttributeDescription<TouchScreenKeyboardType> m_KeyboardType = new UxmlEnumAttributeDescription<TouchScreenKeyboardType>
			{
				name = "keyboard-type"
			};

			// Token: 0x0400033A RID: 826
			private UxmlBoolAttributeDescription m_AutoCorrection = new UxmlBoolAttributeDescription
			{
				name = "auto-correction"
			};
		}

		// Token: 0x020000D8 RID: 216
		protected internal abstract class TextInputBase : VisualElement
		{
			// Token: 0x17000150 RID: 336
			// (get) Token: 0x0600075A RID: 1882 RVA: 0x0001BE33 File Offset: 0x0001A033
			// (set) Token: 0x0600075B RID: 1883 RVA: 0x0001BE3B File Offset: 0x0001A03B
			internal TextElement textElement { get; private set; }

			// Token: 0x17000151 RID: 337
			// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001BE44 File Offset: 0x0001A044
			public ITextSelection textSelection
			{
				get
				{
					return this.textElement.selection;
				}
			}

			// Token: 0x17000152 RID: 338
			// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001BE51 File Offset: 0x0001A051
			public ITextEdition textEdition
			{
				get
				{
					return this.textElement.edition;
				}
			}

			// Token: 0x0600075E RID: 1886 RVA: 0x0001BE5E File Offset: 0x0001A05E
			public void SelectAll()
			{
				this.textSelection.SelectAll();
			}

			// Token: 0x0600075F RID: 1887 RVA: 0x0001BE6D File Offset: 0x0001A06D
			internal void SelectNone()
			{
				this.textSelection.SelectNone();
			}

			// Token: 0x17000153 RID: 339
			// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001BE7C File Offset: 0x0001A07C
			internal string originalText
			{
				get
				{
					return this.textElement.originalText;
				}
			}

			// Token: 0x06000761 RID: 1889 RVA: 0x0001BE89 File Offset: 0x0001A089
			protected virtual TValueType StringToValue(string str)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000762 RID: 1890 RVA: 0x0001BE94 File Offset: 0x0001A094
			internal void UpdateValueFromText()
			{
				TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)base.parent;
				textInputBaseField.UpdateValueFromText();
			}

			// Token: 0x06000763 RID: 1891 RVA: 0x0001BEB8 File Offset: 0x0001A0B8
			internal void UpdateTextFromValue()
			{
				TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)base.parent;
				textInputBaseField.UpdateTextFromValue();
			}

			// Token: 0x06000764 RID: 1892 RVA: 0x0001BEDC File Offset: 0x0001A0DC
			internal void MoveFocusToCompositeRoot()
			{
				TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)base.parent;
				textInputBaseField.Focus();
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x0001BEFD File Offset: 0x0001A0FD
			public void ResetValueAndText()
			{
				this.textEdition.ResetValueAndText();
			}

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x06000766 RID: 1894 RVA: 0x0001BF0C File Offset: 0x0001A10C
			// (set) Token: 0x06000767 RID: 1895 RVA: 0x0001BF19 File Offset: 0x0001A119
			public bool isReadOnly
			{
				get
				{
					return this.textEdition.isReadOnly;
				}
				set
				{
					this.textEdition.isReadOnly = value;
				}
			}

			// Token: 0x17000155 RID: 341
			// (get) Token: 0x06000768 RID: 1896 RVA: 0x0001BF28 File Offset: 0x0001A128
			// (set) Token: 0x06000769 RID: 1897 RVA: 0x0001BF35 File Offset: 0x0001A135
			public int maxLength
			{
				get
				{
					return this.textEdition.maxLength;
				}
				set
				{
					this.textEdition.maxLength = value;
				}
			}

			// Token: 0x17000156 RID: 342
			// (get) Token: 0x0600076A RID: 1898 RVA: 0x0001BF44 File Offset: 0x0001A144
			// (set) Token: 0x0600076B RID: 1899 RVA: 0x0001BF51 File Offset: 0x0001A151
			public char maskChar
			{
				get
				{
					return this.textEdition.maskChar;
				}
				set
				{
					this.textEdition.maskChar = value;
				}
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x0600076C RID: 1900 RVA: 0x0001BF60 File Offset: 0x0001A160
			// (set) Token: 0x0600076D RID: 1901 RVA: 0x0001BF6D File Offset: 0x0001A16D
			public virtual bool isPasswordField
			{
				get
				{
					return this.textEdition.isPassword;
				}
				set
				{
					this.textEdition.isPassword = value;
				}
			}

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001BF7C File Offset: 0x0001A17C
			// (set) Token: 0x0600076F RID: 1903 RVA: 0x0001BF89 File Offset: 0x0001A189
			internal bool isDelayed
			{
				get
				{
					return this.textEdition.isDelayed;
				}
				set
				{
					this.textEdition.isDelayed = value;
				}
			}

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000770 RID: 1904 RVA: 0x0001BF98 File Offset: 0x0001A198
			// (set) Token: 0x06000771 RID: 1905 RVA: 0x0001BFA0 File Offset: 0x0001A1A0
			internal bool isDragging { get; set; }

			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000772 RID: 1906 RVA: 0x0001BFA9 File Offset: 0x0001A1A9
			// (set) Token: 0x06000773 RID: 1907 RVA: 0x0001BFB6 File Offset: 0x0001A1B6
			public Color selectionColor
			{
				get
				{
					return this.textSelection.selectionColor;
				}
				set
				{
					this.textSelection.selectionColor = value;
				}
			}

			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000774 RID: 1908 RVA: 0x0001BFC5 File Offset: 0x0001A1C5
			// (set) Token: 0x06000775 RID: 1909 RVA: 0x0001BFD2 File Offset: 0x0001A1D2
			public Color cursorColor
			{
				get
				{
					return this.textSelection.cursorColor;
				}
				set
				{
					this.textSelection.cursorColor = value;
				}
			}

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x06000776 RID: 1910 RVA: 0x0001BFE1 File Offset: 0x0001A1E1
			public int cursorIndex
			{
				get
				{
					return this.textSelection.cursorIndex;
				}
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000777 RID: 1911 RVA: 0x0001BFEE File Offset: 0x0001A1EE
			public int selectIndex
			{
				get
				{
					return this.textSelection.selectIndex;
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x06000778 RID: 1912 RVA: 0x0001BFFB File Offset: 0x0001A1FB
			// (set) Token: 0x06000779 RID: 1913 RVA: 0x0001C008 File Offset: 0x0001A208
			public bool doubleClickSelectsWord
			{
				get
				{
					return this.textSelection.doubleClickSelectsWord;
				}
				set
				{
					this.textSelection.doubleClickSelectsWord = value;
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x0600077A RID: 1914 RVA: 0x0001C017 File Offset: 0x0001A217
			// (set) Token: 0x0600077B RID: 1915 RVA: 0x0001C024 File Offset: 0x0001A224
			public bool tripleClickSelectsLine
			{
				get
				{
					return this.textSelection.tripleClickSelectsLine;
				}
				set
				{
					this.textSelection.tripleClickSelectsLine = value;
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x0600077C RID: 1916 RVA: 0x0001C033 File Offset: 0x0001A233
			// (set) Token: 0x0600077D RID: 1917 RVA: 0x0001C040 File Offset: 0x0001A240
			public string text
			{
				get
				{
					return this.textElement.text;
				}
				set
				{
					bool flag = this.textElement.text == value;
					if (!flag)
					{
						this.textElement.text = value;
					}
				}
			}

			// Token: 0x0600077E RID: 1918 RVA: 0x0001C074 File Offset: 0x0001A274
			internal TextInputBase()
			{
				base.delegatesFocus = true;
				this.textElement = new TextElement();
				this.textElement.parseEscapeSequences = false;
				this.textElement.selection.isSelectable = true;
				this.textEdition.isReadOnly = false;
				this.textEdition.keyboardType = TouchScreenKeyboardType.Default;
				this.textEdition.autoCorrection = false;
				this.textSelection.isSelectable = true;
				this.textElement.enableRichText = false;
				this.textSelection.selectAllOnFocus = true;
				this.textSelection.selectAllOnMouseUp = true;
				this.textElement.tabIndex = 0;
				ITextEdition textEdition = this.textEdition;
				textEdition.AcceptCharacter = (Func<char, bool>)Delegate.Combine(textEdition.AcceptCharacter, new Func<char, bool>(this.AcceptCharacter));
				ITextEdition textEdition2 = this.textEdition;
				textEdition2.UpdateScrollOffset = (Action<bool>)Delegate.Combine(textEdition2.UpdateScrollOffset, new Action<bool>(this.UpdateScrollOffset));
				ITextEdition textEdition3 = this.textEdition;
				textEdition3.UpdateValueFromText = (Action)Delegate.Combine(textEdition3.UpdateValueFromText, new Action(this.UpdateValueFromText));
				ITextEdition textEdition4 = this.textEdition;
				textEdition4.UpdateTextFromValue = (Action)Delegate.Combine(textEdition4.UpdateTextFromValue, new Action(this.UpdateTextFromValue));
				ITextEdition textEdition5 = this.textEdition;
				textEdition5.MoveFocusToCompositeRoot = (Action)Delegate.Combine(textEdition5.MoveFocusToCompositeRoot, new Action(this.MoveFocusToCompositeRoot));
				base.AddToClassList(TextInputBaseField<TValueType>.inputUssClassName);
				base.name = TextInputBaseField<string>.textInputUssName;
				this.SetSingleLine();
				base.RegisterCallback<CustomStyleResolvedEvent>(new EventCallback<CustomStyleResolvedEvent>(this.OnInputCustomStyleResolved), TrickleDown.NoTrickleDown);
				base.tabIndex = -1;
			}

			// Token: 0x0600077F RID: 1919 RVA: 0x00010DA2 File Offset: 0x0000EFA2
			private void MakeSureScrollViewDoesNotLeakEvents(ChangeEvent<float> evt)
			{
				evt.StopPropagation();
			}

			// Token: 0x06000780 RID: 1920 RVA: 0x0001C248 File Offset: 0x0001A448
			internal void SetSingleLine()
			{
				base.hierarchy.Clear();
				this.RemoveMultilineComponents();
				base.Add(this.textElement);
				base.AddToClassList(TextInputBaseField<TValueType>.singleLineInputUssClassName);
				this.textElement.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.innerTextElementUssClassName);
				this.textElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.TextElementOnGeometryChangedEvent), TrickleDown.NoTrickleDown);
				bool flag = this.scrollOffset != Vector2.zero;
				if (flag)
				{
					this.scrollOffset.y = 0f;
					this.UpdateScrollOffset(false);
				}
			}

			// Token: 0x06000781 RID: 1921 RVA: 0x0001C2E0 File Offset: 0x0001A4E0
			internal void SetMultiline()
			{
				bool flag = !this.textEdition.multiline;
				if (!flag)
				{
					this.RemoveSingleLineComponents();
					this.RemoveMultilineComponents();
					bool flag2 = this.m_VerticalScrollerVisibility != ScrollerVisibility.Hidden && this.scrollView == null;
					if (flag2)
					{
						this.scrollView = new ScrollView();
						this.scrollView.Add(this.textElement);
						base.Add(this.scrollView);
						this.SetScrollViewMode();
						this.scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
						this.scrollView.verticalScrollerVisibility = this.m_VerticalScrollerVisibility;
						this.scrollView.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.innerScrollviewUssClassName);
						this.scrollView.contentViewport.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.innerViewportUssClassName);
						this.scrollView.contentContainer.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.innerContentContainerUssClassName);
						this.scrollView.contentContainer.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.ScrollViewOnGeometryChangedEvent), TrickleDown.NoTrickleDown);
						this.scrollView.verticalScroller.slider.RegisterValueChangedCallback<float>(new EventCallback<ChangeEvent<float>>(this.MakeSureScrollViewDoesNotLeakEvents));
						this.scrollView.verticalScroller.slider.focusable = false;
						this.scrollView.horizontalScroller.slider.RegisterValueChangedCallback<float>(new EventCallback<ChangeEvent<float>>(this.MakeSureScrollViewDoesNotLeakEvents));
						this.scrollView.horizontalScroller.slider.focusable = false;
						base.AddToClassList(TextInputBaseField<TValueType>.multilineInputWithScrollViewUssClassName);
						this.textElement.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.innerTextElementWithScrollViewUssClassName);
					}
					else
					{
						bool flag3 = this.multilineContainer == null;
						if (flag3)
						{
							this.textElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.TextElementOnGeometryChangedEvent), TrickleDown.NoTrickleDown);
							this.multilineContainer = new VisualElement
							{
								classList = { TextInputBaseField<TValueType>.multilineContainerClassName }
							};
							this.multilineContainer.Add(this.textElement);
							base.Add(this.multilineContainer);
							this.SetMultilineContainerStyle();
							base.AddToClassList(TextInputBaseField<TValueType>.multilineInputUssClassName);
							this.textElement.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.innerTextElementUssClassName);
						}
					}
				}
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x0001C4F8 File Offset: 0x0001A6F8
			private void ScrollViewOnGeometryChangedEvent(GeometryChangedEvent e)
			{
				bool flag = e.oldRect.size == e.newRect.size;
				if (!flag)
				{
					this.UpdateScrollOffset(false);
				}
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x0001C538 File Offset: 0x0001A738
			private void TextElementOnGeometryChangedEvent(GeometryChangedEvent e)
			{
				bool flag = e.oldRect.size == e.newRect.size;
				if (!flag)
				{
					bool flag2 = Math.Abs(e.oldRect.size.x - e.newRect.size.x) > 1E-30f;
					this.UpdateScrollOffset(false, flag2);
				}
			}

			// Token: 0x06000784 RID: 1924 RVA: 0x0001C5AC File Offset: 0x0001A7AC
			internal void OnInputCustomStyleResolved(CustomStyleResolvedEvent e)
			{
				ICustomStyle customStyle = e.customStyle;
				Color color;
				bool flag = customStyle.TryGetValue(TextInputBaseField<TValueType>.s_SelectionColorProperty, out color);
				if (flag)
				{
					this.textSelection.selectionColor = color;
				}
				Color color2;
				bool flag2 = customStyle.TryGetValue(TextInputBaseField<TValueType>.s_CursorColorProperty, out color2);
				if (flag2)
				{
					this.textSelection.cursorColor = color2;
				}
				this.SetScrollViewMode();
				this.SetMultilineContainerStyle();
			}

			// Token: 0x06000785 RID: 1925 RVA: 0x0001C610 File Offset: 0x0001A810
			internal virtual bool AcceptCharacter(char c)
			{
				return !this.isReadOnly && base.enabledInHierarchy;
			}

			// Token: 0x06000786 RID: 1926 RVA: 0x0001C633 File Offset: 0x0001A833
			internal void UpdateScrollOffset(bool isBackspace = false)
			{
				this.UpdateScrollOffset(isBackspace, false);
			}

			// Token: 0x06000787 RID: 1927 RVA: 0x0001C640 File Offset: 0x0001A840
			internal void UpdateScrollOffset(bool isBackspace, bool widthChanged)
			{
				ITextSelection textSelection = this.textSelection;
				bool flag = textSelection.cursorIndex < 0;
				if (!flag)
				{
					bool flag2 = this.scrollView != null;
					if (flag2)
					{
						this.scrollOffset = this.GetScrollOffset(this.scrollView.scrollOffset.x, this.scrollView.scrollOffset.y, this.scrollView.contentViewport.layout.width, isBackspace, widthChanged);
						this.scrollView.scrollOffset = this.scrollOffset;
						this.m_ScrollViewWasClamped = this.scrollOffset.x > this.scrollView.scrollOffset.x || this.scrollOffset.y > this.scrollView.scrollOffset.y;
					}
					else
					{
						Vector3 position = this.textElement.transform.position;
						this.scrollOffset = this.GetScrollOffset(this.scrollOffset.x, this.scrollOffset.y, base.contentRect.width, isBackspace, widthChanged);
						position.y = -Mathf.Min(this.scrollOffset.y, Math.Abs(this.textElement.contentRect.height - base.contentRect.height));
						position.x = -this.scrollOffset.x;
						bool flag3 = !position.Equals(this.textElement.transform.position);
						if (flag3)
						{
							this.textElement.transform.position = position;
						}
					}
				}
			}

			// Token: 0x06000788 RID: 1928 RVA: 0x0001C7E4 File Offset: 0x0001A9E4
			private Vector2 GetScrollOffset(float xOffset, float yOffset, float contentViewportWidth, bool isBackspace, bool widthChanged)
			{
				Vector2 cursorPosition = this.textSelection.cursorPosition;
				float cursorWidth = this.textSelection.cursorWidth;
				float num = xOffset;
				float num2 = yOffset;
				bool flag = Math.Abs(this.lastCursorPos.x - cursorPosition.x) > 0.05f || this.m_ScrollViewWasClamped || widthChanged;
				if (flag)
				{
					bool flag2 = cursorPosition.x > xOffset + contentViewportWidth - cursorWidth || (xOffset > 0f && widthChanged);
					if (flag2)
					{
						float num3 = Mathf.Ceil(cursorPosition.x + cursorWidth - contentViewportWidth);
						num = Mathf.Max(num3, 0f);
					}
					else
					{
						bool flag3 = cursorPosition.x < xOffset + 5f;
						if (flag3)
						{
							num = Mathf.Max(cursorPosition.x - 5f, 0f);
						}
					}
				}
				bool flag4 = this.textEdition.multiline && (Math.Abs(this.lastCursorPos.y - cursorPosition.y) > 0.05f || this.m_ScrollViewWasClamped);
				if (flag4)
				{
					bool flag5 = cursorPosition.y > base.contentRect.height + yOffset;
					if (flag5)
					{
						num2 = cursorPosition.y - base.contentRect.height;
					}
					else
					{
						bool flag6 = cursorPosition.y < this.textSelection.lineHeightAtCursorPosition + yOffset + 0.05f;
						if (flag6)
						{
							num2 = cursorPosition.y - this.textSelection.lineHeightAtCursorPosition;
						}
					}
				}
				this.lastCursorPos = cursorPosition;
				bool flag7 = Math.Abs(xOffset - num) > 0.05f || Math.Abs(yOffset - num2) > 0.05f;
				Vector2 vector;
				if (flag7)
				{
					vector = new Vector2(num, num2);
				}
				else
				{
					vector = ((this.scrollView != null) ? this.scrollView.scrollOffset : this.scrollOffset);
				}
				return vector;
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x0001C9B8 File Offset: 0x0001ABB8
			internal void SetScrollViewMode()
			{
				bool flag = this.scrollView == null;
				if (!flag)
				{
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.verticalVariantInnerTextElementUssClassName);
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.verticalHorizontalVariantInnerTextElementUssClassName);
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.horizontalVariantInnerTextElementUssClassName);
					bool flag2 = this.textEdition.multiline && base.computedStyle.whiteSpace == WhiteSpace.Normal;
					if (flag2)
					{
						this.textElement.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.verticalVariantInnerTextElementUssClassName);
						this.scrollView.mode = ScrollViewMode.Vertical;
					}
					else
					{
						bool multiline = this.textEdition.multiline;
						if (multiline)
						{
							this.textElement.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.verticalHorizontalVariantInnerTextElementUssClassName);
							this.scrollView.mode = ScrollViewMode.VerticalAndHorizontal;
						}
						else
						{
							this.textElement.AddToClassList(TextInputBaseField<TValueType>.TextInputBase.horizontalVariantInnerTextElementUssClassName);
							this.scrollView.mode = ScrollViewMode.Horizontal;
						}
					}
				}
			}

			// Token: 0x0600078A RID: 1930 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
			private void SetMultilineContainerStyle()
			{
				bool flag = this.multilineContainer != null;
				if (flag)
				{
					bool flag2 = base.computedStyle.whiteSpace == WhiteSpace.Normal;
					if (flag2)
					{
						base.style.overflow = Overflow.Hidden;
					}
					else
					{
						base.style.overflow = (Overflow)2;
					}
				}
			}

			// Token: 0x0600078B RID: 1931 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
			private void RemoveSingleLineComponents()
			{
				base.RemoveFromClassList(TextInputBaseField<TValueType>.singleLineInputUssClassName);
				this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.innerTextElementUssClassName);
				this.textElement.RemoveFromHierarchy();
				this.textElement.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.TextElementOnGeometryChangedEvent), TrickleDown.NoTrickleDown);
			}

			// Token: 0x0600078C RID: 1932 RVA: 0x0001CB48 File Offset: 0x0001AD48
			private void RemoveMultilineComponents()
			{
				bool flag = this.scrollView != null;
				if (flag)
				{
					this.scrollView.RemoveFromHierarchy();
					this.scrollView.contentContainer.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.ScrollViewOnGeometryChangedEvent), TrickleDown.NoTrickleDown);
					this.scrollView.verticalScroller.slider.UnregisterValueChangedCallback<float>(new EventCallback<ChangeEvent<float>>(this.MakeSureScrollViewDoesNotLeakEvents));
					this.scrollView.horizontalScroller.slider.UnregisterValueChangedCallback<float>(new EventCallback<ChangeEvent<float>>(this.MakeSureScrollViewDoesNotLeakEvents));
					this.scrollView = null;
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.verticalVariantInnerTextElementUssClassName);
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.verticalHorizontalVariantInnerTextElementUssClassName);
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.horizontalVariantInnerTextElementUssClassName);
					base.RemoveFromClassList(TextInputBaseField<TValueType>.multilineInputWithScrollViewUssClassName);
					this.textElement.RemoveFromClassList(TextInputBaseField<TValueType>.TextInputBase.innerTextElementWithScrollViewUssClassName);
				}
				bool flag2 = this.multilineContainer != null;
				if (flag2)
				{
					this.textElement.transform.position = Vector3.zero;
					this.multilineContainer.RemoveFromHierarchy();
					this.textElement.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.TextElementOnGeometryChangedEvent), TrickleDown.NoTrickleDown);
					this.multilineContainer = null;
					base.RemoveFromClassList(TextInputBaseField<TValueType>.multilineInputUssClassName);
				}
			}

			// Token: 0x0600078D RID: 1933 RVA: 0x0001CC8C File Offset: 0x0001AE8C
			internal bool SetVerticalScrollerVisibility(ScrollerVisibility sv)
			{
				bool multiline = this.textEdition.multiline;
				bool flag2;
				if (multiline)
				{
					this.m_VerticalScrollerVisibility = sv;
					bool flag = this.scrollView == null;
					if (flag)
					{
						this.SetMultiline();
					}
					else
					{
						this.scrollView.verticalScrollerVisibility = this.m_VerticalScrollerVisibility;
					}
					flag2 = true;
				}
				else
				{
					Debug.LogWarning("Can't SetVerticalScrollerVisibility as the field isn't multiline.");
					flag2 = false;
				}
				return flag2;
			}

			// Token: 0x0400033C RID: 828
			internal ScrollView scrollView;

			// Token: 0x0400033D RID: 829
			internal VisualElement multilineContainer;

			// Token: 0x0400033E RID: 830
			public static readonly string innerComponentsModifierName = "--inner-input-field-component";

			// Token: 0x0400033F RID: 831
			public static readonly string innerTextElementUssClassName = TextElement.ussClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName;

			// Token: 0x04000340 RID: 832
			internal static readonly string innerTextElementWithScrollViewUssClassName = TextElement.ussClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName + "--scroll-view";

			// Token: 0x04000341 RID: 833
			public static readonly string horizontalVariantInnerTextElementUssClassName = TextElement.ussClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName + "--horizontal";

			// Token: 0x04000342 RID: 834
			public static readonly string verticalVariantInnerTextElementUssClassName = TextElement.ussClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName + "--vertical";

			// Token: 0x04000343 RID: 835
			public static readonly string verticalHorizontalVariantInnerTextElementUssClassName = TextElement.ussClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName + "--vertical-horizontal";

			// Token: 0x04000344 RID: 836
			public static readonly string innerScrollviewUssClassName = ScrollView.ussClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName;

			// Token: 0x04000345 RID: 837
			public static readonly string innerViewportUssClassName = ScrollView.viewportUssClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName;

			// Token: 0x04000346 RID: 838
			public static readonly string innerContentContainerUssClassName = ScrollView.contentUssClassName + TextInputBaseField<TValueType>.TextInputBase.innerComponentsModifierName;

			// Token: 0x04000348 RID: 840
			internal Vector2 scrollOffset = Vector2.zero;

			// Token: 0x04000349 RID: 841
			private bool m_ScrollViewWasClamped;

			// Token: 0x0400034A RID: 842
			private Vector2 lastCursorPos = Vector2.zero;

			// Token: 0x0400034B RID: 843
			private ScrollerVisibility m_VerticalScrollerVisibility = ScrollerVisibility.Hidden;
		}
	}
}
