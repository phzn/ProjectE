using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200007E RID: 126
	public class Button : TextElement
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x000150A8 File Offset: 0x000132A8
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x000150C0 File Offset: 0x000132C0
		public Clickable clickable
		{
			get
			{
				return this.m_Clickable;
			}
			set
			{
				bool flag = this.m_Clickable != null && this.m_Clickable.target == this;
				if (flag)
				{
					this.RemoveManipulator(this.m_Clickable);
				}
				this.m_Clickable = value;
				bool flag2 = this.m_Clickable != null;
				if (flag2)
				{
					this.AddManipulator(this.m_Clickable);
				}
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000563 RID: 1379 RVA: 0x0001511D File Offset: 0x0001331D
		// (remove) Token: 0x06000564 RID: 1380 RVA: 0x00015128 File Offset: 0x00013328
		[Obsolete("onClick is obsolete. Use clicked instead (UnityUpgradable) -> clicked", true)]
		public event Action onClick
		{
			add
			{
				this.clicked += value;
			}
			remove
			{
				this.clicked -= value;
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000565 RID: 1381 RVA: 0x00015134 File Offset: 0x00013334
		// (remove) Token: 0x06000566 RID: 1382 RVA: 0x00015170 File Offset: 0x00013370
		public event Action clicked
		{
			add
			{
				bool flag = this.m_Clickable == null;
				if (flag)
				{
					this.clickable = new Clickable(value);
				}
				else
				{
					this.m_Clickable.clicked += value;
				}
			}
			remove
			{
				bool flag = this.m_Clickable != null;
				if (flag)
				{
					this.m_Clickable.clicked -= value;
				}
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001519A File Offset: 0x0001339A
		public Button()
			: this(null)
		{
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000151A8 File Offset: 0x000133A8
		public Button(Action clickEvent)
		{
			base.AddToClassList(Button.ussClassName);
			this.clickable = new Clickable(clickEvent);
			base.focusable = true;
			base.tabIndex = 0;
			base.RegisterCallback<NavigationSubmitEvent>(new EventCallback<NavigationSubmitEvent>(this.OnNavigationSubmit), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000151FA File Offset: 0x000133FA
		private void OnNavigationSubmit(NavigationSubmitEvent evt)
		{
			Clickable clickable = this.clickable;
			if (clickable != null)
			{
				clickable.SimulateSingleClick(evt, 100);
			}
			evt.StopPropagation();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001521C File Offset: 0x0001341C
		protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
		{
			string text = this.text;
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				text = Button.NonEmptyString;
			}
			return base.MeasureTextSize(text, desiredWidth, widthMode, desiredHeight, heightMode);
		}

		// Token: 0x0400021F RID: 543
		public new static readonly string ussClassName = "unity-button";

		// Token: 0x04000220 RID: 544
		private Clickable m_Clickable;

		// Token: 0x04000221 RID: 545
		private static readonly string NonEmptyString = " ";

		// Token: 0x0200007F RID: 127
		public new class UxmlFactory : UxmlFactory<Button, Button.UxmlTraits>
		{
		}

		// Token: 0x02000080 RID: 128
		public new class UxmlTraits : TextElement.UxmlTraits
		{
			// Token: 0x0600056D RID: 1389 RVA: 0x00015272 File Offset: 0x00013472
			public UxmlTraits()
			{
				base.focusable.defaultValue = true;
			}
		}
	}
}
