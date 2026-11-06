using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000054 RID: 84
	public abstract class BaseBoolField : BaseField<bool>
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000D9FF File Offset: 0x0000BBFF
		internal Label boolFieldLabelElement
		{
			get
			{
				return this.m_Label;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000DA08 File Offset: 0x0000BC08
		public BaseBoolField(string label)
			: base(label, null)
		{
			this.m_CheckMark = new VisualElement
			{
				name = "unity-checkmark",
				pickingMode = PickingMode.Ignore
			};
			base.visualInput.Add(this.m_CheckMark);
			base.visualInput.pickingMode = PickingMode.Position;
			this.text = null;
			this.AddManipulator(this.m_Clickable = new Clickable(new Action<EventBase>(this.OnClickEvent)));
			base.RegisterCallback<NavigationSubmitEvent>(new EventCallback<NavigationSubmitEvent>(this.OnNavigationSubmit), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000DA9A File Offset: 0x0000BC9A
		private void OnNavigationSubmit(NavigationSubmitEvent evt)
		{
			this.ToggleValue();
			evt.StopPropagation();
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000DAAC File Offset: 0x0000BCAC
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
		public string text
		{
			get
			{
				Label label = this.m_Label;
				return (label != null) ? label.text : null;
			}
			set
			{
				bool flag = !string.IsNullOrEmpty(value);
				if (flag)
				{
					bool flag2 = this.m_Label == null;
					if (flag2)
					{
						this.InitLabel();
					}
					this.m_Label.text = value;
				}
				else
				{
					bool flag3 = this.m_Label != null;
					if (flag3)
					{
						this.m_Label.RemoveFromHierarchy();
						this.m_Label = null;
					}
				}
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000DB34 File Offset: 0x0000BD34
		protected virtual void InitLabel()
		{
			this.m_Label = new Label();
			base.visualInput.Add(this.m_Label);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000DB54 File Offset: 0x0000BD54
		public override void SetValueWithoutNotify(bool newValue)
		{
			if (newValue)
			{
				base.visualInput.pseudoStates |= PseudoStates.Checked;
				base.pseudoStates |= PseudoStates.Checked;
			}
			else
			{
				base.visualInput.pseudoStates &= ~PseudoStates.Checked;
				base.pseudoStates &= ~PseudoStates.Checked;
			}
			base.SetValueWithoutNotify(newValue);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000DBC0 File Offset: 0x0000BDC0
		private void OnClickEvent(EventBase evt)
		{
			bool flag = evt.eventTypeId == EventBase<MouseUpEvent>.TypeId();
			if (flag)
			{
				IMouseEvent mouseEvent = (IMouseEvent)evt;
				bool flag2 = mouseEvent.button == 0;
				if (flag2)
				{
					this.ToggleValue();
				}
			}
			else
			{
				bool flag3 = evt.eventTypeId == EventBase<PointerUpEvent>.TypeId() || evt.eventTypeId == EventBase<ClickEvent>.TypeId();
				if (flag3)
				{
					IPointerEvent pointerEvent = (IPointerEvent)evt;
					bool flag4 = pointerEvent.button == 0;
					if (flag4)
					{
						this.ToggleValue();
					}
				}
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000DC44 File Offset: 0x0000BE44
		protected virtual void ToggleValue()
		{
			this.value = !this.value;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000DC58 File Offset: 0x0000BE58
		protected override void UpdateMixedValueContent()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				base.visualInput.pseudoStates &= ~PseudoStates.Checked;
				base.pseudoStates &= ~PseudoStates.Checked;
				this.m_CheckMark.RemoveFromHierarchy();
				base.visualInput.Add(base.mixedValueLabel);
				this.m_OriginalText = this.text;
				this.text = "";
			}
			else
			{
				base.mixedValueLabel.RemoveFromHierarchy();
				base.visualInput.Add(this.m_CheckMark);
				bool flag = this.m_OriginalText != null;
				if (flag)
				{
					this.text = this.m_OriginalText;
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000DD09 File Offset: 0x0000BF09
		internal override void RegisterEditingCallbacks()
		{
			base.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.StartEditing), TrickleDown.NoTrickleDown);
			base.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000DD34 File Offset: 0x0000BF34
		internal override void UnregisterEditingCallbacks()
		{
			base.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.StartEditing), TrickleDown.NoTrickleDown);
			base.UnregisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x04000116 RID: 278
		protected Label m_Label;

		// Token: 0x04000117 RID: 279
		protected readonly VisualElement m_CheckMark;

		// Token: 0x04000118 RID: 280
		internal Clickable m_Clickable;

		// Token: 0x04000119 RID: 281
		private string m_OriginalText;
	}
}
