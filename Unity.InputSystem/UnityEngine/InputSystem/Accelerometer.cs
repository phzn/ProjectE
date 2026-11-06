using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000048 RID: 72
	[InputControlLayout(stateType = typeof(AccelerometerState))]
	public class Accelerometer : Sensor
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002DD01 File Offset: 0x0002BF01
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x0002DD09 File Offset: 0x0002BF09
		public Vector3Control acceleration { get; protected set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002DD12 File Offset: 0x0002BF12
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0002DD19 File Offset: 0x0002BF19
		public static Accelerometer current { get; private set; }

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002DD21 File Offset: 0x0002BF21
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Accelerometer.current = this;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002DD2F File Offset: 0x0002BF2F
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Accelerometer.current == this)
			{
				Accelerometer.current = null;
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002DD45 File Offset: 0x0002BF45
		protected override void FinishSetup()
		{
			this.acceleration = base.GetChildControl<Vector3Control>("acceleration");
			base.FinishSetup();
		}
	}
}
