using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000054 RID: 84
	[InputControlLayout(displayName = "Hinge Angle")]
	public class HingeAngle : Sensor
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0002E1BD File Offset: 0x0002C3BD
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x0002E1C5 File Offset: 0x0002C3C5
		public AxisControl angle { get; protected set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0002E1CE File Offset: 0x0002C3CE
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x0002E1D5 File Offset: 0x0002C3D5
		public static HingeAngle current { get; private set; }

		// Token: 0x06000841 RID: 2113 RVA: 0x0002E1DD File Offset: 0x0002C3DD
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			HingeAngle.current = this;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002E1EB File Offset: 0x0002C3EB
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (HingeAngle.current == this)
			{
				HingeAngle.current = null;
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002E201 File Offset: 0x0002C401
		protected override void FinishSetup()
		{
			this.angle = base.GetChildControl<AxisControl>("angle");
			base.FinishSetup();
		}
	}
}
