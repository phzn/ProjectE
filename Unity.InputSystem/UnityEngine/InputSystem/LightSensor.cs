using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200004E RID: 78
	[InputControlLayout(displayName = "Light")]
	public class LightSensor : Sensor
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0002DF5F File Offset: 0x0002C15F
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0002DF67 File Offset: 0x0002C167
		[InputControl(displayName = "Light Level", noisy = true)]
		public AxisControl lightLevel { get; protected set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0002DF70 File Offset: 0x0002C170
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0002DF77 File Offset: 0x0002C177
		public static LightSensor current { get; private set; }

		// Token: 0x06000811 RID: 2065 RVA: 0x0002DF7F File Offset: 0x0002C17F
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			LightSensor.current = this;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002DF8D File Offset: 0x0002C18D
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (LightSensor.current == this)
			{
				LightSensor.current = null;
			}
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002DFA3 File Offset: 0x0002C1A3
		protected override void FinishSetup()
		{
			this.lightLevel = base.GetChildControl<AxisControl>("lightLevel");
			base.FinishSetup();
		}
	}
}
