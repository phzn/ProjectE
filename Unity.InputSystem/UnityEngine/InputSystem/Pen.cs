using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000040 RID: 64
	[InputControlLayout(stateType = typeof(PenState), isGenericTypeOfDevice = true)]
	public class Pen : Pointer
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00015B35 File Offset: 0x00013D35
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00015B3D File Offset: 0x00013D3D
		public ButtonControl tip { get; protected set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00015B46 File Offset: 0x00013D46
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00015B4E File Offset: 0x00013D4E
		public ButtonControl eraser { get; protected set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00015B57 File Offset: 0x00013D57
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x00015B5F File Offset: 0x00013D5F
		public ButtonControl firstBarrelButton { get; protected set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00015B68 File Offset: 0x00013D68
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x00015B70 File Offset: 0x00013D70
		public ButtonControl secondBarrelButton { get; protected set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00015B79 File Offset: 0x00013D79
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00015B81 File Offset: 0x00013D81
		public ButtonControl thirdBarrelButton { get; protected set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00015B8A File Offset: 0x00013D8A
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00015B92 File Offset: 0x00013D92
		public ButtonControl fourthBarrelButton { get; protected set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00015B9B File Offset: 0x00013D9B
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00015BA3 File Offset: 0x00013DA3
		public ButtonControl inRange { get; protected set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00015BAC File Offset: 0x00013DAC
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x00015BB4 File Offset: 0x00013DB4
		public Vector2Control tilt { get; protected set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00015BBD File Offset: 0x00013DBD
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x00015BC5 File Offset: 0x00013DC5
		public AxisControl twist { get; protected set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00015BCE File Offset: 0x00013DCE
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00015BD5 File Offset: 0x00013DD5
		public new static Pen current { get; internal set; }

		// Token: 0x170001F3 RID: 499
		public ButtonControl this[PenButton button]
		{
			get
			{
				switch (button)
				{
				case PenButton.Tip:
					return this.tip;
				case PenButton.Eraser:
					return this.eraser;
				case PenButton.BarrelFirst:
					return this.firstBarrelButton;
				case PenButton.BarrelSecond:
					return this.secondBarrelButton;
				case PenButton.InRange:
					return this.inRange;
				case PenButton.BarrelThird:
					return this.thirdBarrelButton;
				case PenButton.BarrelFourth:
					return this.fourthBarrelButton;
				default:
					throw new InvalidEnumArgumentException("button", (int)button, typeof(PenButton));
				}
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00015C57 File Offset: 0x00013E57
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Pen.current = this;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00015C65 File Offset: 0x00013E65
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Pen.current == this)
			{
				Pen.current = null;
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00015C7C File Offset: 0x00013E7C
		protected override void FinishSetup()
		{
			this.tip = base.GetChildControl<ButtonControl>("tip");
			this.eraser = base.GetChildControl<ButtonControl>("eraser");
			this.firstBarrelButton = base.GetChildControl<ButtonControl>("barrel1");
			this.secondBarrelButton = base.GetChildControl<ButtonControl>("barrel2");
			this.thirdBarrelButton = base.GetChildControl<ButtonControl>("barrel3");
			this.fourthBarrelButton = base.GetChildControl<ButtonControl>("barrel4");
			this.inRange = base.GetChildControl<ButtonControl>("inRange");
			this.tilt = base.GetChildControl<Vector2Control>("tilt");
			this.twist = base.GetChildControl<AxisControl>("twist");
			base.displayIndex = base.GetChildControl<IntegerControl>("displayIndex");
			base.FinishSetup();
		}
	}
}
