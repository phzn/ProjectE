using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000B9 RID: 185
	public class GroupBox : BindableElement, IGroupBox
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00018B00 File Offset: 0x00016D00
		internal Label titleLabel
		{
			get
			{
				return this.m_TitleLabel;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00018B08 File Offset: 0x00016D08
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x00018B1C File Offset: 0x00016D1C
		public string text
		{
			get
			{
				Label titleLabel = this.m_TitleLabel;
				return (titleLabel != null) ? titleLabel.text : null;
			}
			set
			{
				bool flag = !string.IsNullOrEmpty(value);
				if (flag)
				{
					bool flag2 = this.m_TitleLabel == null;
					if (flag2)
					{
						this.m_TitleLabel = new Label(value);
						this.m_TitleLabel.AddToClassList(GroupBox.labelUssClassName);
						base.Insert(0, this.m_TitleLabel);
					}
					this.m_TitleLabel.text = value;
				}
				else
				{
					bool flag3 = this.m_TitleLabel != null;
					if (flag3)
					{
						this.m_TitleLabel.RemoveFromHierarchy();
						this.m_TitleLabel = null;
					}
				}
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00018BA4 File Offset: 0x00016DA4
		public GroupBox()
			: this(null)
		{
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00018BAF File Offset: 0x00016DAF
		public GroupBox(string text)
		{
			base.AddToClassList(GroupBox.ussClassName);
			this.text = text;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void IGroupBox.OnOptionAdded(IGroupBoxOption option)
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void IGroupBox.OnOptionRemoved(IGroupBoxOption option)
		{
		}

		// Token: 0x040002CD RID: 717
		public static readonly string ussClassName = "unity-group-box";

		// Token: 0x040002CE RID: 718
		public static readonly string labelUssClassName = GroupBox.ussClassName + "__label";

		// Token: 0x040002CF RID: 719
		private Label m_TitleLabel;

		// Token: 0x020000BA RID: 186
		public new class UxmlFactory : UxmlFactory<GroupBox, GroupBox.UxmlTraits>
		{
		}

		// Token: 0x020000BB RID: 187
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x06000676 RID: 1654 RVA: 0x00018BF6 File Offset: 0x00016DF6
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				((GroupBox)ve).text = this.m_Text.GetValueFromBag(bag, cc);
			}

			// Token: 0x040002D0 RID: 720
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};
		}
	}
}
