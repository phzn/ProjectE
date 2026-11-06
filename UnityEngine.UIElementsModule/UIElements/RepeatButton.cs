using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200011A RID: 282
	public class RepeatButton : TextElement
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x000249B7 File Offset: 0x00022BB7
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x000249C0 File Offset: 0x00022BC0
		internal bool acceptClicksIfDisabled
		{
			get
			{
				return this.m_AcceptClicksIfDisabled;
			}
			set
			{
				bool flag = this.m_AcceptClicksIfDisabled == value;
				if (!flag)
				{
					this.m_AcceptClicksIfDisabled = value;
					bool flag2 = this.m_Clickable != null;
					if (flag2)
					{
						this.m_Clickable.acceptClicksIfDisabled = value;
					}
				}
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000249FE File Offset: 0x00022BFE
		public RepeatButton()
		{
			base.AddToClassList(RepeatButton.ussClassName);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00024A14 File Offset: 0x00022C14
		public RepeatButton(Action clickEvent, long delay, long interval)
			: this()
		{
			this.SetAction(clickEvent, delay, interval);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00024A28 File Offset: 0x00022C28
		public void SetAction(Action clickEvent, long delay, long interval)
		{
			this.RemoveManipulator(this.m_Clickable);
			this.m_Clickable = new Clickable(clickEvent, delay, interval);
			this.AddManipulator(this.m_Clickable);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00024A53 File Offset: 0x00022C53
		internal void AddAction(Action clickEvent)
		{
			this.m_Clickable.clicked += clickEvent;
		}

		// Token: 0x04000448 RID: 1096
		private Clickable m_Clickable;

		// Token: 0x04000449 RID: 1097
		private bool m_AcceptClicksIfDisabled;

		// Token: 0x0400044A RID: 1098
		public new static readonly string ussClassName = "unity-repeat-button";

		// Token: 0x0200011B RID: 283
		public new class UxmlFactory : UxmlFactory<RepeatButton, RepeatButton.UxmlTraits>
		{
		}

		// Token: 0x0200011C RID: 284
		public new class UxmlTraits : TextElement.UxmlTraits
		{
			// Token: 0x0600097B RID: 2427 RVA: 0x00024A78 File Offset: 0x00022C78
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				RepeatButton repeatButton = (RepeatButton)ve;
				repeatButton.SetAction(null, this.m_Delay.GetValueFromBag(bag, cc), this.m_Interval.GetValueFromBag(bag, cc));
			}

			// Token: 0x0400044B RID: 1099
			private UxmlLongAttributeDescription m_Delay = new UxmlLongAttributeDescription
			{
				name = "delay"
			};

			// Token: 0x0400044C RID: 1100
			private UxmlLongAttributeDescription m_Interval = new UxmlLongAttributeDescription
			{
				name = "interval"
			};
		}
	}
}
