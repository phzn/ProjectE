using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020000A5 RID: 165
	public class DropdownField : PopupField<string>
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x00016A5A File Offset: 0x00014C5A
		public DropdownField()
			: this(null)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00016A65 File Offset: 0x00014C65
		public DropdownField(string label)
			: base(label)
		{
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00016A70 File Offset: 0x00014C70
		public DropdownField(List<string> choices, string defaultValue, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
			: this(null, choices, defaultValue, formatSelectedValueCallback, formatListItemCallback)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00016A80 File Offset: 0x00014C80
		public DropdownField(string label, List<string> choices, string defaultValue, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
			: base(label, choices, defaultValue, formatSelectedValueCallback, formatListItemCallback)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00016A91 File Offset: 0x00014C91
		public DropdownField(List<string> choices, int defaultIndex, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
			: this(null, choices, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00016AA1 File Offset: 0x00014CA1
		public DropdownField(string label, List<string> choices, int defaultIndex, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null)
			: base(label, choices, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
		{
		}

		// Token: 0x020000A6 RID: 166
		public new class UxmlFactory : UxmlFactory<DropdownField, DropdownField.UxmlTraits>
		{
		}

		// Token: 0x020000A7 RID: 167
		public new class UxmlTraits : BaseField<string>.UxmlTraits
		{
			// Token: 0x060005FF RID: 1535 RVA: 0x00016ABC File Offset: 0x00014CBC
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				DropdownField dropdownField = (DropdownField)ve;
				List<string> list = BaseField<string>.UxmlTraits.ParseChoiceList(this.m_Choices.GetValueFromBag(bag, cc));
				bool flag = list != null;
				if (flag)
				{
					dropdownField.choices = list;
				}
				dropdownField.index = this.m_Index.GetValueFromBag(bag, cc);
			}

			// Token: 0x0400028D RID: 653
			private UxmlIntAttributeDescription m_Index = new UxmlIntAttributeDescription
			{
				name = "index",
				defaultValue = -1
			};

			// Token: 0x0400028E RID: 654
			private UxmlStringAttributeDescription m_Choices = new UxmlStringAttributeDescription
			{
				name = "choices"
			};
		}
	}
}
