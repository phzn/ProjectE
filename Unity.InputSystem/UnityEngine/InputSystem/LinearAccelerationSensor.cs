using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200004C RID: 76
	[InputControlLayout(stateType = typeof(LinearAccelerationState), displayName = "Linear Acceleration")]
	public class LinearAccelerationSensor : Sensor
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0002DE95 File Offset: 0x0002C095
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0002DE9D File Offset: 0x0002C09D
		public Vector3Control acceleration { get; protected set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0002DEA6 File Offset: 0x0002C0A6
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0002DEAD File Offset: 0x0002C0AD
		public static LinearAccelerationSensor current { get; private set; }

		// Token: 0x06000801 RID: 2049 RVA: 0x0002DEB5 File Offset: 0x0002C0B5
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			LinearAccelerationSensor.current = this;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002DEC3 File Offset: 0x0002C0C3
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (LinearAccelerationSensor.current == this)
			{
				LinearAccelerationSensor.current = null;
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002DED9 File Offset: 0x0002C0D9
		protected override void FinishSetup()
		{
			this.acceleration = base.GetChildControl<Vector3Control>("acceleration");
			base.FinishSetup();
		}
	}
}
