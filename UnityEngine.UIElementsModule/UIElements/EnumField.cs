using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x020000A9 RID: 169
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class EnumField : BaseField<Enum>
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00016C5F File Offset: 0x00014E5F
		internal Type type
		{
			get
			{
				return this.m_EnumType;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00016C67 File Offset: 0x00014E67
		internal bool includeObsoleteValues
		{
			get
			{
				return this.m_IncludeObsoleteValues;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00016C70 File Offset: 0x00014E70
		public string text
		{
			get
			{
				return this.m_TextElement.text;
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00016C90 File Offset: 0x00014E90
		private void Initialize(Enum defaultValue)
		{
			this.m_TextElement = new TextElement();
			this.m_TextElement.AddToClassList(EnumField.textUssClassName);
			this.m_TextElement.pickingMode = PickingMode.Ignore;
			base.visualInput.Add(this.m_TextElement);
			this.m_ArrowElement = new VisualElement();
			this.m_ArrowElement.AddToClassList(EnumField.arrowUssClassName);
			this.m_ArrowElement.pickingMode = PickingMode.Ignore;
			base.visualInput.Add(this.m_ArrowElement);
			bool flag = defaultValue != null;
			if (flag)
			{
				this.Init(defaultValue);
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00016D26 File Offset: 0x00014F26
		public EnumField()
			: this(null, null)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00016D32 File Offset: 0x00014F32
		public EnumField(Enum defaultValue)
			: this(null, defaultValue)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00016D40 File Offset: 0x00014F40
		public EnumField(string label, Enum defaultValue = null)
			: base(label, null)
		{
			base.AddToClassList(EnumField.ussClassName);
			base.labelElement.AddToClassList(EnumField.labelUssClassName);
			base.visualInput.AddToClassList(EnumField.inputUssClassName);
			this.Initialize(defaultValue);
			base.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDownEvent), TrickleDown.NoTrickleDown);
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

		// Token: 0x0600060A RID: 1546 RVA: 0x00016DF0 File Offset: 0x00014FF0
		public void Init(Enum defaultValue)
		{
			this.Init(defaultValue, false);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00016DFC File Offset: 0x00014FFC
		public void Init(Enum defaultValue, bool includeObsoleteValues)
		{
			bool flag = defaultValue == null;
			if (flag)
			{
				throw new ArgumentNullException("defaultValue");
			}
			this.m_IncludeObsoleteValues = includeObsoleteValues;
			this.PopulateDataFromType(defaultValue.GetType());
			bool flag2 = !object.Equals(base.rawValue, defaultValue);
			if (flag2)
			{
				this.SetValueWithoutNotify(defaultValue);
			}
			else
			{
				this.UpdateValueLabel(defaultValue);
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00016E57 File Offset: 0x00015057
		internal void PopulateDataFromType(Type enumType)
		{
			this.m_EnumType = enumType;
			this.m_EnumData = EnumDataUtility.GetCachedEnumData(this.m_EnumType, this.includeObsoleteValues ? EnumDataUtility.CachedType.IncludeObsoleteExceptErrors : EnumDataUtility.CachedType.ExcludeObsolete, null);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00016E80 File Offset: 0x00015080
		public override void SetValueWithoutNotify(Enum newValue)
		{
			bool flag = !object.Equals(base.rawValue, newValue);
			if (flag)
			{
				base.SetValueWithoutNotify(newValue);
				bool flag2 = this.m_EnumType == null;
				if (!flag2)
				{
					this.UpdateValueLabel(newValue);
				}
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00016EC8 File Offset: 0x000150C8
		private void UpdateValueLabel(Enum value)
		{
			int num = Array.IndexOf<Enum>(this.m_EnumData.values, value);
			bool flag = (num >= 0) & (num < this.m_EnumData.values.Length);
			if (flag)
			{
				this.m_TextElement.text = this.m_EnumData.displayNames[num];
			}
			else
			{
				this.m_TextElement.text = string.Empty;
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00016F34 File Offset: 0x00015134
		private void OnPointerDownEvent(PointerDownEvent evt)
		{
			this.ProcessPointerDown<PointerDownEvent>(evt);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00016F40 File Offset: 0x00015140
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

		// Token: 0x06000611 RID: 1553 RVA: 0x00016F78 File Offset: 0x00015178
		private bool ContainsPointer(int pointerId)
		{
			VisualElement topElementUnderPointer = base.elementPanel.GetTopElementUnderPointer(pointerId);
			return this == topElementUnderPointer || base.visualInput == topElementUnderPointer;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00016FA8 File Offset: 0x000151A8
		private void ProcessPointerDown<T>(PointerEventBase<T> evt) where T : PointerEventBase<T>, new()
		{
			bool flag = evt.button == 0;
			if (flag)
			{
				bool flag2 = this.ContainsPointer(evt.pointerId);
				if (flag2)
				{
					this.ShowMenu();
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00016FE5 File Offset: 0x000151E5
		private void OnNavigationSubmit(NavigationSubmitEvent evt)
		{
			this.ShowMenu();
			evt.StopPropagation();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00016FF8 File Offset: 0x000151F8
		internal void ShowMenu()
		{
			bool flag = this.m_EnumType == null;
			if (!flag)
			{
				IGenericMenu genericMenu = ((this.createMenuCallback != null) ? this.createMenuCallback() : base.elementPanel.CreateMenu());
				int num = Array.IndexOf<Enum>(this.m_EnumData.values, this.value);
				for (int i = 0; i < this.m_EnumData.values.Length; i++)
				{
					bool flag2 = num == i;
					genericMenu.AddItem(this.m_EnumData.displayNames[i], flag2, delegate(object contentView)
					{
						this.ChangeValueFromMenu(contentView);
					}, this.m_EnumData.values[i]);
				}
				genericMenu.DropDown(base.visualInput.worldBound, this, true);
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000170BC File Offset: 0x000152BC
		private void ChangeValueFromMenu(object menuItem)
		{
			this.value = menuItem as Enum;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000170CC File Offset: 0x000152CC
		protected override void UpdateMixedValueContent()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				this.m_TextElement.text = BaseField<Enum>.mixedValueString;
			}
			else
			{
				this.UpdateValueLabel(this.value);
			}
			this.m_TextElement.EnableInClassList(EnumField.labelUssClassName, base.showMixedValue);
			this.m_TextElement.EnableInClassList(BaseField<Enum>.mixedValueLabelUssClassName, base.showMixedValue);
		}

		// Token: 0x04000292 RID: 658
		private Type m_EnumType;

		// Token: 0x04000293 RID: 659
		private bool m_IncludeObsoleteValues;

		// Token: 0x04000294 RID: 660
		private TextElement m_TextElement;

		// Token: 0x04000295 RID: 661
		private VisualElement m_ArrowElement;

		// Token: 0x04000296 RID: 662
		private EnumData m_EnumData;

		// Token: 0x04000297 RID: 663
		internal Func<IGenericMenu> createMenuCallback;

		// Token: 0x04000298 RID: 664
		public new static readonly string ussClassName = "unity-enum-field";

		// Token: 0x04000299 RID: 665
		public static readonly string textUssClassName = EnumField.ussClassName + "__text";

		// Token: 0x0400029A RID: 666
		public static readonly string arrowUssClassName = EnumField.ussClassName + "__arrow";

		// Token: 0x0400029B RID: 667
		public new static readonly string labelUssClassName = EnumField.ussClassName + "__label";

		// Token: 0x0400029C RID: 668
		public new static readonly string inputUssClassName = EnumField.ussClassName + "__input";

		// Token: 0x020000AA RID: 170
		public new class UxmlFactory : UxmlFactory<EnumField, EnumField.UxmlTraits>
		{
		}

		// Token: 0x020000AB RID: 171
		public new class UxmlTraits : BaseField<Enum>.UxmlTraits
		{
			// Token: 0x0600061A RID: 1562 RVA: 0x000171B4 File Offset: 0x000153B4
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Type type;
				Enum @enum;
				bool flag2;
				bool flag = EnumFieldHelpers.ExtractValue(bag, cc, out type, out @enum, out flag2);
				if (flag)
				{
					EnumField enumField = (EnumField)ve;
					enumField.Init(@enum, flag2);
				}
				else
				{
					bool flag3 = null != type;
					if (flag3)
					{
						EnumField enumField2 = (EnumField)ve;
						enumField2.m_EnumType = type;
						bool flag4 = enumField2.m_EnumType != null;
						if (flag4)
						{
							enumField2.PopulateDataFromType(enumField2.m_EnumType);
						}
						enumField2.value = null;
					}
					else
					{
						EnumField enumField3 = (EnumField)ve;
						enumField3.m_EnumType = null;
						enumField3.value = null;
					}
				}
			}

			// Token: 0x0400029D RID: 669
			private UxmlTypeAttributeDescription<Enum> m_Type = EnumFieldHelpers.type;

			// Token: 0x0400029E RID: 670
			private UxmlStringAttributeDescription m_Value = EnumFieldHelpers.value;

			// Token: 0x0400029F RID: 671
			private UxmlBoolAttributeDescription m_IncludeObsoleteValues = EnumFieldHelpers.includeObsoleteValues;
		}
	}
}
