using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000113 RID: 275
	public class RadioButton : BaseBoolField, IGroupBoxOption
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00024032 File Offset: 0x00022232
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x0002403C File Offset: 0x0002223C
		public override bool value
		{
			get
			{
				return base.value;
			}
			set
			{
				bool flag = base.value != value;
				if (flag)
				{
					base.value = value;
					this.UpdateCheckmark();
					if (value)
					{
						this.OnOptionSelected<RadioButton>();
					}
				}
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00024079 File Offset: 0x00022279
		public RadioButton()
			: this(null)
		{
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00024084 File Offset: 0x00022284
		public RadioButton(string label)
			: base(label)
		{
			base.AddToClassList(RadioButton.ussClassName);
			base.visualInput.AddToClassList(RadioButton.inputUssClassName);
			base.labelElement.AddToClassList(RadioButton.labelUssClassName);
			this.m_CheckMark.RemoveFromHierarchy();
			this.m_CheckmarkBackground = new VisualElement
			{
				pickingMode = PickingMode.Ignore
			};
			this.m_CheckmarkBackground.Add(this.m_CheckMark);
			this.m_CheckmarkBackground.AddToClassList(RadioButton.checkmarkBackgroundUssClassName);
			this.m_CheckMark.AddToClassList(RadioButton.checkmarkUssClassName);
			base.visualInput.Add(this.m_CheckmarkBackground);
			this.UpdateCheckmark();
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnOptionAttachToPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnOptionDetachFromPanel), TrickleDown.NoTrickleDown);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002415C File Offset: 0x0002235C
		private void OnOptionAttachToPanel(AttachToPanelEvent evt)
		{
			this.RegisterGroupBoxOption<RadioButton>();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00024166 File Offset: 0x00022366
		private void OnOptionDetachFromPanel(DetachFromPanelEvent evt)
		{
			this.UnregisterGroupBoxOption<RadioButton>();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00024170 File Offset: 0x00022370
		protected override void InitLabel()
		{
			base.InitLabel();
			this.m_Label.AddToClassList(RadioButton.textUssClassName);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002418C File Offset: 0x0002238C
		protected override void ToggleValue()
		{
			bool flag = !this.value;
			if (flag)
			{
				this.value = true;
			}
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000241B1 File Offset: 0x000223B1
		[Obsolete("[UI Toolkit] Please set the value property instead.", false)]
		public void SetSelected(bool selected)
		{
			((IGroupBoxOption)this).SetSelected(selected);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x000241BC File Offset: 0x000223BC
		void IGroupBoxOption.SetSelected(bool selected)
		{
			this.value = selected;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000241C7 File Offset: 0x000223C7
		public override void SetValueWithoutNotify(bool newValue)
		{
			base.SetValueWithoutNotify(newValue);
			this.UpdateCheckmark();
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000241D9 File Offset: 0x000223D9
		private void UpdateCheckmark()
		{
			this.m_CheckMark.style.display = (this.value ? DisplayStyle.Flex : DisplayStyle.None);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00024200 File Offset: 0x00022400
		protected override void UpdateMixedValueContent()
		{
			base.UpdateMixedValueContent();
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				this.m_CheckmarkBackground.RemoveFromHierarchy();
			}
			else
			{
				this.m_CheckmarkBackground.Add(this.m_CheckMark);
				base.visualInput.Add(this.m_CheckmarkBackground);
			}
		}

		// Token: 0x04000434 RID: 1076
		public new static readonly string ussClassName = "unity-radio-button";

		// Token: 0x04000435 RID: 1077
		public new static readonly string labelUssClassName = RadioButton.ussClassName + "__label";

		// Token: 0x04000436 RID: 1078
		public new static readonly string inputUssClassName = RadioButton.ussClassName + "__input";

		// Token: 0x04000437 RID: 1079
		public static readonly string checkmarkBackgroundUssClassName = RadioButton.ussClassName + "__checkmark-background";

		// Token: 0x04000438 RID: 1080
		public static readonly string checkmarkUssClassName = RadioButton.ussClassName + "__checkmark";

		// Token: 0x04000439 RID: 1081
		public static readonly string textUssClassName = RadioButton.ussClassName + "__text";

		// Token: 0x0400043A RID: 1082
		private VisualElement m_CheckmarkBackground;

		// Token: 0x02000114 RID: 276
		public new class UxmlFactory : UxmlFactory<RadioButton, RadioButton.UxmlTraits>
		{
		}

		// Token: 0x02000115 RID: 277
		public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription>
		{
			// Token: 0x0600095A RID: 2394 RVA: 0x000242DC File Offset: 0x000224DC
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				((RadioButton)ve).text = this.m_Text.GetValueFromBag(bag, cc);
			}

			// Token: 0x0400043B RID: 1083
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};
		}
	}
}
