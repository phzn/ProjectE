using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200003E RID: 62
	[InputControlLayout(stateType = typeof(MouseState), isGenericTypeOfDevice = true)]
	public class Mouse : Pointer, IInputStateCallbackReceiver
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00015951 File Offset: 0x00013B51
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00015959 File Offset: 0x00013B59
		public DeltaControl scroll { get; protected set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00015962 File Offset: 0x00013B62
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x0001596A File Offset: 0x00013B6A
		public ButtonControl leftButton { get; protected set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00015973 File Offset: 0x00013B73
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x0001597B File Offset: 0x00013B7B
		public ButtonControl middleButton { get; protected set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00015984 File Offset: 0x00013B84
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0001598C File Offset: 0x00013B8C
		public ButtonControl rightButton { get; protected set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00015995 File Offset: 0x00013B95
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x0001599D File Offset: 0x00013B9D
		public ButtonControl backButton { get; protected set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x000159A6 File Offset: 0x00013BA6
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x000159AE File Offset: 0x00013BAE
		public ButtonControl forwardButton { get; protected set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x000159B7 File Offset: 0x00013BB7
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x000159BF File Offset: 0x00013BBF
		public IntegerControl clickCount { get; protected set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x000159C8 File Offset: 0x00013BC8
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x000159CF File Offset: 0x00013BCF
		public new static Mouse current { get; private set; }

		// Token: 0x060005A1 RID: 1441 RVA: 0x000159D7 File Offset: 0x00013BD7
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Mouse.current = this;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x000159E5 File Offset: 0x00013BE5
		protected override void OnAdded()
		{
			base.OnAdded();
			if (base.native && Mouse.s_PlatformMouseDevice == null)
			{
				Mouse.s_PlatformMouseDevice = this;
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00015A02 File Offset: 0x00013C02
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Mouse.current == this)
			{
				Mouse.current = null;
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00015A18 File Offset: 0x00013C18
		public void WarpCursorPosition(Vector2 position)
		{
			WarpMousePositionCommand warpMousePositionCommand = WarpMousePositionCommand.Create(position);
			base.ExecuteCommand<WarpMousePositionCommand>(ref warpMousePositionCommand);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00015A38 File Offset: 0x00013C38
		protected override void FinishSetup()
		{
			this.scroll = base.GetChildControl<DeltaControl>("scroll");
			this.leftButton = base.GetChildControl<ButtonControl>("leftButton");
			this.middleButton = base.GetChildControl<ButtonControl>("middleButton");
			this.rightButton = base.GetChildControl<ButtonControl>("rightButton");
			this.forwardButton = base.GetChildControl<ButtonControl>("forwardButton");
			this.backButton = base.GetChildControl<ButtonControl>("backButton");
			base.displayIndex = base.GetChildControl<IntegerControl>("displayIndex");
			this.clickCount = base.GetChildControl<IntegerControl>("clickCount");
			base.FinishSetup();
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00015AD4 File Offset: 0x00013CD4
		protected new void OnNextUpdate()
		{
			base.OnNextUpdate();
			InputState.Change<Vector2>(this.scroll, Vector2.zero, InputUpdateType.None, default(InputEventPtr));
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00015B01 File Offset: 0x00013D01
		protected new void OnStateEvent(InputEventPtr eventPtr)
		{
			this.scroll.AccumulateValueInEvent(base.currentStatePtr, eventPtr);
			base.OnStateEvent(eventPtr);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00015B1C File Offset: 0x00013D1C
		void IInputStateCallbackReceiver.OnNextUpdate()
		{
			this.OnNextUpdate();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00015B24 File Offset: 0x00013D24
		void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
		{
			this.OnStateEvent(eventPtr);
		}

		// Token: 0x04000226 RID: 550
		internal static Mouse s_PlatformMouseDevice;
	}
}
