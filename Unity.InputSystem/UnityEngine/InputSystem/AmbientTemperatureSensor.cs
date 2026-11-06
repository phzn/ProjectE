using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000052 RID: 82
	[InputControlLayout(displayName = "Ambient Temperature")]
	public class AmbientTemperatureSensor : Sensor
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0002E0F3 File Offset: 0x0002C2F3
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0002E0FB File Offset: 0x0002C2FB
		[InputControl(displayName = "Ambient Temperature", noisy = true)]
		public AxisControl ambientTemperature { get; protected set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0002E104 File Offset: 0x0002C304
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0002E10B File Offset: 0x0002C30B
		public static AmbientTemperatureSensor current { get; private set; }

		// Token: 0x06000831 RID: 2097 RVA: 0x0002E113 File Offset: 0x0002C313
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			AmbientTemperatureSensor.current = this;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002E121 File Offset: 0x0002C321
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (AmbientTemperatureSensor.current == this)
			{
				AmbientTemperatureSensor.current = null;
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002E137 File Offset: 0x0002C337
		protected override void FinishSetup()
		{
			this.ambientTemperature = base.GetChildControl<AxisControl>("ambientTemperature");
			base.FinishSetup();
		}
	}
}
