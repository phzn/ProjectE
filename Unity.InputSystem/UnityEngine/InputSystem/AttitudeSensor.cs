using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200004B RID: 75
	[InputControlLayout(stateType = typeof(AttitudeState), displayName = "Attitude")]
	public class AttitudeSensor : Sensor
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002DE30 File Offset: 0x0002C030
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0002DE38 File Offset: 0x0002C038
		public QuaternionControl attitude { get; protected set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0002DE41 File Offset: 0x0002C041
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x0002DE48 File Offset: 0x0002C048
		public static AttitudeSensor current { get; private set; }

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002DE50 File Offset: 0x0002C050
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			AttitudeSensor.current = this;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002DE5E File Offset: 0x0002C05E
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (AttitudeSensor.current == this)
			{
				AttitudeSensor.current = null;
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002DE74 File Offset: 0x0002C074
		protected override void FinishSetup()
		{
			this.attitude = base.GetChildControl<QuaternionControl>("attitude");
			base.FinishSetup();
		}
	}
}
