using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000132 RID: 306
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public abstract class TextValueField<TValueType> : TextInputBaseField<TValueType>, IValueField<TValueType>
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0002896B File Offset: 0x00026B6B
		private TextValueField<TValueType>.TextValueInput textValueInput
		{
			get
			{
				return (TextValueField<TValueType>.TextValueInput)base.textInputBase;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00028978 File Offset: 0x00026B78
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x00028988 File Offset: 0x00026B88
		public string formatString
		{
			get
			{
				return this.textValueInput.formatString;
			}
			set
			{
				bool flag = this.textValueInput.formatString != value;
				if (flag)
				{
					this.textValueInput.formatString = value;
					base.textEdition.UpdateText(this.ValueToString(base.rawValue));
				}
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000289D2 File Offset: 0x00026BD2
		protected TextValueField(int maxLength, TextValueField<TValueType>.TextValueInput textValueInput)
			: this(null, maxLength, textValueInput)
		{
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000289DF File Offset: 0x00026BDF
		protected TextValueField(string label, int maxLength, TextValueField<TValueType>.TextValueInput textValueInput)
			: base(label, maxLength, '\0', textValueInput)
		{
			base.textEdition.UpdateText(this.ValueToString(base.rawValue));
			base.onIsReadOnlyChanged += this.OnIsReadOnlyChanged;
		}

		// Token: 0x06000A21 RID: 2593
		public abstract void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TValueType startValue);

		// Token: 0x06000A22 RID: 2594 RVA: 0x00028A18 File Offset: 0x00026C18
		public void StartDragging()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				this.value = default(TValueType);
			}
			this.textValueInput.StartDragging();
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00028A4E File Offset: 0x00026C4E
		public void StopDragging()
		{
			this.textValueInput.StopDragging();
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00028A5D File Offset: 0x00026C5D
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x00028A65 File Offset: 0x00026C65
		public override TValueType value
		{
			get
			{
				return base.value;
			}
			set
			{
				base.value = value;
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00028A70 File Offset: 0x00026C70
		internal override void UpdateValueFromText()
		{
			this.m_UpdateTextFromValue = false;
			try
			{
				this.value = this.StringToValue(base.text);
			}
			finally
			{
				this.m_UpdateTextFromValue = true;
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00028AB8 File Offset: 0x00026CB8
		internal override void UpdateTextFromValue()
		{
			bool updateTextFromValue = this.m_UpdateTextFromValue;
			if (updateTextFromValue)
			{
				base.text = this.ValueToString(base.rawValue);
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00028AE5 File Offset: 0x00026CE5
		private void OnIsReadOnlyChanged(bool newValue)
		{
			this.EnableLabelDragger(!newValue);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000960A File Offset: 0x0000780A
		internal virtual bool CanTryParse(string textString)
		{
			return false;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00028AF3 File Offset: 0x00026CF3
		protected void AddLabelDragger<TDraggerType>()
		{
			this.m_Dragger = new FieldMouseDragger<TDraggerType>((IValueField<TDraggerType>)this);
			this.EnableLabelDragger(!base.isReadOnly);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00028B18 File Offset: 0x00026D18
		private void EnableLabelDragger(bool enable)
		{
			bool flag = this.m_Dragger != null;
			if (flag)
			{
				this.m_Dragger.SetDragZone(enable ? base.labelElement : null);
				base.labelElement.EnableInClassList(BaseField<TValueType>.labelDraggerVariantUssClassName, enable);
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00028B60 File Offset: 0x00026D60
		public override void SetValueWithoutNotify(TValueType newValue)
		{
			bool flag = this.m_ForceUpdateDisplay || (this.m_UpdateTextFromValue && !EqualityComparer<TValueType>.Default.Equals(base.rawValue, newValue));
			base.SetValueWithoutNotify(newValue);
			bool flag2 = flag;
			if (flag2)
			{
				base.textEdition.UpdateText(this.ValueToString(base.rawValue));
			}
			this.m_ForceUpdateDisplay = false;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00028BC8 File Offset: 0x00026DC8
		[EventInterest(new Type[]
		{
			typeof(BlurEvent),
			typeof(FocusEvent)
		})]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			bool flag = evt == null;
			if (!flag)
			{
				bool flag2 = evt.eventTypeId == EventBase<BlurEvent>.TypeId();
				if (flag2)
				{
					bool showMixedValue = base.showMixedValue;
					if (showMixedValue)
					{
						this.UpdateMixedValueContent();
					}
					else
					{
						bool flag3 = string.IsNullOrEmpty(base.text);
						if (flag3)
						{
							base.textInputBase.UpdateTextFromValue();
						}
						else
						{
							base.textInputBase.UpdateValueFromText();
							base.textInputBase.UpdateTextFromValue();
						}
					}
				}
				else
				{
					bool flag4 = evt.eventTypeId == EventBase<FocusEvent>.TypeId();
					if (flag4)
					{
						bool flag5 = base.showMixedValue && base.textInputBase.textElement.hasFocus;
						if (flag5)
						{
							base.textInputBase.text = "";
						}
					}
				}
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00028C98 File Offset: 0x00026E98
		internal override void OnViewDataReady()
		{
			this.m_ForceUpdateDisplay = true;
			base.OnViewDataReady();
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00028CA9 File Offset: 0x00026EA9
		internal override void RegisterEditingCallbacks()
		{
			base.RegisterEditingCallbacks();
			base.labelElement.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			base.labelElement.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00028CE5 File Offset: 0x00026EE5
		internal override void UnregisterEditingCallbacks()
		{
			base.UnregisterEditingCallbacks();
			base.labelElement.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			base.labelElement.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x040004DE RID: 1246
		private BaseFieldMouseDragger m_Dragger;

		// Token: 0x040004DF RID: 1247
		private bool m_ForceUpdateDisplay;

		// Token: 0x040004E0 RID: 1248
		internal const int kMaxValueFieldLength = 1000;

		// Token: 0x02000133 RID: 307
		protected abstract class TextValueInput : TextInputBaseField<TValueType>.TextInputBase
		{
			// Token: 0x170001ED RID: 493
			// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00028D21 File Offset: 0x00026F21
			private TextValueField<TValueType> textValueFieldParent
			{
				get
				{
					return (TextValueField<TValueType>)base.parent;
				}
			}

			// Token: 0x06000A32 RID: 2610 RVA: 0x00028D2E File Offset: 0x00026F2E
			protected TextValueInput()
			{
				base.textEdition.AcceptCharacter = new Func<char, bool>(this.AcceptCharacter);
			}

			// Token: 0x06000A33 RID: 2611 RVA: 0x00028D54 File Offset: 0x00026F54
			internal override bool AcceptCharacter(char c)
			{
				return base.AcceptCharacter(c) && c != '\0' && this.allowedCharacters.IndexOf(c) != -1;
			}

			// Token: 0x170001EE RID: 494
			// (get) Token: 0x06000A34 RID: 2612
			protected abstract string allowedCharacters { get; }

			// Token: 0x170001EF RID: 495
			// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00028D87 File Offset: 0x00026F87
			// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00028D8F File Offset: 0x00026F8F
			public string formatString { get; set; }

			// Token: 0x06000A37 RID: 2615
			public abstract void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TValueType startValue);

			// Token: 0x06000A38 RID: 2616 RVA: 0x00028D98 File Offset: 0x00026F98
			public void StartDragging()
			{
				base.isDragging = true;
				base.SelectNone();
				base.MarkDirtyRepaint();
			}

			// Token: 0x06000A39 RID: 2617 RVA: 0x00028DB4 File Offset: 0x00026FB4
			public void StopDragging()
			{
				bool isDelayed = this.textValueFieldParent.isDelayed;
				if (isDelayed)
				{
					base.UpdateValueFromText();
				}
				base.isDragging = false;
				base.SelectAll();
				base.MarkDirtyRepaint();
			}

			// Token: 0x06000A3A RID: 2618
			protected abstract string ValueToString(TValueType value);

			// Token: 0x06000A3B RID: 2619 RVA: 0x00028DF0 File Offset: 0x00026FF0
			protected override TValueType StringToValue(string str)
			{
				return base.StringToValue(str);
			}
		}
	}
}
