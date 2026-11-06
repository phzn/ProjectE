using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000053 RID: 83
	[InputControlLayout(displayName = "Step Counter")]
	public class StepCounter : Sensor
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0002E158 File Offset: 0x0002C358
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0002E160 File Offset: 0x0002C360
		[InputControl(displayName = "Step Counter", noisy = true)]
		public IntegerControl stepCounter { get; protected set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0002E169 File Offset: 0x0002C369
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0002E170 File Offset: 0x0002C370
		public static StepCounter current { get; private set; }

		// Token: 0x06000839 RID: 2105 RVA: 0x0002E178 File Offset: 0x0002C378
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			StepCounter.current = this;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002E186 File Offset: 0x0002C386
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (StepCounter.current == this)
			{
				StepCounter.current = null;
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002E19C File Offset: 0x0002C39C
		protected override void FinishSetup()
		{
			this.stepCounter = base.GetChildControl<IntegerControl>("stepCounter");
			base.FinishSetup();
		}
	}
}
