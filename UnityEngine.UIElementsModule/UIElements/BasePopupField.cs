using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x0200005E RID: 94
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public abstract class BasePopupField<TValueType, TValueChoice> : BaseField<TValueType>
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000F540 File Offset: 0x0000D740
		protected TextElement textElement
		{
			get
			{
				return this.m_TextElement;
			}
		}

		// Token: 0x060003ED RID: 1005
		internal abstract string GetValueToDisplay();

		// Token: 0x060003EE RID: 1006
		internal abstract string GetListItemToDisplay(TValueType item);

		// Token: 0x060003EF RID: 1007
		internal abstract void AddMenuItems(IGenericMenu menu);

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000F558 File Offset: 0x0000D758
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000F570 File Offset: 0x0000D770
		public virtual List<TValueChoice> choices
		{
			get
			{
				return this.m_Choices;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				this.m_Choices = value;
				this.SetValueWithoutNotify(base.rawValue);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000F5A5 File Offset: 0x0000D7A5
		public override void SetValueWithoutNotify(TValueType newValue)
		{
			base.SetValueWithoutNotify(newValue);
			((INotifyValueChanged<string>)this.m_TextElement).SetValueWithoutNotify(this.GetValueToDisplay());
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000F5C4 File Offset: 0x0000D7C4
		public string text
		{
			get
			{
				return this.m_TextElement.text;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000F5E1 File Offset: 0x0000D7E1
		internal BasePopupField()
			: this(null)
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000F5EC File Offset: 0x0000D7EC
		internal BasePopupField(string label)
			: base(label, null)
		{
			base.AddToClassList(BasePopupField<TValueType, TValueChoice>.ussClassName);
			base.labelElement.AddToClassList(BasePopupField<TValueType, TValueChoice>.labelUssClassName);
			this.m_TextElement = new BasePopupField<TValueType, TValueChoice>.PopupTextElement
			{
				pickingMode = PickingMode.Ignore
			};
			this.m_TextElement.AddToClassList(BasePopupField<TValueType, TValueChoice>.textUssClassName);
			base.visualInput.AddToClassList(BasePopupField<TValueType, TValueChoice>.inputUssClassName);
			base.visualInput.Add(this.m_TextElement);
			this.m_ArrowElement = new VisualElement();
			this.m_ArrowElement.AddToClassList(BasePopupField<TValueType, TValueChoice>.arrowUssClassName);
			this.m_ArrowElement.pickingMode = PickingMode.Ignore;
			base.visualInput.Add(this.m_ArrowElement);
			this.choices = new List<TValueChoice>();
			base.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDownEvent), TrickleDown.NoTrickleDown);
			base.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUpEvent), TrickleDown.NoTrickleDown);
			base.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMoveEvent), TrickleDown.NoTrickleDown);
			base.RegisterCallback<MouseDownEvent>(delegate(MouseDownEvent e)
			{
				bool flag = e.button == 0;
				if (flag)
				{
					e.StopPropagation();
				}
			}, TrickleDown.NoTrickleDown);
			base.RegisterCallback<NavigationSubmitEvent>(new EventCallback<NavigationSubmitEvent>(this.OnNavigationSubmit), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000F725 File Offset: 0x0000D925
		private void OnPointerDownEvent(PointerDownEvent evt)
		{
			this.ProcessPointerDown<PointerDownEvent>(evt);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000F730 File Offset: 0x0000D930
		private void OnPointerUpEvent(PointerUpEvent evt)
		{
			bool flag = evt.button == 0 && this.ContainsPointer(evt.pointerId);
			if (flag)
			{
				evt.StopPropagation();
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000F764 File Offset: 0x0000D964
		private void OnPointerMoveEvent(PointerMoveEvent evt)
		{
			bool flag = evt.button == 0;
			if (flag)
			{
				bool flag2 = (evt.pressedButtons & 1) != 0;
				if (flag2)
				{
					this.ProcessPointerDown<PointerMoveEvent>(evt);
				}
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000F79C File Offset: 0x0000D99C
		private bool ContainsPointer(int pointerId)
		{
			VisualElement topElementUnderPointer = base.elementPanel.GetTopElementUnderPointer(pointerId);
			return this == topElementUnderPointer || base.visualInput == topElementUnderPointer;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		private void ProcessPointerDown<T>(PointerEventBase<T> evt) where T : PointerEventBase<T>, new()
		{
			bool flag = evt.button == 0;
			if (flag)
			{
				bool flag2 = this.ContainsPointer(evt.pointerId);
				if (flag2)
				{
					base.schedule.Execute(new Action(this.ShowMenu));
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000F81A File Offset: 0x0000DA1A
		private void OnNavigationSubmit(NavigationSubmitEvent evt)
		{
			this.ShowMenu();
			evt.StopPropagation();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000F82C File Offset: 0x0000DA2C
		internal void ShowMenu()
		{
			IGenericMenu genericMenu = ((this.createMenuCallback != null) ? this.createMenuCallback() : base.elementPanel.CreateMenu());
			this.AddMenuItems(genericMenu);
			genericMenu.DropDown(base.visualInput.worldBound, this, true);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000F878 File Offset: 0x0000DA78
		protected override void UpdateMixedValueContent()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				((INotifyValueChanged<string>)this.m_TextElement).SetValueWithoutNotify(BaseField<TValueType>.mixedValueString);
			}
			this.textElement.EnableInClassList(BaseField<TValueType>.mixedValueLabelUssClassName, base.showMixedValue);
		}

		// Token: 0x04000165 RID: 357
		internal List<TValueChoice> m_Choices;

		// Token: 0x04000166 RID: 358
		private TextElement m_TextElement;

		// Token: 0x04000167 RID: 359
		private VisualElement m_ArrowElement;

		// Token: 0x04000168 RID: 360
		internal Func<TValueChoice, string> m_FormatSelectedValueCallback;

		// Token: 0x04000169 RID: 361
		internal Func<TValueChoice, string> m_FormatListItemCallback;

		// Token: 0x0400016A RID: 362
		internal Func<IGenericMenu> createMenuCallback;

		// Token: 0x0400016B RID: 363
		public new static readonly string ussClassName = "unity-base-popup-field";

		// Token: 0x0400016C RID: 364
		public static readonly string textUssClassName = BasePopupField<TValueType, TValueChoice>.ussClassName + "__text";

		// Token: 0x0400016D RID: 365
		public static readonly string arrowUssClassName = BasePopupField<TValueType, TValueChoice>.ussClassName + "__arrow";

		// Token: 0x0400016E RID: 366
		public new static readonly string labelUssClassName = BasePopupField<TValueType, TValueChoice>.ussClassName + "__label";

		// Token: 0x0400016F RID: 367
		public new static readonly string inputUssClassName = BasePopupField<TValueType, TValueChoice>.ussClassName + "__input";

		// Token: 0x0200005F RID: 95
		private class PopupTextElement : TextElement
		{
			// Token: 0x060003FF RID: 1023 RVA: 0x0000F924 File Offset: 0x0000DB24
			protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
			{
				string text = this.text;
				bool flag = string.IsNullOrEmpty(text);
				if (flag)
				{
					text = " ";
				}
				return base.MeasureTextSize(text, desiredWidth, widthMode, desiredHeight, heightMode);
			}
		}
	}
}
