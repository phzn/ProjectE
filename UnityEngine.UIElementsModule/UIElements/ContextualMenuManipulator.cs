using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000053 RID: 83
	public class ContextualMenuManipulator : MouseManipulator
	{
		// Token: 0x06000386 RID: 902 RVA: 0x0000D6C4 File Offset: 0x0000B8C4
		public ContextualMenuManipulator(Action<ContextualMenuPopulateEvent> menuBuilder)
		{
			this.m_MenuBuilder = menuBuilder;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.RightMouse
			});
			bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
			if (flag)
			{
				base.activators.Add(new ManipulatorActivationFilter
				{
					button = MouseButton.LeftMouse,
					modifiers = EventModifiers.Control
				});
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000D740 File Offset: 0x0000B940
		protected override void RegisterCallbacksOnTarget()
		{
			bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
			if (flag)
			{
				base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDownEventOSX), TrickleDown.NoTrickleDown);
				base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUpEventOSX), TrickleDown.NoTrickleDown);
			}
			else
			{
				base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUpDownEvent), TrickleDown.NoTrickleDown);
			}
			base.target.RegisterCallback<KeyUpEvent>(new EventCallback<KeyUpEvent>(this.OnKeyUpEvent), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<ContextualMenuPopulateEvent>(new EventCallback<ContextualMenuPopulateEvent>(this.OnContextualMenuEvent), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000D7E8 File Offset: 0x0000B9E8
		protected override void UnregisterCallbacksFromTarget()
		{
			bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
			if (flag)
			{
				base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDownEventOSX), TrickleDown.NoTrickleDown);
				base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUpEventOSX), TrickleDown.NoTrickleDown);
			}
			else
			{
				base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUpDownEvent), TrickleDown.NoTrickleDown);
			}
			base.target.UnregisterCallback<KeyUpEvent>(new EventCallback<KeyUpEvent>(this.OnKeyUpEvent), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<ContextualMenuPopulateEvent>(new EventCallback<ContextualMenuPopulateEvent>(this.OnContextualMenuEvent), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000D890 File Offset: 0x0000BA90
		private void OnMouseUpDownEvent(IMouseEvent evt)
		{
			bool flag = base.CanStartManipulation(evt);
			if (flag)
			{
				this.DoDisplayMenu(evt as EventBase);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		private void OnMouseDownEventOSX(MouseDownEvent evt)
		{
			BaseVisualElementPanel elementPanel = base.target.elementPanel;
			bool flag = ((elementPanel != null) ? elementPanel.contextualMenuManager : null) != null;
			if (flag)
			{
				base.target.elementPanel.contextualMenuManager.displayMenuHandledOSX = false;
			}
			bool isDefaultPrevented = evt.isDefaultPrevented;
			if (!isDefaultPrevented)
			{
				this.OnMouseUpDownEvent(evt);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000D914 File Offset: 0x0000BB14
		private void OnMouseUpEventOSX(MouseUpEvent evt)
		{
			BaseVisualElementPanel elementPanel = base.target.elementPanel;
			bool flag = ((elementPanel != null) ? elementPanel.contextualMenuManager : null) != null && base.target.elementPanel.contextualMenuManager.displayMenuHandledOSX;
			if (!flag)
			{
				this.OnMouseUpDownEvent(evt);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000D964 File Offset: 0x0000BB64
		private void OnKeyUpEvent(KeyUpEvent evt)
		{
			bool flag = evt.keyCode == KeyCode.Menu;
			if (flag)
			{
				this.DoDisplayMenu(evt);
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000D990 File Offset: 0x0000BB90
		private void DoDisplayMenu(EventBase evt)
		{
			BaseVisualElementPanel elementPanel = base.target.elementPanel;
			bool flag = ((elementPanel != null) ? elementPanel.contextualMenuManager : null) != null;
			if (flag)
			{
				base.target.elementPanel.contextualMenuManager.DisplayMenu(evt, base.target);
				evt.StopPropagation();
				evt.PreventDefault();
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000D9E9 File Offset: 0x0000BBE9
		private void OnContextualMenuEvent(ContextualMenuPopulateEvent evt)
		{
			Action<ContextualMenuPopulateEvent> menuBuilder = this.m_MenuBuilder;
			if (menuBuilder != null)
			{
				menuBuilder(evt);
			}
		}

		// Token: 0x04000115 RID: 277
		private Action<ContextualMenuPopulateEvent> m_MenuBuilder;
	}
}
