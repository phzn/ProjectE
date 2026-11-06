using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000051 RID: 81
	[InputControlLayout(displayName = "Humidity")]
	public class HumiditySensor : Sensor
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0002E08E File Offset: 0x0002C28E
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0002E096 File Offset: 0x0002C296
		[InputControl(displayName = "Relative Humidity", noisy = true)]
		public AxisControl relativeHumidity { get; protected set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0002E09F File Offset: 0x0002C29F
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0002E0A6 File Offset: 0x0002C2A6
		public static HumiditySensor current { get; private set; }

		// Token: 0x06000829 RID: 2089 RVA: 0x0002E0AE File Offset: 0x0002C2AE
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			HumiditySensor.current = this;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002E0BC File Offset: 0x0002C2BC
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (HumiditySensor.current == this)
			{
				HumiditySensor.current = null;
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002E0D2 File Offset: 0x0002C2D2
		protected override void FinishSetup()
		{
			this.relativeHumidity = base.GetChildControl<AxisControl>("relativeHumidity");
			base.FinishSetup();
		}
	}
}
