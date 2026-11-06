using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200004D RID: 77
	[InputControlLayout(displayName = "Magnetic Field")]
	public class MagneticFieldSensor : Sensor
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0002DEFA File Offset: 0x0002C0FA
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x0002DF02 File Offset: 0x0002C102
		[InputControl(displayName = "Magnetic Field", noisy = true)]
		public Vector3Control magneticField { get; protected set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0002DF0B File Offset: 0x0002C10B
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x0002DF12 File Offset: 0x0002C112
		public static MagneticFieldSensor current { get; private set; }

		// Token: 0x06000809 RID: 2057 RVA: 0x0002DF1A File Offset: 0x0002C11A
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			MagneticFieldSensor.current = this;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002DF28 File Offset: 0x0002C128
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (MagneticFieldSensor.current == this)
			{
				MagneticFieldSensor.current = null;
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002DF3E File Offset: 0x0002C13E
		protected override void FinishSetup()
		{
			this.magneticField = base.GetChildControl<Vector3Control>("magneticField");
			base.FinishSetup();
		}
	}
}
