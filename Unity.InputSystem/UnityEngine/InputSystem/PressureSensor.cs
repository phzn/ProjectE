using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200004F RID: 79
	[InputControlLayout(displayName = "Pressure")]
	public class PressureSensor : Sensor
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x0002DFCC File Offset: 0x0002C1CC
		[InputControl(displayName = "Atmospheric Pressure", noisy = true)]
		public AxisControl atmosphericPressure { get; protected set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0002DFD5 File Offset: 0x0002C1D5
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0002DFDC File Offset: 0x0002C1DC
		public static PressureSensor current { get; private set; }

		// Token: 0x06000819 RID: 2073 RVA: 0x0002DFE4 File Offset: 0x0002C1E4
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			PressureSensor.current = this;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002DFF2 File Offset: 0x0002C1F2
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (PressureSensor.current == this)
			{
				PressureSensor.current = null;
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002E008 File Offset: 0x0002C208
		protected override void FinishSetup()
		{
			this.atmosphericPressure = base.GetChildControl<AxisControl>("atmosphericPressure");
			base.FinishSetup();
		}
	}
}
