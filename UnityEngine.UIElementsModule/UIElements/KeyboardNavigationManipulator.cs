using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements
{
	// Token: 0x02000271 RID: 625
	public class KeyboardNavigationManipulator : Manipulator
	{
		// Token: 0x060011BF RID: 4543 RVA: 0x000408DC File Offset: 0x0003EADC
		public KeyboardNavigationManipulator(Action<KeyboardNavigationOperation, EventBase> action)
		{
			this.m_Action = action;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000408F0 File Offset: 0x0003EAF0
		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<NavigationMoveEvent>(new EventCallback<NavigationMoveEvent>(this.OnNavigationMove), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<NavigationSubmitEvent>(new EventCallback<NavigationSubmitEvent>(this.OnNavigationSubmit), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<NavigationCancelEvent>(new EventCallback<NavigationCancelEvent>(this.OnNavigationCancel), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00040964 File Offset: 0x0003EB64
		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<NavigationMoveEvent>(new EventCallback<NavigationMoveEvent>(this.OnNavigationMove), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<NavigationSubmitEvent>(new EventCallback<NavigationSubmitEvent>(this.OnNavigationSubmit), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<NavigationCancelEvent>(new EventCallback<NavigationCancelEvent>(this.OnNavigationCancel), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000409D8 File Offset: 0x0003EBD8
		internal void OnKeyDown(KeyDownEvent evt)
		{
			KeyboardNavigationManipulator.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.evt = evt;
			KeyboardNavigationOperation keyboardNavigationOperation = KeyboardNavigationManipulator.<OnKeyDown>g__GetOperation|4_0(ref CS$<>8__locals1);
			bool flag = keyboardNavigationOperation > KeyboardNavigationOperation.None;
			if (flag)
			{
				this.Invoke(keyboardNavigationOperation, CS$<>8__locals1.evt);
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00040A0F File Offset: 0x0003EC0F
		private void OnNavigationCancel(NavigationCancelEvent evt)
		{
			this.Invoke(KeyboardNavigationOperation.Cancel, evt);
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00040A1B File Offset: 0x0003EC1B
		private void OnNavigationSubmit(NavigationSubmitEvent evt)
		{
			this.Invoke(KeyboardNavigationOperation.Submit, evt);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00040A28 File Offset: 0x0003EC28
		private void OnNavigationMove(NavigationMoveEvent evt)
		{
			switch (evt.direction)
			{
			case NavigationMoveEvent.Direction.Left:
				this.Invoke(KeyboardNavigationOperation.MoveLeft, evt);
				break;
			case NavigationMoveEvent.Direction.Up:
				this.Invoke(KeyboardNavigationOperation.Previous, evt);
				break;
			case NavigationMoveEvent.Direction.Right:
				this.Invoke(KeyboardNavigationOperation.MoveRight, evt);
				break;
			case NavigationMoveEvent.Direction.Down:
				this.Invoke(KeyboardNavigationOperation.Next, evt);
				break;
			}
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00040A85 File Offset: 0x0003EC85
		private void Invoke(KeyboardNavigationOperation operation, EventBase evt)
		{
			Action<KeyboardNavigationOperation, EventBase> action = this.m_Action;
			if (action != null)
			{
				action(operation, evt);
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00040A9C File Offset: 0x0003EC9C
		[CompilerGenerated]
		internal static KeyboardNavigationOperation <OnKeyDown>g__GetOperation|4_0(ref KeyboardNavigationManipulator.<>c__DisplayClass4_0 A_0)
		{
			KeyCode keyCode = A_0.evt.keyCode;
			KeyCode keyCode2 = keyCode;
			if (keyCode2 != KeyCode.A)
			{
				switch (keyCode2)
				{
				case KeyCode.UpArrow:
				case KeyCode.DownArrow:
				case KeyCode.RightArrow:
				case KeyCode.LeftArrow:
					A_0.evt.StopPropagation();
					break;
				case KeyCode.Home:
					return KeyboardNavigationOperation.Begin;
				case KeyCode.End:
					return KeyboardNavigationOperation.End;
				case KeyCode.PageUp:
					return KeyboardNavigationOperation.PageUp;
				case KeyCode.PageDown:
					return KeyboardNavigationOperation.PageDown;
				}
			}
			else if (A_0.evt.actionKey)
			{
				return KeyboardNavigationOperation.SelectAll;
			}
			return KeyboardNavigationOperation.None;
		}

		// Token: 0x040007E3 RID: 2019
		private readonly Action<KeyboardNavigationOperation, EventBase> m_Action;
	}
}
