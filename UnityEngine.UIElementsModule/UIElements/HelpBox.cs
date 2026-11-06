using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000C1 RID: 193
	public class HelpBox : VisualElement
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00018F98 File Offset: 0x00017198
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x00018FB5 File Offset: 0x000171B5
		public string text
		{
			get
			{
				return this.m_Label.text;
			}
			set
			{
				this.m_Label.text = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00018FC8 File Offset: 0x000171C8
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x00018FE0 File Offset: 0x000171E0
		public HelpBoxMessageType messageType
		{
			get
			{
				return this.m_HelpBoxMessageType;
			}
			set
			{
				bool flag = value != this.m_HelpBoxMessageType;
				if (flag)
				{
					this.m_HelpBoxMessageType = value;
					this.UpdateIcon(value);
				}
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001900F File Offset: 0x0001720F
		public HelpBox()
			: this(string.Empty, HelpBoxMessageType.None)
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00019020 File Offset: 0x00017220
		public HelpBox(string text, HelpBoxMessageType messageType)
		{
			base.AddToClassList(HelpBox.ussClassName);
			this.m_HelpBoxMessageType = messageType;
			this.m_Label = new Label(text);
			this.m_Label.AddToClassList(HelpBox.labelUssClassName);
			base.Add(this.m_Label);
			this.m_Icon = new VisualElement();
			this.m_Icon.AddToClassList(HelpBox.iconUssClassName);
			this.UpdateIcon(messageType);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00019098 File Offset: 0x00017298
		private string GetIconClass(HelpBoxMessageType messageType)
		{
			string text;
			switch (messageType)
			{
			case HelpBoxMessageType.Info:
				text = HelpBox.iconInfoUssClassName;
				break;
			case HelpBoxMessageType.Warning:
				text = HelpBox.iconwarningUssClassName;
				break;
			case HelpBoxMessageType.Error:
				text = HelpBox.iconErrorUssClassName;
				break;
			default:
				text = null;
				break;
			}
			return text;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000190E0 File Offset: 0x000172E0
		private void UpdateIcon(HelpBoxMessageType messageType)
		{
			bool flag = !string.IsNullOrEmpty(this.m_IconClass);
			if (flag)
			{
				this.m_Icon.RemoveFromClassList(this.m_IconClass);
			}
			this.m_IconClass = this.GetIconClass(messageType);
			bool flag2 = this.m_IconClass == null;
			if (flag2)
			{
				this.m_Icon.RemoveFromHierarchy();
			}
			else
			{
				this.m_Icon.AddToClassList(this.m_IconClass);
				bool flag3 = this.m_Icon.parent == null;
				if (flag3)
				{
					base.Insert(0, this.m_Icon);
				}
			}
		}

		// Token: 0x040002D9 RID: 729
		public static readonly string ussClassName = "unity-help-box";

		// Token: 0x040002DA RID: 730
		public static readonly string labelUssClassName = HelpBox.ussClassName + "__label";

		// Token: 0x040002DB RID: 731
		public static readonly string iconUssClassName = HelpBox.ussClassName + "__icon";

		// Token: 0x040002DC RID: 732
		public static readonly string iconInfoUssClassName = HelpBox.iconUssClassName + "--info";

		// Token: 0x040002DD RID: 733
		public static readonly string iconwarningUssClassName = HelpBox.iconUssClassName + "--warning";

		// Token: 0x040002DE RID: 734
		public static readonly string iconErrorUssClassName = HelpBox.iconUssClassName + "--error";

		// Token: 0x040002DF RID: 735
		private HelpBoxMessageType m_HelpBoxMessageType;

		// Token: 0x040002E0 RID: 736
		private VisualElement m_Icon;

		// Token: 0x040002E1 RID: 737
		private string m_IconClass;

		// Token: 0x040002E2 RID: 738
		private Label m_Label;

		// Token: 0x020000C2 RID: 194
		public new class UxmlFactory : UxmlFactory<HelpBox, HelpBox.UxmlTraits>
		{
		}

		// Token: 0x020000C3 RID: 195
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x06000699 RID: 1689 RVA: 0x000191F8 File Offset: 0x000173F8
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				HelpBox helpBox = ve as HelpBox;
				helpBox.text = this.m_Text.GetValueFromBag(bag, cc);
				helpBox.messageType = this.m_MessageType.GetValueFromBag(bag, cc);
			}

			// Token: 0x040002E3 RID: 739
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};

			// Token: 0x040002E4 RID: 740
			private UxmlEnumAttributeDescription<HelpBoxMessageType> m_MessageType = new UxmlEnumAttributeDescription<HelpBoxMessageType>
			{
				name = "message-type",
				defaultValue = HelpBoxMessageType.None
			};
		}
	}
}
