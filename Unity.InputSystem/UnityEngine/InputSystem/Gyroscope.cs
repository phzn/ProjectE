using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000049 RID: 73
	[InputControlLayout(stateType = typeof(GyroscopeState))]
	public class Gyroscope : Sensor
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0002DD66 File Offset: 0x0002BF66
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0002DD6E File Offset: 0x0002BF6E
		public Vector3Control angularVelocity { get; protected set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0002DD77 File Offset: 0x0002BF77
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x0002DD7E File Offset: 0x0002BF7E
		public static Gyroscope current { get; private set; }

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002DD86 File Offset: 0x0002BF86
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Gyroscope.current = this;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0002DD94 File Offset: 0x0002BF94
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Gyroscope.current == this)
			{
				Gyroscope.current = null;
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002DDAA File Offset: 0x0002BFAA
		protected override void FinishSetup()
		{
			this.angularVelocity = base.GetChildControl<Vector3Control>("angularVelocity");
			base.FinishSetup();
		}
	}
}
