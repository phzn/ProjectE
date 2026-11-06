using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000041 RID: 65
	[InputControlLayout(stateType = typeof(PointerState), isGenericTypeOfDevice = true)]
	public class Pointer : InputDevice, IInputStateCallbackReceiver
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00015D41 File Offset: 0x00013F41
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00015D49 File Offset: 0x00013F49
		public Vector2Control position { get; protected set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00015D52 File Offset: 0x00013F52
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x00015D5A File Offset: 0x00013F5A
		public DeltaControl delta { get; protected set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00015D63 File Offset: 0x00013F63
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x00015D6B File Offset: 0x00013F6B
		public Vector2Control radius { get; protected set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x00015D74 File Offset: 0x00013F74
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x00015D7C File Offset: 0x00013F7C
		public AxisControl pressure { get; protected set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00015D85 File Offset: 0x00013F85
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x00015D8D File Offset: 0x00013F8D
		public ButtonControl press { get; protected set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00015D96 File Offset: 0x00013F96
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x00015D9E File Offset: 0x00013F9E
		public IntegerControl displayIndex { get; protected set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00015DA7 File Offset: 0x00013FA7
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x00015DAE File Offset: 0x00013FAE
		public static Pointer current { get; internal set; }

		// Token: 0x060005D2 RID: 1490 RVA: 0x00015DB6 File Offset: 0x00013FB6
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Pointer.current = this;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00015DC4 File Offset: 0x00013FC4
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Pointer.current == this)
			{
				Pointer.current = null;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00015DDC File Offset: 0x00013FDC
		protected override void FinishSetup()
		{
			this.position = base.GetChildControl<Vector2Control>("position");
			this.delta = base.GetChildControl<DeltaControl>("delta");
			this.radius = base.GetChildControl<Vector2Control>("radius");
			this.pressure = base.GetChildControl<AxisControl>("pressure");
			this.press = base.GetChildControl<ButtonControl>("press");
			this.displayIndex = base.GetChildControl<IntegerControl>("displayIndex");
			base.FinishSetup();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00015E58 File Offset: 0x00014058
		protected void OnNextUpdate()
		{
			InputState.Change<Vector2>(this.delta, Vector2.zero, InputUpdateType.None, default(InputEventPtr));
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00015E7F File Offset: 0x0001407F
		protected void OnStateEvent(InputEventPtr eventPtr)
		{
			this.delta.AccumulateValueInEvent(base.currentStatePtr, eventPtr);
			InputState.Change(this, eventPtr, InputUpdateType.None);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00015E9B File Offset: 0x0001409B
		void IInputStateCallbackReceiver.OnNextUpdate()
		{
			this.OnNextUpdate();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00015EA3 File Offset: 0x000140A3
		void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
		{
			this.OnStateEvent(eventPtr);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00015EAC File Offset: 0x000140AC
		bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
		{
			return false;
		}
	}
}
