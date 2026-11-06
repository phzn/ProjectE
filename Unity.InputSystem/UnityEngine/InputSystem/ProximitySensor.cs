using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000050 RID: 80
	[InputControlLayout(displayName = "Proximity")]
	public class ProximitySensor : Sensor
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002E029 File Offset: 0x0002C229
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0002E031 File Offset: 0x0002C231
		[InputControl(displayName = "Distance", noisy = true)]
		public AxisControl distance { get; protected set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0002E03A File Offset: 0x0002C23A
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0002E041 File Offset: 0x0002C241
		public static ProximitySensor current { get; private set; }

		// Token: 0x06000821 RID: 2081 RVA: 0x0002E049 File Offset: 0x0002C249
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			ProximitySensor.current = this;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0002E057 File Offset: 0x0002C257
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (ProximitySensor.current == this)
			{
				ProximitySensor.current = null;
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0002E06D File Offset: 0x0002C26D
		protected override void FinishSetup()
		{
			this.distance = base.GetChildControl<AxisControl>("distance");
			base.FinishSetup();
		}
	}
}
